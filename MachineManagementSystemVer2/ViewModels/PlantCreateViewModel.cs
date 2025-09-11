using System.ComponentModel.DataAnnotations;

namespace MachineManagementSystemVer2.ViewModels
{
    public class PlantCreateViewModel
    {
        // 這個隱藏欄位至關重要，用於記錄此廠區屬於哪個客戶
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "廠區名稱必填")]
        [StringLength(50)]
        [Display(Name = "廠區名稱")]
        public string PlantName { get; set; }

        [StringLength(15)]
        [Display(Name = "廠區代稱")]
        public string? PlantCode { get; set; }

        [Required(ErrorMessage = "廠區地址必填")]
        [StringLength(100)]
        [Display(Name = "廠區地址")]
        public string PlantAddress { get; set; }

        [Required(ErrorMessage = "廠區電話必填")]
        [StringLength(15)]
        [Display(Name = "廠區電話")]
        public string PlantPhone { get; set; }

        // 這個欄位用於在頁面上顯示客戶名稱，提升使用者體驗
        public string? CustomerName { get; set; }
    }
}