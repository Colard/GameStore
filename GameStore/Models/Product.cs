using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GameStore.Validations;
using System.Collections.Generic;

namespace GameStore.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Назва товару")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Короткі відомості")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Ціна")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Кількість товару на складі")]
        [NumberMin(1, ErrorMessage = "Мінімальна кількість товару - 1")]
        public int Count { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        public byte[] MainPhoto { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
