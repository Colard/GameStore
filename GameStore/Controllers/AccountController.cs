using GameStore.Authentication;
using GameStore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GameStore.Authentication.Models;
using GameStore.Authentication.Attributes;
using System.Diagnostics;
using System.Net.Sockets;

namespace GameStore.Controllers
{

    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [CustomAuthorize]
        public ActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView, string ReturnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = ReturnUrl;
                return View(loginView);
            }

            if (verifyUser(loginView)) return redirectToUrl(ReturnUrl);

            ModelState.AddModelError("", "Логін або пароль не вірний!");
            ViewBag.ReturnUrl = ReturnUrl;
            return View(loginView);
        }

        [HttpGet]
        public ActionResult Registration(string ReturnUrl = "")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User user, string ReturnUrl = "")
        {
            using (GameStoreDBContext dbContext = new GameStoreDBContext())
            {
                IQueryable<User> problemedUsers = dbContext.Users
                    .Where(currentUser =>
                        currentUser.Login == user.Login ||
                        currentUser.Email == user.Email
                        );

                if (problemedUsers.Any(currentUser => currentUser.Login == user.Login))
                {
                    ModelState.AddModelError("Login", "Користувач з таким логіном вже існує!");
                };

                if (problemedUsers.Any(currentUser => currentUser.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Користувач з такою поштою вже існує!");
                };
            }

            if (!ModelState.IsValid) {
                ViewBag.ReturnUrl = ReturnUrl;
                return View(user);
            } 

            using (GameStoreDBContext dbContext = new GameStoreDBContext())
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }

            LoginView loginView = new LoginView() {
                Login = user.Login,
                Password = user.Password,
            };

            verifyUser(loginView);

            return redirectToUrl(ReturnUrl);
        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("UserCookie", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        private ActionResult redirectToUrl(string ReturnUrl)
        {
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private bool verifyUser(LoginView loginView)
        {
            if (Membership.ValidateUser(loginView.Login, loginView.Password))
            {
                setUserCookie(loginView);
                return true;
            }
            return false;
        }

        private void setUserCookie(LoginView loginView) {
            CustomMembershipUser user = (CustomMembershipUser)Membership.GetUser(loginView.Login, false);
            if (user != null)
            {
                UserSerializeModel userModel = new UserSerializeModel()
                {
                    Id = user.UserId,
                    RoleName = user.Roles.Select(r => r.RoleName).ToList()
                };

                string userData = JsonConvert.SerializeObject(userModel);
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1, 
                    loginView.Login,
                    DateTime.Now,
                    DateTime.Now.AddDays(14),
                    true,
                    userData
                );

                string enTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie("UserCookie", enTicket);
                if (authTicket.IsPersistent)
                    faCookie.Expires = authTicket.Expiration;

                Response.Cookies.Add(faCookie);
            }
        }

    }
}