using Microsoft.AspNetCore.Mvc;
using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;

namespace MachineManagementSystemVer2.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class KpiController : Controller
    {
        private readonly AppDbContext _context;

        public KpiController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchStatus, string? searchEmployeeId, DateTime? searchStartDate, DateTime? searchEndDate)
        {
            var model = new KpiViewModel
            {
                SearchStatus = searchStatus,
                SearchEmployeeId = searchEmployeeId,
                SearchStartDate = searchStartDate,
                SearchEndDate = searchEndDate
            };

            model.EmployeeList = new SelectList(await _context.Users.OrderBy(e => e.EmployeeName).ToListAsync(), "Id", "EmployeeName", model.SearchEmployeeId);
            model.StatusList = new SelectList(new List<string> { "OPEN", "暫置", "CLOSE" }, model.SearchStatus);

            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParm"] = sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewData["StatusSortParm"] = sortOrder == "status_asc" ? "status_desc" : "status_asc";
            ViewData["CustomerSortParm"] = sortOrder == "customer_asc" ? "customer_desc" : "customer_asc";
            ViewData["PlantSortParm"] = sortOrder == "plant_asc" ? "plant_desc" : "plant_asc";
            ViewData["DeviceSortParm"] = sortOrder == "device_asc" ? "device_desc" : "device_asc";
            ViewData["EmployeeSortParm"] = sortOrder == "employee_asc" ? "employee_desc" : "employee_asc";
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_asc" : "";

            var query = _context.RepairCases.AsQueryable();

            if (!string.IsNullOrEmpty(model.SearchEmployeeId))
            {
                var empId = model.SearchEmployeeId;
                query = query.Where(rc => rc.EmployeeId == empId || rc.CaseComments.Any(cc => cc.EmployeeId == empId));
            }
            if (model.SearchStartDate.HasValue)
            {
                var startDate = model.SearchStartDate.Value.Date;
                query = query.Where(rc => rc.OccurredAt >= startDate || rc.CaseComments.Any(cc => cc.CreatedAt >= startDate));
            }
            if (model.SearchEndDate.HasValue)
            {
                var endDate = model.SearchEndDate.Value.Date.AddDays(1);
                query = query.Where(rc => rc.OccurredAt < endDate || rc.CaseComments.Any(cc => cc.CreatedAt < endDate));
            }
            if (!string.IsNullOrEmpty(model.SearchStatus))
            {
                query = query.Where(rc => rc.CaseStatus == model.SearchStatus);
            }

            switch (sortOrder)
            {
                case "id_asc": query = query.OrderBy(r => r.RepairCaseId); break;
                case "id_desc": query = query.OrderByDescending(r => r.RepairCaseId); break;
                case "status_asc":
                    query = query.OrderBy(r => r.CaseStatus == "OPEN" ? 1 : r.CaseStatus == "暫置" ? 2 : 3);
                    break;
                case "status_desc":
                    query = query.OrderByDescending(r => r.CaseStatus == "OPEN" ? 1 : r.CaseStatus == "暫置" ? 2 : 3);
                    break;
                case "customer_asc": query = query.OrderBy(r => r.Plant.Customer.CustomerName); break;
                case "customer_desc": query = query.OrderByDescending(r => r.Plant.Customer.CustomerName); break;
                case "plant_asc": query = query.OrderBy(r => r.Plant.PlantName); break;
                case "plant_desc": query = query.OrderByDescending(r => r.Plant.PlantName); break;
                case "device_asc": query = query.OrderBy(r => r.Device.DeviceModel); break;
                case "device_desc": query = query.OrderByDescending(r => r.Device.DeviceModel); break;
                case "employee_asc": query = query.OrderBy(r => r.Employee.EmployeeName); break;
                case "employee_desc": query = query.OrderByDescending(r => r.Employee.EmployeeName); break;
                case "date_asc": query = query.OrderBy(r => r.OccurredAt); break;
                default: query = query.OrderByDescending(r => r.OccurredAt); break;
            }

            var filteredResults = await query
                .Include(rc => rc.Plant).ThenInclude(p => p.Customer)
                .Include(rc => rc.Device)
                .Include(rc => rc.Employee)
                .Include(rc => rc.CaseComments).ThenInclude(cc => cc.Employee)
                .ToListAsync();

            model.TotalCount = filteredResults.Count;
            model.ClosedCount = filteredResults.Count(rc => rc.CaseStatus == "CLOSE");
            model.OpenCount = model.TotalCount - model.ClosedCount;
            model.FilteredCases = filteredResults;

            // --- 【修改】判斷是否為 AJAX 請求 ---
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // 如果是，只回傳表格部分的局部檢視
                return PartialView("_KpiTablePartial", model);
            }

            // 如果是正常的頁面載入，回傳完整頁面
            return View(model);
        }
    }
}

