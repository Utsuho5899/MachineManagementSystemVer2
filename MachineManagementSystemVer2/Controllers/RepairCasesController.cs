using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using MachineManagementSystemVer2.ViewModels;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace MachineManagementSystemVer2.Controllers
{
    public class RepairCasesController : Controller
    {
        private readonly AppDbContext _context;

        public RepairCasesController(AppDbContext context)
        {
            _context = context;
        }
        // 【新增】補上 Index 方法
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

        private int _GetLoggedInEmployeeId()
        {
            // 假設返回 ID 為 1 的員工
            return 1;
        }

        // GET: RepairCases/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new RepairCaseCreateViewModel
            {
                PlantList = new SelectList(await _context.Plants.ToListAsync(), "PlantId", "PlantName"),
                // 初始時設備列表為空，等待使用者選擇廠區
                DeviceList = new SelectList(new List<Device>(), "DeviceId", "DeviceModel")
            };
            return View(viewModel);
        }

        // POST: RepairCases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairCaseCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newCase = new RepairCase
                {
                    CaseStatus = "OPEN",
                    OccurredAt = viewModel.OccurredAt,
                    PlantId = viewModel.PlantId,
                    DeviceId = viewModel.DeviceId,
                    EmployeeId = _GetLoggedInEmployeeId(),
                    CustomerContact = viewModel.CustomerContact,
                    Description = viewModel.Description,
                    CaseRemark = viewModel.CaseRemark
                };

                _context.Add(newCase);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = newCase.RepairCaseId });
            }

            viewModel.PlantList = new SelectList(await _context.Plants.ToListAsync(), "PlantId", "PlantName", viewModel.PlantId);
            viewModel.DeviceList = new SelectList(await _context.Devices.Where(d => d.PlantId == viewModel.PlantId).ToListAsync(), "DeviceId", "DeviceModel", viewModel.DeviceId);
            return View(viewModel);
        }

        // --- AJAX Action ---
        // GET: /RepairCases/GetDevicesByPlant?plantId=5
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
                .Include(rc => rc.Device).ThenInclude(d => d.Plant)
                .Include(rc => rc.CaseComments).ThenInclude(cc => cc.Employee)
                .Include(rc => rc.CasePhotos)
                .FirstOrDefaultAsync(rc => rc.RepairCaseId == id);

            if (repairCase == null)
            {
                return NotFound();
            }

            var viewModel = new RepairCaseDetailViewModel
            {
                RepairCaseId = repairCase.RepairCaseId,
                CaseStatus = repairCase.CaseStatus,
                OccurredAt = repairCase.OccurredAt,
                CustomerContact = repairCase.CustomerContact,
                Description = repairCase.Description,
                CaseRemark = repairCase.CaseRemark,
                PlantName = repairCase.Device.Plant.PlantName,
                DeviceName = repairCase.Device.DeviceModel,
                EmployeeName = repairCase.Employee.EmployeeName,
                Comments = repairCase.CaseComments.OrderByDescending(c => c.CreatedAt).ToList(),
                Photos = repairCase.CasePhotos.OrderByDescending(p => p.UploadedAt).ToList()
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
                // 【同步】更新屬性名稱以匹配最新的 Model
                CaseComments = newCommentContent,
                CreatedAt = DateTime.Now,
                CaseId = caseId,
                EmployeeId = _GetLoggedInEmployeeId()
            };

            _context.CaseComments.Add(newComment);
            await _context.SaveChangesAsync();

            var employee = await _context.Employees.FindAsync(newComment.EmployeeId);

            return Json(new
            {
                success = true,
                // 【同步】更新回傳給前端的 JSON 屬性名稱
                caseComments = newComment.CaseComments,
                createdAt = newComment.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                employeeName = employee?.EmployeeName ?? "未知人員"
            });
        }

        // POST: RepairCases/AddPhoto (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoto(int caseId, IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                return Json(new { success = false, message = "請選擇要上傳的檔案。" });
            }

            byte[] photoData;
            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                photoData = memoryStream.ToArray();
            }

            var newCasePhoto = new CasePhoto
            {
                CaseId = caseId,
                FileName = photo.FileName,
                PhotoData = photoData,
                UploadedAt = DateTime.Now
            };

            _context.CasePhotos.Add(newCasePhoto);
            await _context.SaveChangesAsync();

            var photoBase64 = Convert.ToBase64String(newCasePhoto.PhotoData);

            return Json(new
            {
                success = true,
                photoId = newCasePhoto.PhotoId,
                fileName = newCasePhoto.FileName,
                photoSrc = $"data:{photo.ContentType};base64,{photoBase64}",
                uploadedAt = newCasePhoto.UploadedAt.ToString("yyyy-MM-dd HH:mm")
            });
        }
    }
}

