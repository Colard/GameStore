using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GameStore.Validation;

namespace GameStore.Models
{
    public class UserAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Заповніть адресу!")]
        [Display(Name = "Адреса")]
        [MaxLength(40, ErrorMessage = "Максимальна довжина адреси 40 символів")]
        [RangeLength(40, MinimumLength = 6)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Заповніть поштовий індекс!")]
        [Display(Name = "Поштовий індекс")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Не коректний індекс!")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Клієнт")]
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
