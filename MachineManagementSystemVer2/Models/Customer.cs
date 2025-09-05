using System.ComponentModel.DataAnnotations;

namespace MachineManagementSystemVer2.Models
{
    public class Customer
    {
        [Key]
        [Display(Name = "客戶No.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "客戶公司名稱必填"), StringLength(50)]
        [Display(Name = "公司名稱")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "統編必填"), StringLength(8)]
        [Display(Name = "統一編號")]
        public string TaxId { get; set; } // 統一編號

        [Required(ErrorMessage = "公司地址必填"), StringLength(100)]
        [Display(Name = "公司地址")]
        public string Address { get; set; }

        [Required(ErrorMessage = "公司電話必填"), StringLength(15)]
        [Display(Name = "公司電話")]
        public string Phone { get; set; }

        [Display(Name = "廠區")]
        public ICollection<Plant> Plants { get; set; }

        public ICollection<RepairCase> RepairCases { get; set; } = new List<RepairCase>();
    }



}