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
        [Display(Name = "Статус замовлення")]
        public int OrderStatusId { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Дата замовлення")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Адреса замовника")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Ціна за замовлення")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        
        [ForeignKey("OrderStatusId")]
        public OrderStatus OrderStatus { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
