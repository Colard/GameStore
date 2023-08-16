using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GameStore.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        
        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Роль")]
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
