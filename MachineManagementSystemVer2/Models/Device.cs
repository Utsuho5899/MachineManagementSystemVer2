using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineManagementSystemVer2.Models
{
    public class Device
    {
        [Key]
        public int DeviceId { get; set; } // 設備序號

        [Required, StringLength(50)]
        [Display(Name = "出廠序號")]
        public string SerialNumber { get; set; } // 出廠序號

        [ForeignKey("Plant")]
        public int PlantId { get; set; }
        public Plant Plant { get; set; }

        [StringLength(15)]
        [Display(Name = "客戶產線代稱")]
        public string ProductionLine { get; set; } // 客戶自訂產線名

        [Required, StringLength(50)]
        [Display(Name = "設備機型")]
        public string Model { get; set; }

        [StringLength(150)]
        [Display(Name = "備註")]
        public string? Remark { get; set; }

        public ICollection<RepairCase> RepairCases { get; set; } = new List<RepairCase>();
    }
}
