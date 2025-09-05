using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineManagementSystemVer2.Models
{
    public class Plant
    {
        [Key]
        public int PlantId { get; set; }

        [Required(ErrorMessage ="廠區名稱必填"), StringLength(50)]
        [Display(Name = "廠區名稱")]
        public string PlantName { get; set; }

        [StringLength(15)]
        [Display(Name = "廠區代稱")]
        public string PlantCode { get; set; }

        [Required(ErrorMessage = "廠區地址必填"), StringLength(100)]
        [Display(Name = "廠區地址")]
        public string Address { get; set; }

        [Required(ErrorMessage = "廠區電話必填"), StringLength(15)]
        [Display(Name = "廠區電話")]
        public string Phone { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
