using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GameStore.Validation;
using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Дані отримувача")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Номер телефону отримувача")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Адреса отримувача")]
        public string Address { get; set; }

        [Display(Name = "Дата замовлення")]
        public DateTime Date { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
