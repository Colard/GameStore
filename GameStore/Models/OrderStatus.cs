using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GameStore.Models
{
    public class OrderStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Статус замовлення")]
        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        public string Status { get; set; }
    }
}
