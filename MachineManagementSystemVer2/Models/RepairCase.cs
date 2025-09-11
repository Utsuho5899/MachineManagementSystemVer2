using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace MachineManagementSystemVer2.Models
{
    public class RepairCase
    {
        [Key]
        [Display(Name = "案件No.")]
        public int RepairCaseId { get; set; }

        [Required]
        [Display(Name = "案件狀態")]
        [StringLength(50)]
        public string CaseStatus { get; set; } = "OPEN";

        [Required]
        [Display(Name = "發生時間")]
        public DateTime OccurredAt { get; set; }

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
        [Display(Name = "建單人員")]
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

        public ICollection<CaseComment> CaseComments { get; set; } = new List<CaseComment>();
        public ICollection<CaseHistory> CaseHistories { get; set; } = new List<CaseHistory>();
        public ICollection<CasePhoto> CasePhotos { get; set; } = new List<CasePhoto>();
    }
}