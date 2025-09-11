using System.ComponentModel.DataAnnotations;


namespace MachineManagementSystemVer2.ViewModels
{
    public class CustomerEditViewModel
    {
        public int CustomerId { get; set; }

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
