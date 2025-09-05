using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineManagementSystemVer2.Models
{
    public class CaseHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [ForeignKey("RepairCase")]
        public int CaseId { get; set; }
        public RepairCase RepairCase { get; set; }

        [StringLength(20)]
        public string OldStatus { get; set; }

        [Required, StringLength(20)]
        public string NewStatus { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; } = DateTime.Now;

        [ForeignKey("Person")]
        public int ChangedBy { get; set; }
        public Person Person { get; set; }
    }
}
