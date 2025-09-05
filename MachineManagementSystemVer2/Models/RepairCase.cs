using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace MachineManagementSystemVer2.Models
{
    public class RepairCase
    {
        [Key]
        [Display(Name = "案件No.")]
        public int RepairCaseId { get; set; }

        [Required]
        [Display(Name = "案件狀態")]
        public string Status { get; set; } = "OPEN"; // 預設 OPEN

        [Required]
        [Display(Name = "發生時間")]
        public DateTime OccurredAt { get; set; }

        [Required]
        [Display(Name = "設備")]
        public int DeviceId { get; set; }
        public Device Device { get; set; }

        [Required]
        [Display(Name = "建單人員")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [Display(Name = "客戶人員")]
        public string? CustomerContact { get; set; }

        [Required]
        [Display(Name = "故障內容描述")]
        public string Description { get; set; }

        [Display(Name = "備註")]
        public string? Notes { get; set; }

        public ICollection<CaseComment> Comments { get; set; } = new List<CaseComment>();
    }
}
