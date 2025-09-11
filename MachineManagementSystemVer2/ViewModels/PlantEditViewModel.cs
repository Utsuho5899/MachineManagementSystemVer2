using System.ComponentModel.DataAnnotations;

namespace MachineManagementSystemVer2.ViewModels
{
    public class PlantEditViewModel
    {
        public int PlantId { get; set; }
        public int CustomerId { get; set; } // 用於返回按鈕

        [Required(ErrorMessage = "廠區名稱為必填")]
        [StringLength(50)]
        [Display(Name = "廠區名稱")]
        public string PlantName { get; set; }

        [Required(ErrorMessage = "廠區地址為必填")]
        [StringLength(100)]
        [Display(Name = "廠區地址")]
        public string PlantAddress { get; set; }

        [Required(ErrorMessage = "廠區電話為必填")]
        [StringLength(15)]
        [Display(Name = "廠區電話")]
        public string PlantPhone { get; set; }
    }
}