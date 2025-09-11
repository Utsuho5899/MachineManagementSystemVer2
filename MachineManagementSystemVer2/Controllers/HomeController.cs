using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MachineManagementSystemVer2.Models;
using Microsoft.AspNetCore.Authorization; // 引用授權
using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MachineManagementSystemVer2.Controllers
{
    [Authorize] // 【安全設定】確保只有登入的使用者才能存取 HomeController 的所有頁面
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                OpenCasesCount = await _context.RepairCases.CountAsync(rc => rc.CaseStatus == "OPEN" || rc.CaseStatus == "暫置"),
                ClosedCasesCount = await _context.RepairCases.CountAsync(rc => rc.CaseStatus == "CLOSE"),
                TotalDevicesCount = await _context.Devices.CountAsync(),
                TotalCustomersCount = await _context.Customers.CountAsync(),
                RecentCases = await _context.RepairCases
                                        .Include(rc => rc.Device)
                                        .Include(rc => rc.Plant)
                                        .OrderByDescending(rc => rc.OccurredAt)
                                        .Take(5) // 只取最近的5筆
                                        .ToListAsync()
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
