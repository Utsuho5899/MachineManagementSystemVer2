using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineManagementSystemVer2.Models
{
    public class RepairCase
    {
        [Key]
        [Display(Name = "案件No.")]
        public int RepairCaseId { get; set; }

        // --- 【新增】案件標題 ---
        [Required(ErrorMessage = "請輸入案件標題")]
        [StringLength(100)]
        [Display(Name = "案件標題")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "案件狀態")]
        [StringLength(50)]
        public string CaseStatus { get; set; } = "OPEN";

        [Required]
        [Display(Name = "初發時間")] // 詞彙更新
        public DateTime OccurredAt { get; set; }

        [Display(Name = "入廠時間(如有進廠時填寫)")] // 詞彙更新
        public DateTime? StartTime { get; set; }

        [Display(Name = "出廠時間")] // 詞彙更新
        public DateTime? EndTime { get; set; }

        [Required]
        [Display(Name = "廠區")]
        public int PlantId { get; set; }
        [ForeignKey("PlantId")]
        public Plant Plant { get; set; }

        [Required]
        [Display(Name = "設備")]
        public int DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public Device Device { get; set; }

        [Required]
        [Display(Name = "處理人員")] // 詞彙更新
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [StringLength(50)]
        [Display(Name = "客戶人員")]
        public string? CustomerContact { get; set; }

        [Required]
        [StringLength(1500)]
        [Display(Name = "故障內容描述")]
        public string Description { get; set; }

        [StringLength(100)]
        [Display(Name = "備註")]
        public string? CaseRemark { get; set; }

        // 【移除】客戶簽名欄位已移除，因為僅需在紙本上呈現

        public ICollection<CaseComment> CaseComments { get; set; } = new List<CaseComment>();
        public ICollection<CaseHistory> CaseHistories { get; set; } = new List<CaseHistory>();
        public ICollection<CasePhoto> CasePhotos { get; set; } = new List<CasePhoto>();
    }
}