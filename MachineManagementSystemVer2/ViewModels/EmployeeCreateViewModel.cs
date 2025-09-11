using System;
using System.ComponentModel.DataAnnotations;

namespace MachineManagementSystemVer2.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required(ErrorMessage = "姓名為必填")]
        [StringLength(50)]
        [Display(Name = "姓名")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "入職日為必填")]
        [DataType(DataType.Date)]
        [Display(Name = "入職日")]
        public DateTime HireDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "職稱為必填")]
        [StringLength(30)]
        [Display(Name = "職稱")]
        public string EmployeeTitle { get; set; }

        [Required(ErrorMessage = "地址為必填")]
        [StringLength(100)]
        [Display(Name = "地址")]
        public string EmployeeAddress { get; set; }

        [Required(ErrorMessage = "聯絡電話為必填")]
        [StringLength(15)]
        [Display(Name = "電話")]
        public string EmployeePhone { get; set; }

        [Required(ErrorMessage = "緊急聯絡人必填")]
        [StringLength(50)]
        [Display(Name = "緊急聯絡人")]
        public string EmergencyContact { get; set; }

        [StringLength(15)]
        [Display(Name = "緊急聯絡人電話")]
        public string EmergencyPhone { get; set; }

        [StringLength(50)]
        [Display(Name = "系統帳號")]
        public string? Account { get; set; }

        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "密碼長度至少需6個字元")]
        [Display(Name = "系統密碼")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不相符。")]
        public string? ConfirmPassword { get; set; }

        [StringLength(100)]
        [Display(Name = "備註")]
        public string? Remarks { get; set; }
    }
}

