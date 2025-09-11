using Microsoft.AspNetCore.Mvc;
using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using MachineManagementSystemVer2.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MachineManagementSystemVer2.Controllers
{
    public class DevicesController : Controller
    {
        private readonly AppDbContext _context;

        public DevicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Devices
        public async Task<IActionResult> Index()
        {
            var devices = await _context.Devices.Include(d => d.Plant).ToListAsync();
            return View(devices);
        }

        // GET: Devices/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new DeviceCreateViewModel
            {
                PlantList = new SelectList(await _context.Plants.ToListAsync(), "PlantId", "PlantName")
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
            viewModel.PlantList = new SelectList(await _context.Plants.ToListAsync(), "PlantId", "PlantName", viewModel.PlantId);
            return View(viewModel);
        }
    }
}