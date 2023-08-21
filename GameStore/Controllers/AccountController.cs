using GameStore.Authentication;
using GameStore.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GameStore.Authentication.Models;
using GameStore.Authentication.Attributes;
using System.Data.Entity;
using GameStore.Controllers.FiltrationModels;
using System.Dynamic;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace GameStore.Controllers
{

    public class AccountController : Controller
    {
        GameStoreDBContext db = new GameStoreDBContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [CustomAuthorize]
        public ActionResult Profile()
        {

            int userId = CustomMembership.getCurrentUser().UserId;

            User user = db.Users
                .Include(el => el.UserAddresses)
                .Include(el => el.Orders
                    .Select(order => order.OrderProducts
                        .Select(orderProduct => orderProduct.Product)))
                .FirstOrDefault(el => el.Id == userId);

            user.UserAddresses = user.UserAddresses.OrderBy(el => el.Address).ToList();
            return View(user);
        }

        [CustomAuthorize]
        public string CheckAuthorize()
        {
            return "success";
        }

        [HttpPost]
        [CustomAuthorize]
        public JsonResult CreateAdress(UserAddress userAddress)
        {
            AddressJSONMassage  msg = new AddressJSONMassage();
            if (!ModelState.IsValidField("Address"))
            {
                msg.ResponseType = "error";
                msg.Massage = ModelState["Address"].Errors.First()?.ErrorMessage;
                return Json(msg);
            }
            if (!ModelState.IsValidField("PostCode"))
            {
                msg.ResponseType = "error";
                msg.Massage = ModelState["PostCode"].Errors.First()?.ErrorMessage;
                return Json(msg);
            }

            userAddress.UserId = CustomMembership.getCurrentUser().UserId;
            db.UsersAdresses.Add(userAddress);
            db.SaveChanges();

            msg.ResponseType = "success";
            msg.Massage = userAddress.Address;
            msg.Id = userAddress.Id;
            return Json(msg);
        }

        [HttpPost]
        [CustomAuthorize]
        public JsonResult DeleteAdress(int? id)
        {
            JSONMassage msg = new JSONMassage();
            if (id == null)
            {
                msg.ResponseType = "error";
                msg.Massage = "Помилка обробки даних";
                return Json(msg);
            }

            int userId = CustomMembership.getCurrentUser().UserId;
            UserAddress address = db.UsersAdresses.SingleOrDefault(p => p.Id == id && p.UserId == userId);

            if (address == null)
            {
                msg.ResponseType = "error";
                msg.Massage = "Помилка обробки даних";
                return Json(msg);
            }

            msg.ResponseType = "success";
            msg.Massage = "Адресу успішно видалено!";

            db.UsersAdresses.Remove(address);
            db.SaveChanges();
            return Json(msg);
        }

        [HttpGet]
        [CustomAuthorize]
        public ActionResult EditUser()
        {
            var curUser = CustomMembership.getCurrentUser().UserId;
            User user = db.Users.Find(curUser);

            if(user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult EditUser(User userData)
        {
            ModelState["Password"].Errors.Clear();
            ModelState["ConfirmPassword"].Errors.Clear();

            IQueryable<User> problemedUsers = db.Users
                .Where(currentUser =>
                    currentUser.Login == userData.Login ||
                    currentUser.Email == userData.Email
                    );

            var curUserId = CustomMembership.getCurrentUser().UserId;
            User user = db.Users.Find(curUserId);

            if (problemedUsers.Any(
                currentUser => currentUser.Login == userData.Login &&
                currentUser.Login != user.Login))
            {
                ModelState.AddModelError("Login", "Користувач з таким логіном вже існує!");
            };

            if (problemedUsers.Any(
                currentUser => currentUser.Email == userData.Email
                && currentUser.Email != user.Email))
            {
                ModelState.AddModelError("Email", "Користувач з такою поштою вже існує!");
            };

            if (!ModelState.IsValid) {
                return View(userData);
            }

            user.FirstName = userData.FirstName;
            user.LastName = userData.LastName;
            user.MiddleName = userData.MiddleName;
            user.Login = userData.Login;
            user.Email = userData.Email;
            user.Phone = userData.Phone;
            user.Password = user.Password;
            user.ConfirmPassword = user.Password;

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            LoginView loginView = new LoginView() {
                Login = user.Login,
                Password = user.Password,
            };

            verifyUser(loginView);

            return RedirectToAction("Profile"); ;
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

            IQueryable<User> problemedUsers = db.Users
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
            

            if (!ModelState.IsValid) {
                ViewBag.ReturnUrl = ReturnUrl;
                return View(user);
            }

            db.Users.Add(user);
            db.SaveChanges();
            
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
                    Login = user.Login,
                    Email = user.Email,
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