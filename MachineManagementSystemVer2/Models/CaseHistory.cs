using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineManagementSystemVer2.Models
{
    public class CaseHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int RepairCaseId { get; set; }
        [ForeignKey("RepairCaseId")]
        public RepairCase RepairCase { get; set; }

        [StringLength(20)]
        public string OldStatus { get; set; }

        [Required, StringLength(20)]
        public string NewStatus { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; } = DateTime.Now;

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
