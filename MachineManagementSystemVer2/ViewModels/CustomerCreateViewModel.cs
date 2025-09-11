using System.ComponentModel.DataAnnotations;

namespace MachineManagementSystemVer2.ViewModels
{
    // 這是一個專門為 "新增客戶" 頁面設計的 ViewModel
    // 它只包含表單需要提交的欄位，不多也不少，可以避免模型繫結的混亂
    public class CustomerCreateViewModel
    {
        [Required(ErrorMessage = "客戶公司名稱必填")]
        [StringLength(50)]
        [Display(Name = "公司名稱")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "統編必填")]
        [StringLength(8)]
        [Display(Name = "統一編號")]
        public string CustomerTaxId { get; set; }

        [Required(ErrorMessage = "公司地址必填")]
        [StringLength(100)]
        [Display(Name = "公司地址")]
        public string CustomerAddress { get; set; }

        [Required(ErrorMessage = "公司電話必填")]
        [StringLength(15)]
        [Display(Name = "公司電話")]
        public string CustomerPhone { get; set; }
    }
}
