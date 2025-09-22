using Microsoft.AspNetCore.Mvc;
using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using MachineManagementSystemVer2.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace MachineManagementSystemVer2.Controllers
{
    [Authorize(Roles = "HR,Admin")]
    public class EmployeesController : Controller
    {
        private readonly UserManager<Employee> _userManager; // 【修改】改用 UserManager
        private readonly AppDbContext _context; // 保留 DbContext 用於非使用者相關查詢

        // 【修改】注入密碼雜湊器
        public EmployeesController(UserManager<Employee> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View(new EmployeeCreateViewModel());
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    EmployeeName = viewModel.EmployeeName,
                    Role = viewModel.Role,
                    Status = "在職",
                    HireDate = viewModel.HireDate,
                    EmployeeTitle = viewModel.EmployeeTitle,
                    EmployeeAddress = viewModel.EmployeeAddress,
                    EmployeePhone = viewModel.EmployeePhone,
                    EmergencyContact = viewModel.EmergencyContact,
                    EmergencyPhone = viewModel.EmergencyPhone,
                    Account = viewModel.Account,
                    //Password = viewModel.Password,
                    Remarks = viewModel.Remarks
                };
                // 【修改】使用 UserManager 建立使用者，它會自動處理密碼雜湊
                var result = await _userManager.CreateAsync(employee, viewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(viewModel);
        }
        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(string id) // 【修改】Id 現在是 string
        {
            if (id == null) return NotFound();

            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null) return NotFound();

            var viewModel = new EmployeeEditViewModel
            {
                EmployeeId = employee.Id, // 【修改】對應 IdentityUser 的 Id
                EmployeeName = employee.EmployeeName,
                HireDate = employee.HireDate,
                EmployeeTitle = employee.EmployeeTitle,
                EmployeeAddress = employee.EmployeeAddress,
                EmployeePhone = employee.EmployeePhone,
                EmergencyContact = employee.EmergencyContact,
                EmergencyPhone = employee.EmergencyPhone,
                Account = employee.UserName, // 【修改】對應 UserName
                Remarks = employee.Remarks,
                Role = employee.Role,
                Status = employee.Status
            };
            return View(viewModel);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EmployeeEditViewModel viewModel)
        {
            if (id != viewModel.EmployeeId) return NotFound();

            if (ModelState.IsValid)
            {
                var userToUpdate = await _userManager.FindByIdAsync(id);
                if (userToUpdate == null) return NotFound("找不到指定的使用者。");

                // 更新自訂的員工資料
                userToUpdate.EmployeeName = viewModel.EmployeeName;
                userToUpdate.HireDate = viewModel.HireDate;
                userToUpdate.EmployeeTitle = viewModel.EmployeeTitle;
                userToUpdate.EmployeeAddress = viewModel.EmployeeAddress;
                userToUpdate.EmployeePhone = viewModel.EmployeePhone;
                userToUpdate.EmergencyContact = viewModel.EmergencyContact;
                userToUpdate.EmergencyPhone = viewModel.EmergencyPhone;
                userToUpdate.UserName = viewModel.Account; // 【修改】更新 UserName
                userToUpdate.Remarks = viewModel.Remarks;
                userToUpdate.Role = viewModel.Role;
                userToUpdate.Status = viewModel.Status;

                // 使用 UserManager 更新基本資料
                var result = await _userManager.UpdateAsync(userToUpdate);

                if (!result.Succeeded)
                {
                    // 如果更新失敗，將錯誤訊息加到 ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(viewModel);
                }

                // 如果提供了新密碼，則單獨更新密碼
                if (!string.IsNullOrEmpty(viewModel.NewPassword))
                {
                    // 移除舊密碼，再設定新密碼
                    var removePasswordResult = await _userManager.RemovePasswordAsync(userToUpdate);
                    if (removePasswordResult.Succeeded)
                    {
                        var addPasswordResult = await _userManager.AddPasswordAsync(userToUpdate, viewModel.NewPassword);
                        if (!addPasswordResult.Succeeded)
                        {
                            // 如果新增密碼失敗，將錯誤訊息加到 ModelState
                            foreach (var error in addPasswordResult.Errors)
                            {
                                ModelState.AddModelError("NewPassword", error.Description);
                            }
                            return View(viewModel);
                        }
                    }
                    else
                    {
                        // 如果移除舊密碼失敗 (通常發生在使用者是用外部登入，本來就沒密碼)
                        // 也可以直接新增密碼
                        var addPasswordResult = await _userManager.AddPasswordAsync(userToUpdate, viewModel.NewPassword);
                        if (!addPasswordResult.Succeeded)
                        {
                            foreach (var error in addPasswordResult.Errors)
                            {
                                ModelState.AddModelError("NewPassword", error.Description);
                            }
                            return View(viewModel);
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 【修正】確保整個 Controller 中只有這一個 EmployeeExists 方法
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}