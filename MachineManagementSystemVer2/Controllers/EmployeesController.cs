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
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // 【新增】補上 Index 方法
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

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = await _context.Employees.FindAsync(id);
                    if (employee == null) return NotFound();

                    employee.EmployeeName = viewModel.EmployeeName;
                    employee.HireDate = viewModel.HireDate;
                    employee.EmployeeTitle = viewModel.EmployeeTitle;
                    employee.EmployeeAddress = viewModel.EmployeeAddress;
                    employee.EmployeePhone = viewModel.EmployeePhone;
                    employee.EmergencyContact = viewModel.EmergencyContact;
                    employee.EmergencyPhone = viewModel.EmergencyPhone;
                    employee.Account = viewModel.Account;
                    employee.Remarks = viewModel.Remarks;

                    if (!string.IsNullOrEmpty(viewModel.NewPassword))
                    {
                        employee.Password = viewModel.NewPassword;
                    }

                    _context.Update(employee);
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

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}