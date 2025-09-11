using Microsoft.AspNetCore.Mvc;
using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using MachineManagementSystemVer2.ViewModels; // 引用 ViewModel 命名空間
using Microsoft.EntityFrameworkCore;

namespace MachineManagementSystemVer2.Controllers
{
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
                CustomerName = customer.CustomerName // 將客戶名稱傳給 View 以供顯示
            };

            return View(viewModel);
        }

        // POST: Plants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlantCreateViewModel viewModel)
        {
            // 現在，控制器接收的是乾淨的 ViewModel
            if (ModelState.IsValid)
            {
                // 手動將 ViewModel 的資料轉換(映射)到真正的資料庫模型 Plant
                var plant = new Plant
                {
                    PlantName = viewModel.PlantName,
                    PlantCode = viewModel.PlantCode,
                    PlantAddress = viewModel.PlantAddress,
                    PlantPhone = viewModel.PlantPhone,
                    CustomerId = viewModel.CustomerId // 關鍵：確保 CustomerId 被正確賦值
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
    }
}