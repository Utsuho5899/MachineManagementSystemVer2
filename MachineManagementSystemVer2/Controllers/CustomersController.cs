using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using MachineManagementSystemVer2.ViewModels;
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
                    .Include(c => c.Plants) // 載入此客戶旗下的所有廠區
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

                //var viewModel = new CustomerEditViewModel
                //{
                //    CustomerId = customer.CustomerId,
                //    CustomerName = customer.CustomerName,
                //    CustomerTaxId = customer.CustomerTaxId,
                //    CustomerAddress = customer.CustomerAddress,
                //    CustomerPhone = customer.CustomerPhone
                //};
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

                // --- 【新增】用於處理 AJAX 新增廠區的 Action ---
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> AddPlant(Plant plant)
                {
                    // 為了安全性，再次檢查傳入的 CustomerId 是否存在
                    if (!await _context.Customers.AnyAsync(c => c.CustomerId == plant.CustomerId))
                    {
                        return Json(new { success = false, message = "無效的客戶ID。" });
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Plants.Add(plant);
                            await _context.SaveChangesAsync();
                            return Json(new { success = true, plant });
                        }
                        catch (Exception ex)
                        {
                            return Json(new { success = false, message = ex.Message });
                        }
                    }
                    // 如果模型驗證失敗，回傳錯誤訊息
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = "輸入資料有誤。", errors = errors });
                }

        private bool CustomerExists(int id)
            {
                return _context.Customers.Any(e => e.CustomerId == id);
            }
        }
        }
    }
