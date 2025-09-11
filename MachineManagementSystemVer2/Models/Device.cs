using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MachineManagementSystemVer2.Models
{
    public class Device
    {
        [Key]
        public int DeviceId { get; set; } // 設備序號

        [Required(ErrorMessage = "出廠序號為必填")]
        [StringLength(50)]
        [Display(Name = "出廠序號")]
        public string SerialNumber { get; set; } // 出廠序號

        [Required(ErrorMessage = "請選擇所屬廠區")]
        [Display(Name = "所屬廠區")]
        public int PlantId { get; set; }

        // 【修正 1】將 ForeignKey 指向正確的屬性 "PlantId"
        [ForeignKey("PlantId")]
        // 【修正 2】將屬性名稱從 "PlantName" 改為 "Plant"，以符合慣例和 Controller 的使用
        public Plant Plant { get; set; }

        [StringLength(15)]
        [Display(Name = "客戶產線代稱")]
        public string? ProductionLine { get; set; } // 客戶自訂產線名

        [Required(ErrorMessage = "設備機型為必填")]
        [StringLength(50)]
        [Display(Name = "設備機型")]
        public string DeviceModel { get; set; }

        [StringLength(150)]
        [Display(Name = "備註")]
        public string? Remark { get; set; }

        public ICollection<RepairCase> RepairCases { get; set; } = new List<RepairCase>();
    }
}
