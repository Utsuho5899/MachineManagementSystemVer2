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

        [AllowAnonymous] // 允許未登入的使用者存取此頁面
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                // 在 Employee 資料表中尋找符合帳號的使用者
                // 實際應用中，密碼應進行雜湊比對
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Account == model.Account && e.Password == model.Password);

                if (employee != null)
                {
                    // --- 建立使用者的身份聲明 (Claims) ---
                    var claims = new List<Claim>
                    {
                        // NameIdentifier 是使用者的唯一識別碼，我們用 EmployeeId
                        new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
                        // Name 是使用者的顯示名稱
                        new Claim(ClaimTypes.Name, employee.EmployeeName),
                        // 可以在這裡加入其他資訊，例如使用者的角色 (Role)
                        // new Claim(ClaimTypes.Role, "Admin"), 
                    };

                    // --- 建立身份 principals 並簽發 Cookie ---
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

                // 如果找不到使用者或密碼錯誤
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
            return RedirectToAction("Index", "Home");
        }
    }
}
