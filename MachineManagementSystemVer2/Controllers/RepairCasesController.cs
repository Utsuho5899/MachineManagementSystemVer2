using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using MachineManagementSystemVer2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic; // For List
using System.Linq; // For OrderBy


namespace MachineManagementSystemVer2.Controllers
{
    [Authorize(Roles = "Engineer,Manager,Admin")]
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
            var repairCases = await _context.RepairCases
                .Include(r => r.Device)
                .Include(r => r.Plant)
                .Include(r => r.Employee)
                .OrderByDescending(r => r.OccurredAt)
                .ToListAsync();
            return View(repairCases);
        }

        private (int id, string name) _GetLoggedInEmployeeInfo()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name) ?? "未知人員";
            if (int.TryParse(userIdString, out int userId))
            {
                return (userId, userName);
            }
            return (1, "系統管理員(預設)"); // Fallback for testing
        }

        // GET: RepairCases/Create
        public async Task<IActionResult> Create()
        {
            var loggedInUser = _GetLoggedInEmployeeInfo();
            var viewModel = new RepairCaseCreateViewModel
            {
                CustomerList = new SelectList(await _context.Customers.ToListAsync(), "CustomerId", "CustomerName"),
                PlantList = new SelectList(new List<Plant>(), "PlantId", "PlantName"),
                DeviceList = new SelectList(new List<Device>(), "DeviceId", "DeviceModel")
            };
            // 【修正】使用 ViewBag 來傳遞僅供顯示的資訊
            ViewBag.LoggedInEmployeeName = loggedInUser.name;
            return View(viewModel);
        }

        // POST: RepairCases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairCaseCreateViewModel viewModel)
        {
            var loggedInUser = _GetLoggedInEmployeeInfo();

            DateTime occurredAt = viewModel.OccurredDate.AddHours(viewModel.OccurredHour).AddMinutes(viewModel.OccurredMinute);
            DateTime? startTime = viewModel.StartDate.HasValue && viewModel.StartHour.HasValue && viewModel.StartMinute.HasValue ? viewModel.StartDate.Value.AddHours(viewModel.StartHour.Value).AddMinutes(viewModel.StartMinute.Value) : null;
            DateTime? endTime = viewModel.EndDate.HasValue && viewModel.EndHour.HasValue && viewModel.EndMinute.HasValue ? viewModel.EndDate.Value.AddHours(viewModel.EndHour.Value).AddMinutes(viewModel.EndMinute.Value) : null;

           
            if (ModelState.IsValid)
            {
                var newCase = new RepairCase
                {
                    Title = viewModel.Title, // Add Title mapping
                    CaseStatus = "OPEN",
                    OccurredAt = occurredAt,
                    StartTime = startTime,
                    EndTime = endTime,
                    PlantId = viewModel.PlantId,
                    DeviceId = viewModel.DeviceId,
                    EmployeeId = loggedInUser.id,
                    CustomerContact = viewModel.CustomerContact,
                    Description = viewModel.Description,
                    CaseRemark = viewModel.CaseRemark
                };

                _context.Add(newCase);
                await _context.SaveChangesAsync();

                if (viewModel.Photos != null && viewModel.Photos.Count > 0)
                {
                    foreach (var photoFile in viewModel.Photos)
                    {
                        if (photoFile.Length > 0)
                        {
                            byte[] photoData;
                            using (var memoryStream = new MemoryStream())
                            {
                                await photoFile.CopyToAsync(memoryStream);
                                photoData = memoryStream.ToArray();
                            }
                            var newCasePhoto = new CasePhoto
                            {
                                CaseId = newCase.RepairCaseId,
                                FileName = Path.GetFileName(photoFile.FileName),
                                PhotoData = photoData
                            };
                            _context.CasePhotos.Add(newCasePhoto);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Details", new { id = newCase.RepairCaseId });
            }

            // 如果驗證失敗，重新填充下拉選單
            viewModel.CustomerList = new SelectList(await _context.Customers.ToListAsync(), "CustomerId", "CustomerName", viewModel.CustomerId);
            viewModel.PlantList = new SelectList(await _context.Plants.Where(p => p.CustomerId == viewModel.CustomerId).ToListAsync(), "PlantId", "PlantName", viewModel.PlantId);
            viewModel.DeviceList = new SelectList(await _context.Devices.Where(d => d.PlantId == viewModel.PlantId).ToListAsync(), "DeviceId", "DeviceModel", viewModel.DeviceId);
            ViewBag.LoggedInEmployeeName = loggedInUser.name;
            return View(viewModel);
        }

        // --- AJAX Actions for Cascading Dropdowns ---
        [HttpGet]
        public async Task<JsonResult> GetPlantsByCustomer(int customerId)
        {
            var plants = await _context.Plants
                                       .Where(p => p.CustomerId == customerId)
                                       .Select(p => new { p.PlantId, p.PlantName })
                                       .ToListAsync();
            return Json(plants);
        }

        [HttpGet]
        public async Task<JsonResult> GetDevicesByPlant(int plantId)
        {
            var devices = await _context.Devices
                                        .Where(d => d.PlantId == plantId)
                                        .Select(d => new { d.DeviceId, d.DeviceModel })
                                        .ToListAsync();
            return Json(devices);
        }

        // GET: RepairCases/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var repairCase = await _context.RepairCases
                .Include(rc => rc.Employee)
                .Include(rc => rc.Plant).ThenInclude(p => p.Customer) // 載入客戶
                .Include(rc => rc.Device)
                .Include(rc => rc.CaseComments).ThenInclude(cc => cc.Employee)
                .Include(rc => rc.CasePhotos)
                .Include(rc => rc.CaseHistories).ThenInclude(ch => ch.Employee) // 載入歷史紀錄
                .FirstOrDefaultAsync(rc => rc.RepairCaseId == id);

            if (repairCase == null) return NotFound();

            // --- 建立整合後的時間軸 (Timeline) ---
            var timeline = new List<TimelineEntry>();

            // 1. 加入初始的故障內容描述

            timeline.Add(new TimelineEntry
            {
                Type = TimelineEntry.EntryType.Initial,
                Timestamp = repairCase.OccurredAt,
                Content = repairCase.Description,
                Author = repairCase.Employee.EmployeeName
            });
            
            // 2. 加入所有的後續留言

            timeline.AddRange(repairCase.CaseComments.Select(c => new TimelineEntry
            {
                Type = TimelineEntry.EntryType.Comment,
                Timestamp = c.CreatedAt,
                Content = c.CaseComments,
                Author = c.Employee.EmployeeName
            }));
            // 3. 加入所有的狀態變更歷史
            timeline.AddRange(repairCase.CaseHistories.Select(h => new TimelineEntry
            {
                Type = TimelineEntry.EntryType.StatusChange,
                Timestamp = h.ChangedAt,
                Author = h.Employee.EmployeeName,
                OldStatus = h.OldStatus,
                NewStatus = h.NewStatus
            }));

            // b. 加入所有的後續留言
            var statusOptions = new List<string> { "OPEN", "暫置", "CLOSE" };
            var viewModel = new RepairCaseDetailViewModel
            {
                RepairCaseId = repairCase.RepairCaseId,
                Title = repairCase.Title,
                CaseStatus = repairCase.CaseStatus,
                OccurredAt = repairCase.OccurredAt,
                StartTime = repairCase.StartTime,
                EndTime = repairCase.EndTime,
                CustomerContact = repairCase.CustomerContact,
                CaseRemark = repairCase.CaseRemark,
                CustomerName = repairCase.Plant.Customer.CustomerName, // 取得客戶名稱
                PlantName = repairCase.Plant.PlantName,
                DeviceName = repairCase.Device.DeviceModel,
                CaseTimeline = timeline.OrderByDescending(t => t.Timestamp).ToList(), 
                Photos = repairCase.CasePhotos.OrderByDescending(p => p.UploadedAt).ToList(),
                StatusList = new SelectList(statusOptions, repairCase.CaseStatus),
                NewStatus = repairCase.CaseStatus
            };

            return View(viewModel);
        }

        // POST: RepairCases/AddComment (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int caseId, string newCommentContent)
        {
            if (string.IsNullOrWhiteSpace(newCommentContent))
            {
                return Json(new { success = false, message = "留言內容不可為空。" });
            }

            var newComment = new CaseComment
            {
                CaseComments = newCommentContent,
                CreatedAt = DateTime.Now,
                CaseId = caseId,
                EmployeeId = _GetLoggedInEmployeeInfo().id
            };

            _context.CaseComments.Add(newComment);
            await _context.SaveChangesAsync();

            var employee = await _context.Employees.FindAsync(newComment.EmployeeId);

            return Json(new
            {
                success = true,
                caseComments = newComment.CaseComments,
                createdAt = newComment.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                employeeName = employee?.EmployeeName ?? "未知人員"
            });
        }

        // --- 【新增】整合後的 AJAX 更新 Action ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCase(int caseId, string newStatus, string? newCommentContent, IFormFile? newPhoto)
        {
            var repairCase = await _context.RepairCases.FindAsync(caseId);
            if (repairCase == null)
            {
                return Json(new { success = false, message = "找不到指定的案件。" });
            }

            var loggedInUser = _GetLoggedInEmployeeInfo();
            var hasChanges = false;

            // 1. 處理狀態變更
            if (repairCase.CaseStatus != newStatus)
            {
                var history = new CaseHistory
                {
                    RepairCaseId = caseId,
                    OldStatus = repairCase.CaseStatus,
                    NewStatus = newStatus,
                    ChangedAt = DateTime.Now,
                    EmployeeId = loggedInUser.id
                };
                _context.CaseHistories.Add(history);
                repairCase.CaseStatus = newStatus;
                hasChanges = true;
            }

            // 2. 處理新增留言
            if (!string.IsNullOrWhiteSpace(newCommentContent))
            {
                var comment = new CaseComment
                {
                    CaseId = caseId,
                    CaseComments = newCommentContent,
                    EmployeeId = loggedInUser.id,
                    CreatedAt = DateTime.Now
                };
                _context.CaseComments.Add(comment);
                hasChanges = true;
            }

            // 3. 處理照片上傳
            if (newPhoto != null && newPhoto.Length > 0)
            {
                byte[] photoData;
                using (var ms = new MemoryStream())
                {
                    await newPhoto.CopyToAsync(ms);
                    photoData = ms.ToArray();
                }
                var photo = new CasePhoto
                {
                    CaseId = caseId,
                    FileName = newPhoto.FileName,
                    PhotoData = photoData,
                    UploadedAt = DateTime.Now
                };
                _context.CasePhotos.Add(photo);
                hasChanges = true;
            }

            if (hasChanges)
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "案件已更新。" });
            }

            return Json(new { success = false, message = "沒有任何變更。" });
        }
    }
}