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
    public class PlantsController : Controller
    {
        private readonly AppDbContext _context;

        public PlantsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Plants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Plants.Include(p => p.Customer).ToListAsync());
        }
        // GET: Plants/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName");
            return View();
        }

        // POST: Plants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlantId,PlantName,PlantCode,Address,Phone,CustomerId")] Plant plant)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(plant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CompanyName", plant.CustomerId);
            return View(plant);
        }
    }
}
