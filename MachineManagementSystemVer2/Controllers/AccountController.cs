using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MachineManagementSystemVer2.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous] 
        public IActionResult Login(string returnUrl = "/")
        {
            // 如果使用者已經登入，直接導向首頁
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect(returnUrl);
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            if (ModelState.IsValid)
            {
             
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Account == model.Account && e.Password == model.Password);

                if (employee != null)
                {
                    // --- 【兩段式保險】第一道防線：檢查員工狀態 ---
                    if (employee.Status != "在職")
                    {
                        ModelState.AddModelError(string.Empty, "此帳號已被停用。");
                        return View(model);
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),               
                        new Claim(ClaimTypes.Name, employee.EmployeeName),
                        new Claim(ClaimTypes.Role, employee.Role)
                    };

                    
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // 可以設定 Cookie 是否為永久性、過期時間等
                        IsPersistent = true,
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return LocalRedirect(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "無效的帳號或密碼。");
            }
            return View(model);
        }

        // --- 【新增】修改密碼功能 ---

        [Authorize] // 只有登入的使用者才能存取
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _context.Employees.FindAsync(int.Parse(userId));

            if (employee == null) return NotFound("找不到使用者。");

            // 【還原】改回直接比對舊的明碼密碼
            if (employee.Password != model.OldPassword)
            {
                ModelState.AddModelError("OldPassword", "目前的密碼不正確。");
                return View(model);
            }

            // 【還原】直接儲存新的明碼密碼
            employee.Password = model.NewPassword;
            _context.Update(employee);
            await _context.SaveChangesAsync();

            ViewData["SuccessMessage"] = "密碼已成功更新！";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
