using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.ViewModels;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // 登出，清除 Cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
