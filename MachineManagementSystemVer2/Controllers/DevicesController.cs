using Microsoft.AspNetCore.Mvc;
using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using MachineManagementSystemVer2.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MachineManagementSystemVer2.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly AppDbContext _context;

        public DevicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Devices
        public async Task<IActionResult> Index(
            string sortOrder,
            string searchCustomer,
            string searchPlant,
            string searchModel)
        {
            // --- 排序參數設定 ---
            ViewData["CustomerSortParm"] = string.IsNullOrEmpty(sortOrder) ? "customer_desc" : "";
            ViewData["PlantSortParm"] = sortOrder == "plant_asc" ? "plant_desc" : "plant_asc";
            ViewData["ModelSortParm"] = sortOrder == "model_asc" ? "model_desc" : "model_asc";

            // --- 篩選參數設定 (用於在搜尋後保留輸入值) ---
            ViewData["CurrentFilterCustomer"] = searchCustomer;
            ViewData["CurrentFilterPlant"] = searchPlant;
            ViewData["CurrentFilterModel"] = searchModel;

            // --- 查詢邏輯 ---
            var devices = from d in _context.Devices
                                .Include(d => d.Plant)
                                .ThenInclude(p => p.Customer)
                          select d;

            // 1. 篩選 (Filtering)
            if (!string.IsNullOrEmpty(searchCustomer))
            {
                devices = devices.Where(d => d.Plant.Customer.CustomerName.Contains(searchCustomer));
            }
            if (!string.IsNullOrEmpty(searchPlant))
            {
                devices = devices.Where(d => d.Plant.PlantName.Contains(searchPlant));
            }
            if (!string.IsNullOrEmpty(searchModel))
            {
                devices = devices.Where(d => d.DeviceModel.Contains(searchModel));
            }

            // 2. 排序 (Sorting)
            switch (sortOrder)
            {
                case "customer_desc":
                    devices = devices.OrderByDescending(d => d.Plant.Customer.CustomerName);
                    break;
                case "plant_asc":
                    devices = devices.OrderBy(d => d.Plant.PlantName);
                    break;
                case "plant_desc":
                    devices = devices.OrderByDescending(d => d.Plant.PlantName);
                    break;
                case "model_asc":
                    devices = devices.OrderBy(d => d.DeviceModel);
                    break;
                case "model_desc":
                    devices = devices.OrderByDescending(d => d.DeviceModel);
                    break;
                default: // 預設排序
                    devices = devices.OrderBy(d => d.Plant.Customer.CustomerName);
                    break;
            }

            return View(await devices.ToListAsync());
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var device = await _context.Devices
                .Include(d => d.Plant)
                     .ThenInclude(p => p.Customer)
                .Include(d => d.RepairCases)
                     .ThenInclude(rc => rc.Employee)
                .FirstOrDefaultAsync(m => m.DeviceId == id);

            if (device == null) return NotFound();

            return View(device);
        }
        // GET: Devices/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new DeviceCreateViewModel
            {
                CustomerList = new SelectList(await _context.Customers.ToListAsync(), "CustomerId", "CustomerName"),
                PlantList = new SelectList(new List<Plant>(), "PlantId", "PlantName") // 初始為空
            };
            return View(viewModel);
        }

        // POST: Devices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeviceCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var device = new Device
                {
                    SerialNumber = viewModel.SerialNumber,
                    PlantId = viewModel.PlantId,
                    ProductionLine = viewModel.ProductionLine,
                    DeviceModel = viewModel.DeviceModel,
                    Remark = viewModel.Remark
                };
                _context.Add(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // 如果驗證失敗，重新填充下拉選單
            viewModel.CustomerList = new SelectList(await _context.Customers.ToListAsync(), "CustomerId", "CustomerName", viewModel.CustomerId);
            viewModel.PlantList = new SelectList(await _context.Plants.Where(p => p.CustomerId == viewModel.CustomerId).ToListAsync(), "PlantId", "PlantName", viewModel.PlantId);
            return View(viewModel);
        }
        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var device = await _context.Devices.FindAsync(id);
            if (device == null) return NotFound();

            var viewModel = new DeviceEditViewModel
            {
                DeviceId = device.DeviceId,
                SerialNumber = device.SerialNumber,
                PlantId = device.PlantId,
                ProductionLine = device.ProductionLine,
                DeviceModel = device.DeviceModel,
                Remark = device.Remark,
                PlantList = new SelectList(await _context.Plants.ToListAsync(), "PlantId", "PlantName", device.PlantId)
            };

            return View(viewModel);
        }

        // POST: Devices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DeviceEditViewModel viewModel)
        {
            if (id != viewModel.DeviceId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var device = await _context.Devices.FindAsync(id);
                    if (device == null) return NotFound();

                    device.SerialNumber = viewModel.SerialNumber;
                    device.PlantId = viewModel.PlantId;
                    device.ProductionLine = viewModel.ProductionLine;
                    device.DeviceModel = viewModel.DeviceModel;
                    device.Remark = viewModel.Remark;

                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(viewModel.DeviceId))
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
            viewModel.PlantList = new SelectList(await _context.Plants.ToListAsync(), "PlantId", "PlantName", viewModel.PlantId);
            return View(viewModel);
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var device = await _context.Devices
                .Include(d => d.Plant)
                .FirstOrDefaultAsync(m => m.DeviceId == id);

            if (device == null) return NotFound();

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                _context.Devices.Remove(device);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.DeviceId == id);
        }

        // --- 【新增】AJAX Action ---
        // GET: /Devices/GetPlantsByCustomer?customerId=1
        [HttpGet]
        public async Task<JsonResult> GetPlantsByCustomer(int customerId)
        {
            var plants = await _context.Plants
                                       .Where(p => p.CustomerId == customerId)
                                       .Select(p => new { p.PlantId, p.PlantName })
                                       .ToListAsync();
            return Json(plants);
        }
    }
}