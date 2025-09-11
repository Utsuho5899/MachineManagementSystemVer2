using System;
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
        public byte[] PhotoData { get; set; } // 將照片的二進位資料直接存入資料庫

        [StringLength(200)]
        [Display(Name = "檔案名稱")]
        public string FileName { get; set; }

        [Required]
        [Display(Name = "上傳日期")]
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}

