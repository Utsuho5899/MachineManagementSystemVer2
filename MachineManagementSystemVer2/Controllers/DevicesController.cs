using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;

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
            return View(await _context.Devices.Include(d => d.Plant).ToListAsync());
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            ViewData["PlantId"] = new SelectList(_context.Plants, "PlantId", "PlantName");
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SerialNumber,PlantId,CustomLineName,ProductionLine,Model,Remark")] Device device)
        {
            if (ModelState.IsValid)
            {
                _context.Add(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlantId"] = new SelectList(_context.Plants, "PlantId", "PlantName", device.PlantId);
            return View(device);
        }
    }
}
