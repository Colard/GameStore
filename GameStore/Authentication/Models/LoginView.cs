using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameStore.Authentication.Models
{
    public class LoginView
    {
        [Required(ErrorMessage = "Введіть ваш логін!")]
        [Display(Name = "Логін")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введіть ваш пароль!")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Запам'ятати мене")]
        public bool RememberMe { get; set; }
    }
}