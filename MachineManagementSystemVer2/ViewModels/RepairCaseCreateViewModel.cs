using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MachineManagementSystemVer2.ViewModels
{
    public class RepairCaseCreateViewModel
    {
        [Required(ErrorMessage = "請選擇廠區")]
        [Display(Name = "廠區")]
        public int PlantId { get; set; }

        [Required(ErrorMessage = "請選擇設備")]
        [Display(Name = "設備")]
        public int DeviceId { get; set; }

        [Required]
        [Display(Name = "發生時間")]
        public DateTime OccurredAt { get; set; } = DateTime.Now;

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

        // --- 用於顯示下拉選單的資料來源 ---
        public SelectList? PlantList { get; set; }
        public SelectList? DeviceList { get; set; }
    }
}

