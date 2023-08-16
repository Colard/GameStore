using GameStore.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GameStore.Models
{
    public class OrderProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        public int OrderId { get; set; }
        
        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Кількість товару")]
        [NumberMin(1, ErrorMessage = "Мінімальна кількість товару - 1")]
        [NumberMax(20, ErrorMessage = "Максимальна кількість товару - 20")]
        public int Count { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
