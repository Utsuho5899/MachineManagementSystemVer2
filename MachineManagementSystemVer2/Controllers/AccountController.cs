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
using MachineManagementSystemVer2.Models;

namespace MachineManagementSystemVer2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;

        // 【修改】注入密碼雜湊器
        public AccountController(UserManager<Employee> userManager, SignInManager<Employee> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public AccountController(AppDbContext context)
        //{
        //    _context = context;
        //}

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
                // 第一步：只用帳號找出使用者
                var user = await _userManager.FindByNameAsync(model.Account);

                // 第二步：如果使用者存在，就用 UserManager 檢查密碼
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    // 第三步：檢查員工狀態
                    if (user.Status != "在職")
                    {
                        ModelState.AddModelError(string.Empty, "此帳號已被停用。");
                        return View(model);
                    }

                    // 第四步：手動建立包含角色資訊的 Claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.EmployeeName),
                        new Claim(ClaimTypes.Role, user.Role) // <-- 將角色資訊加入！

                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, IdentityConstants.ApplicationScheme);


                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true // 實現「記住我」的功能
                    };

                    // 第五步：使用包含完整資訊的 Claims 進行登入
                    await HttpContext.SignInAsync(
                          IdentityConstants.ApplicationScheme,
                          new ClaimsPrincipal(claimsIdentity),
                          authProperties);

                    return LocalRedirect(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "無效的帳號或密碼。");
            }
            return View(model);
        }


        // --- 【新增】修改密碼功能 ---

        [Authorize]
            public IActionResult ChangePassword()
            {
                return View();
            }

            [HttpPost]
            [Authorize]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
            {
                if (!ModelState.IsValid) return View(model);

                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound("找不到使用者。");

                // 【修改】使用 UserManager 更改密碼
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    ViewData["SuccessMessage"] = "密碼已成功更新！";
                    return View(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Logout()
            {
                // 【修改】使用 SignInManager 登出
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }
        }
}
