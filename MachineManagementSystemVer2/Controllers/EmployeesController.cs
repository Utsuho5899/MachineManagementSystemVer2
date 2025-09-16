using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using MachineManagementSystemVer2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MachineManagementSystemVer2.Controllers
{
    [Authorize(Roles = "HR,Admin")]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
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
            return View();
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
                    HireDate = viewModel.HireDate,
                    EmployeeTitle = viewModel.EmployeeTitle,
                    EmployeeAddress = viewModel.EmployeeAddress,
                    EmployeePhone = viewModel.EmployeePhone,
                    EmergencyContact = viewModel.EmergencyContact,
                    EmergencyPhone = viewModel.EmergencyPhone,
                    Account = viewModel.Account,
                    Password = viewModel.Password,
                    Remarks = viewModel.Remarks
                };

                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }
        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            var viewModel = new EmployeeEditViewModel
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.EmployeeName,
                HireDate = employee.HireDate,
                EmployeeTitle = employee.EmployeeTitle,
                Role = employee.Role,
                Status = employee.Status,
                EmployeeAddress = employee.EmployeeAddress,
                EmployeePhone = employee.EmployeePhone,
                EmergencyContact = employee.EmergencyContact,
                EmergencyPhone = employee.EmergencyPhone,
                Account = employee.Account,
                Remarks = employee.Remarks
            };
            return View(viewModel);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeEditViewModel viewModel)
        {
            if (id != viewModel.EmployeeId) return NotFound();

            // 先從資料庫讀取原始員工資料，用於權限檢查和後續更新
            var employeeToUpdate = await _context.Employees.FindAsync(id);
            if (employeeToUpdate == null) return NotFound();

            // 【原則 2】後端安全檢查：如果目前登入者不是 Admin 或 HR，
            // 就算前端的表單被惡意修改並送出了 Status 資料，我們也強制將它設回原本的狀態。
            if (!User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                viewModel.Status = employeeToUpdate.Status;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 更新欄位
                    employeeToUpdate.EmployeeName = viewModel.EmployeeName;
                    employeeToUpdate.HireDate = viewModel.HireDate;
                    employeeToUpdate.EmployeeTitle = viewModel.EmployeeTitle;
                    employeeToUpdate.EmployeeAddress = viewModel.EmployeeAddress;
                    employeeToUpdate.EmployeePhone = viewModel.EmployeePhone;
                    employeeToUpdate.EmergencyContact = viewModel.EmergencyContact;
                    employeeToUpdate.EmergencyPhone = viewModel.EmergencyPhone;
                    employeeToUpdate.Account = viewModel.Account;
                    employeeToUpdate.Remarks = viewModel.Remarks;
                    employeeToUpdate.Role = viewModel.Role;
                    employeeToUpdate.Status = viewModel.Status;

                    if (!string.IsNullOrEmpty(viewModel.NewPassword))
                    {
                        employeeToUpdate.Password = viewModel.NewPassword;
                    }

                    _context.Update(employeeToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(viewModel.EmployeeId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

       

        // GET: Employees/Delete/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // POST: Employees/Delete/5
        [Authorize(Roles = "Admin")]
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

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}