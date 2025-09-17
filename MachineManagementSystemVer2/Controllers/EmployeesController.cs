using Microsoft.AspNetCore.Mvc;
using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using MachineManagementSystemVer2.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;

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
                Remarks = employee.Remarks,
                Role = employee.Role,
                Status = employee.Status
            };
            return View(viewModel);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeEditViewModel viewModel)
        {
            if (id != viewModel.EmployeeId) return NotFound();

            var employeeToUpdate = await _context.Employees.FindAsync(id);
            if (employeeToUpdate == null) return NotFound();

            if (!User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                viewModel.Status = employeeToUpdate.Status;
            }

            if (ModelState.IsValid)
            {
                try
                {
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