using System.ComponentModel.DataAnnotations;

namespace MachineManagementSystemVer2.Models
{

    public class Person
    {
        [Key]
        [Display(Name = "員工編號")]
        public int PersonId { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "入職日")]
        public DateTime HireDate { get; set; }

        [Required, StringLength(30)]
        [Display(Name = "職稱")]
        public string Title { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "聯絡地址")]
        public string Address { get; set; }

        [Required, StringLength(15)]
        [Display(Name = "聯絡電話")]
        public string Phone { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "緊急聯絡人")]
        public string EmergencyContact { get; set; }

        [Required, StringLength(15)]
        [Display(Name = "緊急聯絡人電話")]
        public string EmergencyPhone { get; set; }

        [StringLength(50)]
        [Display(Name = "帳號")]
        public string? Account { get; set; }

        [StringLength(200)]
        [Display(Name = "密碼")]
        public string?  PasswordHash { get; set; }

        [StringLength(100)]
        [Display(Name = "備註")]
        public string? Remark { get; set; }

        // 權限角色
        //[Required]
        public string? Role { get; set; } // Admin / Manager / Engineer

        public ICollection<RepairCase> RepairCases { get; set; } = new List<RepairCase>();
    }

}
