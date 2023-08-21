using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Authentication.Models
{
    public class UserSerializeModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public ICollection<string> RoleName { get; set; }
    }
}