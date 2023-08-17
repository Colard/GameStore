using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace GameStore.Authentication
{
    public class CustomMembershipUser : MembershipUser { 

        public int UserId { get; set; }
        public string Login { get; set; }
        public ICollection<Role> Roles { get; set; }

        public CustomMembershipUser(User user) : base("CustomMembership", user.Login, user.Id, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.Id;
            Login = user.Login;
            Roles = user.Roles;
        }
    }
}