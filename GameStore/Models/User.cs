using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Collections.Generic;
using GameStore.Validation;

namespace GameStore.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "По батькові")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Ел. адреса" )]
        [EmailAddress(ErrorMessage = "Не коректна ел. адреса")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Логін")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [RangeLength(30, MinimumLength = 6)]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [NotMapped]
        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Повторіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Номер телефону")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        public virtual ICollection<Order> Orders { get; set; } 
        public virtual ICollection<Role> Roles { get; set; }
    }
}
