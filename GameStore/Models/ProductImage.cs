using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Models
{
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Статус замовлення")]
        byte[] url { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Товар")]
        public int ProductId { get; set; }
        
        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
