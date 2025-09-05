using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineManagementSystemVer2.Models
{
    public class CasePhoto
    {
        [Key]
        public int PhotoId { get; set; }

        [ForeignKey("RepairCase")]
        public int CaseId { get; set; }
        public RepairCase RepairCase { get; set; }

        [Required]
        [Display(Name = "相關照片")]
        public byte[] PhotoData { get; set; } // 存 DB

        [StringLength(200)]
        [Display(Name = "檔案名稱")]
        public string FileName { get; set; }

        [Required]
        [Display(Name = "上傳日期")]
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
