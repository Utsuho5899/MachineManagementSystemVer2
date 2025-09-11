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
            public async Task<IActionResult> Index(string searchName, string searchTaxId)
            {
                var customersQuery = _context.Customers.AsQueryable();

                if (!string.IsNullOrEmpty(searchName))
                {
                    customersQuery = customersQuery.Where(c => c.CustomerName.Contains(searchName));
                }

                if (!string.IsNullOrEmpty(searchTaxId))
                {
                    customersQuery = customersQuery.Where(c => c.CustomerTaxId == searchTaxId);
                }

                ViewBag.CurrentFilterName = searchName;
                ViewBag.CurrentFilterTaxId = searchTaxId;

                return View(await customersQuery.ToListAsync());
            }

            // GET: Customers/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var customer = await _context.Customers
                    .Include(c => c.Plants)
                    .FirstOrDefaultAsync(m => m.CustomerId == id);

                if (customer == null)
                {
                    return NotFound();
                }

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
                    return RedirectToAction("Edit", new { id = customer.CustomerId });
                }

                // 【修正】如果驗證失敗，將 viewModel 物件本身傳回 View，而不是 customer
                return View(viewModel);
            }


            // --- 補上的 Edit (GET) 方法 ---
            // GET: Customers/Edit/5
            // 顯示編輯客戶的表單，並載入其關聯的廠區資料
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                // 使用 Include 一次性載入客戶及其所有廠區
                var customer = await _context.Customers.Include(c => c.Plants)
                                                 .FirstOrDefaultAsync(c => c.CustomerId == id);

                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }


            // POST: Customers/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerName,CustomerTaxId,CustomerAddress,CustomerPhone")] Customer customer)
            {
                if (id != customer.CustomerId)
                {
                    return NotFound();
                }

                // Find the existing customer from the database
                var customerToUpdate = await _context.Customers
                    .Include(c => c.Plants)
                    .FirstOrDefaultAsync(c => c.CustomerId == id);

                if (customerToUpdate == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    // Update only the properties that are bound from the form
                    customerToUpdate.CustomerName = customer.CustomerName;
                    customerToUpdate.CustomerTaxId = customer.CustomerTaxId;
                    customerToUpdate.CustomerAddress = customer.CustomerAddress;
                    customerToUpdate.CustomerPhone = customer.CustomerPhone;

                    try
                    {
                        _context.Update(customerToUpdate);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CustomerExists(customer.CustomerId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                // If ModelState is invalid, return the view with the customer data including plants
                return View(customerToUpdate);
            }

            // --- 補上的 Delete (GET) 方法 ---
            // GET: Customers/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(m => m.CustomerId == id);
                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }

            // --- 補上的 Delete (POST) 方法 ---
            // POST: Customers/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // --- 補上的輔助方法 ---
            private bool CustomerExists(int id)
            {
                return _context.Customers.Any(e => e.CustomerId == id);
            }
        }
    }
}