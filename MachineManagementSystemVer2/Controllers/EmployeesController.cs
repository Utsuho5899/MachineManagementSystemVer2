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
    }
}