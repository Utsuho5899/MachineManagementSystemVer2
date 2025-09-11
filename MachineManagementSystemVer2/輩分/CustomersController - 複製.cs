//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using MachineManagementSystemVer2.Data;
//using MachineManagementSystemVer2.Models;

//namespace MachineManagementSystemVer2.輩分
//{
//    public class CustomersController : Controller
//    {
//        private readonly AppDbContext _context;

//        public CustomersController(AppDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Customers
//        // 已加入搜尋功能
//        public async Task<IActionResult> Index(string searchName, string searchTaxId)
//        {
//            // 使用 IQueryable<T> 來延遲查詢，直到最後才從資料庫取出資料
//            var customersQuery = _context.Customers.AsQueryable();

//            if (!string.IsNullOrEmpty(searchName))
//            {
//                customersQuery = customersQuery.Where(c => c.CustomerName.Contains(searchName));
//            }

//            if (!string.IsNullOrEmpty(searchTaxId))
//            {
//                customersQuery = customersQuery.Where(c => c.CustomerTaxId == searchTaxId);
//            }

//            // 將 ViewBag 的值傳給 View，以便在搜尋框中保留使用者輸入的關鍵字
//            ViewBag.CurrentFilterName = searchName;
//            ViewBag.CurrentFilterTaxId = searchTaxId;

//            return View(await customersQuery.ToListAsync());
//        }

//        // GET: Customers/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var customer = await _context.Customers
//                .Include(c => c.Plants) // 載入關聯的廠區資料
//                .FirstOrDefaultAsync(m => m.CustomerId == id);

//            if (customer == null)
//            {
//                return NotFound();
//            }

//            return View(customer);
//        }

//        // GET: Customers/Create
//        public IActionResult Create()
//        {
//            // 在 Create 頁面，客戶還沒有廠區，所以不需要載入廠區資料
//            return View();
//        }


//        // POST: Customers/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create
//            ([Bind("CustomerId,CustomerName,CustomerTaxId,CustomerAddress,CustomerPhone")] Customer customer)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(customer);
//                await _context.SaveChangesAsync();
//                // 建立客戶後，導向到該客戶的編輯頁面，讓使用者可以接著新增廠區
//                return RedirectToAction("Edit", new { id = customer.CustomerId });
//            }
//            return View(customer);
//        }// GET: Customers/Edit/5
//        // 顯示編輯客戶的表單，並載入其關聯的廠區資料
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            // 使用 Include 一次性載入客戶及其所有廠區
//            var customer = await _context.Customers.Include(c => c.Plants)
//                                             .FirstOrDefaultAsync(c => c.CustomerId == id);

//            if (customer == null)
//            {
//                return NotFound();
//            }
//            return View(customer);
//        }

//        // POST: Customers/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerName,CustomerTaxId,CustomerAddress,CustomerPhone")] Customer customer)
//        {
//            if (id != customer.CustomerId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(customer);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!CustomerExists(customer.CustomerId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(customer);
//        }


//        // --- AJAX 相關 Action ---

//        // POST: Customers/AddPlant
//        // 透過 AJAX 新增廠區資料
//        [HttpPost]
//        [ValidateAntiForgeryToken] // 建議 AJAX POST 也加上 Token 驗證
//        public async Task<IActionResult> AddPlant(Plant plant)
//        {
//            // 伺服器端再次驗證傳入的資料
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    // 將廠區加入資料庫並儲存
//                    _context.Plants.Add(plant);
//                    await _context.SaveChangesAsync();

//                    // 回傳成功的 JSON 結果，並附上剛才新增的廠區資料
//                    // 這將讓前端的 JavaScript 可以用來動態產生新的表格列
//                    return Json(new
//                    {
//                        success = true,
//                        plant = new
//                        {
//                            plantId = plant.PlantId,
//                            plantName = plant.PlantName,
//                            plantCode = plant.PlantCode, // 請確認您的 Plant Model 有此屬性
//                            plantAddress = plant.PlantAddress, // 請確認您的 Plant Model 有此屬性
//                            plantPhone = plant.PlantPhone // 請確認您的 Plant Model 有此屬性
//                        }
//                    });
//                }
//                catch (Exception ex)
//                {
//                    // 如果發生錯誤，回傳包含錯誤訊息的 JSON
//                    return Json(new { success = false, message = ex.Message });
//                }
//            }

//            // 如果模型驗證失敗，回傳失敗訊息
//            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
//            return Json(new { success = false, message = "輸入的資料格式不正確。", errors });
//        }

//        private bool CustomerExists(int id)
//        {
//            return _context.Customers.Any(e => e.CustomerId == id);
//        }
//    }
//}
