using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace MachineManagementSystemVer2.Controllers
{
    //[Authorize]
    public class RepairCasesController : Controller
    {
        private readonly AppDbContext _context;

        public RepairCasesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: RepairCases
        public async Task<IActionResult> Index()
        {
            var repairCases = _context.RepairCases
                .Include(r => r.Device)
                .Include(r => r.Person);
            return View(await repairCases.ToListAsync());
        }

        // GET: RepairCases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairCase = await _context.RepairCases
                .Include(r => r.Device)
                .Include(r => r.Person)
                .FirstOrDefaultAsync(m => m.RepairCaseId == id);
            if (repairCase == null)
            {
                return NotFound();
            }

            return View(repairCase);
        }

        // GET: RepairCases/Create
        //[Authorize(Roles = "Admin,Engineer")]
        // Create GET
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CompanyName");
            return View();
        }

        // Ajax: 取得客戶對應的廠區
        public JsonResult GetPlants(int customerId)
        {
            var plants = _context.Plants
                .Where(p => p.CustomerId == customerId)
                .Select(p => new { p.PlantId, p.PlantName })
                .ToList();

            return Json(plants);
        }

        // Ajax: 取得廠區對應的設備
        public JsonResult GetDevices(int plantId)
        {
            var devices = _context.Devices
                .Where(d => d.PlantId == plantId)
                .Select(d => new { d.DeviceId, d.SerialNumber })
                .ToList();

            return Json(devices);
        }

        // Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RepairCaseId,DeviceId,Title,Description,Status,OccurredAt,CreatedById")] RepairCase repairCase)
        {
            if (ModelState.IsValid)
            {
                repairCase.Status = "OPEN";
                repairCase.OccurredAt = DateTime.Now;

                _context.Add(repairCase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // 如果失敗，重新載入下拉
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CompanyName");
            return View(repairCase);
        }

        // GET: RepairCases/Edit/5
        //[Authorize(Roles = "Admin,Engineer,Supervisor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairCase = await _context.RepairCases.FindAsync(id);
            if (repairCase == null)
            {
                return NotFound();
            }
            ViewData["DeviceId"] = new SelectList(_context.Devices, "DeviceId", "Model", repairCase.DeviceId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "PersonId", "Name", repairCase.PersonId);
            return View(repairCase);
        }

        // POST: RepairCases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin,Engineer,Supervisor")]
        public async Task<IActionResult> Edit(int id, [Bind("RepairCaseId,Status,OccurredAt,DeviceId,PersonId,CustomerContact,Description,Notes")] RepairCase repairCase)
        {
            if (id != repairCase.RepairCaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (repairCase.Status == "CLOSE" && !User.IsInRole("Admin") && !User.IsInRole("Supervisor"))
                    {
                        ModelState.AddModelError("", "只有主管或系統管理者可以結案。");
                        return View(repairCase);
                    }
                    _context.Update(repairCase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairCaseExists(repairCase.RepairCaseId))
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
            ViewData["DeviceId"] = new SelectList(_context.Devices, "DeviceId", "Model", repairCase.DeviceId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "PersonId", "Name", repairCase.PersonId);
            return View(repairCase);
        }

        // GET: RepairCases/Delete/5
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairCase = await _context.RepairCases
                .Include(r => r.Device)
                .Include(r => r.Person)
                .FirstOrDefaultAsync(m => m.RepairCaseId == id);
            if (repairCase == null)
            {
                return NotFound();
            }

            return View(repairCase);
        }

        // POST: RepairCases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repairCase = await _context.RepairCases.FindAsync(id);
            if (repairCase != null)
            {
                _context.RepairCases.Remove(repairCase);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepairCaseExists(int id)
        {
            return _context.RepairCases.Any(e => e.RepairCaseId == id);
        }
    }
}
