using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineManagementSystemVer2.Models
{
    public class SystemLog
    {
        [Key]
        public int SystemLogId { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required, StringLength(100)]
        public string Action { get; set; }

        [Required, StringLength(50)]
        public string TableName { get; set; }

        public int? RecordId { get; set; }

        [Required]
        public DateTime ActionTime { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string IPAddress { get; set; }
    }

}
