using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using MachineManagementSystemVer2.ViewModels; // 引用 ViewModel 命名空間
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MachineManagementSystemVer2.Controllers
{
    [Authorize(Roles = "Engineer,Manager,Admin")]
    public class PlantsController : Controller
    {
        private readonly AppDbContext _context;

        public PlantsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Plants/Create?customerId=5
        public async Task<IActionResult> Create(int customerId)
        {
            if (customerId == 0)
            {
                return BadRequest("必須提供客戶ID。");
            }

            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return NotFound("找不到指定的客戶。");
            }

            // 建立 ViewModel 並填入需要的資料
            var viewModel = new PlantCreateViewModel
            {
                CustomerId = customerId,
                CustomerName = customer.CustomerName 
            };

            return View(viewModel);
        }

        // POST: Plants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlantCreateViewModel viewModel)
        {
            
            if (ModelState.IsValid)
            {
                var plant = new Plant
                {
                    PlantName = viewModel.PlantName,
                    PlantCode = viewModel.PlantCode,
                    PlantAddress = viewModel.PlantAddress,
                    PlantPhone = viewModel.PlantPhone,
                    CustomerId = viewModel.CustomerId 
                };

                _context.Add(plant);
                await _context.SaveChangesAsync();

                // 成功後，使用 ViewModel 中的 CustomerId 導向回正確的客戶編輯頁面
                return RedirectToAction("Edit", "Customers", new { id = viewModel.CustomerId });
            }

            // 如果驗證失敗，需要重新載入客戶名稱，才能在返回的頁面上正確顯示
            if (viewModel.CustomerId != 0)
            {
                var customer = await _context.Customers.FindAsync(viewModel.CustomerId);
                if (customer != null)
                {
                    viewModel.CustomerName = customer.CustomerName;
                }
            }
            // 將包含錯誤訊息的 viewModel 傳回 View
            return View(viewModel);
        }

        // GET: Plants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var plant = await _context.Plants.FindAsync(id);
            if (plant == null) return NotFound();

            var viewModel = new PlantEditViewModel
            {
                PlantId = plant.PlantId,
                PlantName = plant.PlantName,
                PlantAddress = plant.PlantAddress,
                PlantPhone = plant.PlantPhone,
                CustomerId = plant.CustomerId // 傳遞 CustomerId 以便返回
            };
            return View(viewModel);
        }

        // POST: Plants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlantEditViewModel viewModel)
        {
            if (id != viewModel.PlantId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var plant = await _context.Plants.FindAsync(id);
                    if (plant == null) return NotFound();

                    plant.PlantName = viewModel.PlantName;
                    plant.PlantAddress = viewModel.PlantAddress;
                    plant.PlantPhone = viewModel.PlantPhone;

                    _context.Update(plant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Plants.Any(e => e.PlantId == viewModel.PlantId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // 編輯完成後，返回到所屬客戶的編輯頁面
                return RedirectToAction("Edit", "Customers", new { id = viewModel.CustomerId });
            }
            return View(viewModel);
        }
    }
}