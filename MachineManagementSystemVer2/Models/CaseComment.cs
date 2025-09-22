using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineManagementSystemVer2.Models
{
    public class CaseComment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public int CaseId { get; set; }
        [ForeignKey("CaseId")]
        public RepairCase RepairCase { get; set; }

        [Required(ErrorMessage = "新增內容為必填")]
        [StringLength(1000)]
        [Display(Name = "新增內容")]
        public string CaseComments { get; set; }

        [Required]
        [Display(Name = "新增日期")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string EmployeeId { get; set; }
        [ForeignKey("EmployeeId")] // 修正 ForeignKey 指向正確的屬性
        public Employee Employee { get; set; }

        //// --- 【請新增以下這兩行】 ---
        public int? CasePhotoId { get; set; }
        [ForeignKey("CasePhotoId")]
        public virtual CasePhoto? CasePhoto { get; set; }
    }
}
