using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace MachineManagementSystemVer2.ViewModels
{
    public class RepairCaseCreateViewModel
    {
        // --- 【新增】三層連動選單 ---
        [Required(ErrorMessage = "請選擇客戶")]
        [Display(Name = "客戶")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "請選擇廠區")]
        [Display(Name = "廠區")]
        public int PlantId { get; set; }

        [Required(ErrorMessage = "請選擇設備")]
        [Display(Name = "設備")]
        public int DeviceId { get; set; }

        [Required(ErrorMessage = "請輸入案件標題")]
        [StringLength(100)]
        [Display(Name = "案件標題")]
        public string Title { get; set; }

        // 詞彙更新
        [Required(ErrorMessage = "請選擇發生日期")]
        [Display(Name = "首發時間")]
        public DateTime OccurredDate { get; set; } = DateTime.Today;

        //[Range(9, 23, ErrorMessage = "請選擇小時")]
        [Display(Name = "時")]
        public int OccurredHour { get; set; } = DateTime.Now.Hour;

        [Required(ErrorMessage = "請選擇分鐘")]
        [Display(Name = "分")]
        public int OccurredMinute { get; set; } = 0;

        [Display(Name = "入廠時間 (如有進廠)")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "時")]
        public int? StartHour { get; set; }

        [Display(Name = "分")]
        public int? StartMinute { get; set; }

        [Display(Name = "出廠時間")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "時")]
        public int? EndHour { get; set; }

        [Display(Name = "分")]
        public int? EndMinute { get; set; }

        [StringLength(50)]
        [Display(Name = "客戶聯絡人 (選填)")]
        public string? CustomerContact { get; set; }

        [Required(ErrorMessage = "請描述故障內容")]
        [StringLength(1500)]
        [Display(Name = "故障內容描述")]
        public string Description { get; set; }

        [StringLength(100)]
        [Display(Name = "備註 (選填)")]
        public string? CaseRemark { get; set; }

        // --- 【新增】用於接收上傳的檔案 ---
        [Display(Name = "上傳照片 (可選多張)")]
        public List<IFormFile>? Photos { get; set; }

        public SelectList? PlantList { get; set; }
        public SelectList? CustomerList { get; set; } 
        public SelectList? DeviceList { get; set; }
    }
}

