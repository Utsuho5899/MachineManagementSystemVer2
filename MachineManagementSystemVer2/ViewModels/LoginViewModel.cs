using System.ComponentModel.DataAnnotations;

namespace MachineManagementSystemVer2.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "請輸入系統帳號")]
        [Display(Name = "系統帳號")]
        public string Account { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }
    }
}
