using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace GameStore.Authentication
{
    public class CustomPrincipal : IPrincipal
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public bool IsInRole(string role)
        {
            return Roles.Any(r => role.Contains(r));
        }

        public CustomPrincipal(string login)
        {
            Identity = new GenericIdentity(login);
        }
    }
}