using MachineManagementSystemVer2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MachineManagementSystemVer2.ViewModels
{
    // 【修改】這個輔助類別，讓它可以攜帶多張照片
    public class TimelineEntry
    {
        // 建立一個小類別來儲存單張照片的資訊
        public class PhotoInfo
        {
            public byte[] PhotoData { get; set; }
            public string FileName { get; set; }
        }

        public enum EntryType { Initial, Comment, StatusChange }

        // --- 共同屬性 ---
        public EntryType Type { get; set; }
        public DateTime Timestamp { get; set; }
        public string Author { get; set; }

        // --- 留言或描述的內容 ---
        public string? Content { get; set; }

        // --- 狀態變更專用屬性 ---
        public string? OldStatus { get; set; }
        public string? NewStatus { get; set; }

        // --- 照片專用屬性 (從單張改為列表) ---
        public List<PhotoInfo> Photos { get; set; } = new List<PhotoInfo>();
    }

    public class RepairCaseDetailViewModel
    {
        // 案件基本資訊
        public int RepairCaseId { get; set; }
        public string Title { get; set; }
        public string CaseStatus { get; set; }
        public DateTime OccurredAt { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? CustomerContact { get; set; }
        //public string Description { get; set; }
        public string? CaseRemark { get; set; }
        public string CustomerName { get; set; } // 新增客戶名稱
        public string PlantName { get; set; }
        public string DeviceName { get; set; }
        public string EmployeeName { get; set; }


        // 【新增】最新的案件歷史更新資訊
        public string? LatestUpdateBy { get; set; }
        public DateTime? LatestUpdateTime { get; set; }

        // 【整合】將案件描述與留言整合為一個時間軸
        public List<TimelineEntry> CaseTimeline { get; set; } = new List<TimelineEntry>();

        // 關聯的照片與狀態變更歷史
        //public List<CaseHistory> Histories { get; set; } = new List<CaseHistory>();
        //public List<CaseComment> Comments { get; set; } = new List<CaseComment>();
        public List<CasePhoto> Photos { get; set; } = new List<CasePhoto>();

        // --- 用於「後續狀態更新」的表單模型 ---
        [Display(Name = "更新案件狀態")]
        public string NewStatus { get; set; }

        [StringLength(1000)]
        [Display(Name = "後續狀態更新")]
        public string? NewCommentContent { get; set; }

        [Display(Name = "上傳新照片")]
        public IFormFile? NewPhoto { get; set; }

        public SelectList? StatusList { get; set; }
    }
}