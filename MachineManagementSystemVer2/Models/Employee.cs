using System.ComponentModel.DataAnnotations;

namespace MachineManagementSystemVer2.Models
{

    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "姓名為必填")]
        [StringLength(50)]
        [Display(Name = "姓名")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "入職日為必填")]
        [DataType(DataType.Date)]
        [Display(Name = "入職日")]
        public DateTime HireDate { get; set; }

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

        // 實際應用中，密碼應進行雜湊(Hashing)處理後再存入資料庫
        [StringLength(200)]
        [Display(Name = "系統密碼")]
        public string? Password { get; set; }

        [StringLength(100)]
        [Display(Name = "備註")]
        public string? Remarks { get; set; }

        // --- 【解決方案】補上這個遺失的導覽屬性 ---
        // 這代表一個員工可以建立多個維修案件
        public ICollection<RepairCase> RepairCases { get; set; } = new List<RepairCase>();

    }

}
