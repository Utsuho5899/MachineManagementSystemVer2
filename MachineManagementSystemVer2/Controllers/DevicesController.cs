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
        public async Task<IActionResult> Index()
        {
            var devices = await _context.Devices.Include(d => d.Plant).ToListAsync();
            return View(devices);
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var device = await _context.Devices
                .Include(d => d.Plant)
                .FirstOrDefaultAsync(m => m.DeviceId == id);

            if (device == null) return NotFound();

            return View(device);
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
    }
}