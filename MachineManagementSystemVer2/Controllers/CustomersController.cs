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
    namespace MachineManagementSystemVer2.Controllers
    {
        [Authorize(Roles = "Engineer,Manager,Admin")]
        public class CustomersController : Controller
        {
            private readonly AppDbContext _context;

            public CustomersController(AppDbContext context)
            {
                _context = context;
            }

            // GET: Customers
            public async Task<IActionResult> Index()
            {
                return View(await _context.Customers.ToListAsync());
            }


            // GET: Customers/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null) return NotFound();

                var customer = await _context.Customers
                    .Include(c => c.Plants) 
                    .FirstOrDefaultAsync(m => m.CustomerId == id);

                if (customer == null) return NotFound();

                return View(customer);
            }

            // GET: Customers/Create
            public IActionResult Create()
            {
                return View(new CustomerCreateViewModel());
            }

            // POST: Customers/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(CustomerCreateViewModel viewModel)
            {
                if (ModelState.IsValid)
                {
                    var customer = new Customer
                    {
                        CustomerName = viewModel.CustomerName,
                        CustomerTaxId = viewModel.CustomerTaxId,
                        CustomerAddress = viewModel.CustomerAddress,
                        CustomerPhone = viewModel.CustomerPhone
                    };

                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(viewModel);
            }


            // GET: Customers/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null) return NotFound();

                var customer = await _context.Customers
                    .Include(c => c.Plants)
                    .FirstOrDefaultAsync(c => c.CustomerId == id);
                if (customer == null) return NotFound();

                return View(customer);
            }


            // POST: Customers/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, CustomerEditViewModel viewModel)
            {
                if (id != viewModel.CustomerId) return NotFound();

                if (ModelState.IsValid)
                {
                    try
                    {
                        var customer = await _context.Customers.FindAsync(id);
                        if (customer == null) return NotFound();

                        customer.CustomerName = viewModel.CustomerName;
                        customer.CustomerTaxId = viewModel.CustomerTaxId;
                        customer.CustomerAddress = viewModel.CustomerAddress;
                        customer.CustomerPhone = viewModel.CustomerPhone;

                        _context.Update(customer);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CustomerExists(viewModel.CustomerId)) return NotFound();
                        else throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
                // 如果驗證失敗，需要重新載入廠區資料才能正確顯示頁面
                var customerWithPlants = await _context.Customers.Include(c => c.Plants).FirstOrDefaultAsync(c => c.CustomerId == id);
                return View(customerWithPlants);
            }


            // --- AJAX Action to Add a Plant ---
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> AddPlant(Plant plant)
            {
                // We remove the manual check for CustomerId existence because
                // the model binding and validation will handle it if CustomerId is required in the Plant model.

                // 【修正】我們需要確保 PlantCode 即使是選填，也能被正確處理
                // 如果 Plant Model 中 PlantCode 是可為 null 的 (string?), 這裡就不會有問題。
                // 如果是必填，前端表單就必須提供這個欄位。
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Plants.Add(plant);
                        await _context.SaveChangesAsync();
                        // Return the full plant object on success
                        return Json(new { success = true, plant });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = ex.Message });
                    }
                }
                // If model validation fails, collect and return the errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "輸入資料有誤。", errors });
            }

            private bool CustomerExists(int id)
            {
                return _context.Customers.Any(e => e.CustomerId == id);
            }


            }
        }
    }
