using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineManagementSystemVer2.Models
{
    public class CaseComment
    {
        [Key]
        public int CommentId { get; set; }

        [ForeignKey("RepairCase")]
        public int CaseId { get; set; }
        public RepairCase RepairCase { get; set; }

        [Required, StringLength(1000)]
        [Display(Name = "新增內容")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "新增日期")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("新增人員")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

       }
}
