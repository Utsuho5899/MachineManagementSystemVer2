using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MachineManagementSystemVer2.ViewModels
{
    public class DeviceEditViewModel
    {
        public int DeviceId { get; set; }

        [Required(ErrorMessage = "出廠序號為必填")]
        [StringLength(50)]
        [Display(Name = "出廠序號")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "請選擇所屬廠區")]
        [Display(Name = "所屬廠區")]
        public int PlantId { get; set; }

        [StringLength(15)]
        [Display(Name = "客戶產線代稱 (選填)")]
        public string? ProductionLine { get; set; }

        [Required(ErrorMessage = "設備機型為必填")]
        [StringLength(50)]
        [Display(Name = "設備機型")]
        public string DeviceModel { get; set; }

        [StringLength(150)]
        [Display(Name = "備註 (選填)")]
        public string? Remark { get; set; }

        public SelectList? PlantList { get; set; }
    }
}

