using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GameStore.Authentication;
using GameStore.Authentication.Attributes;
using GameStore.Controllers.FiltrationModels;
using GameStore.Models;


namespace GameStore.Controllers
{
    public class CartController : Controller
    {
        private GameStoreDBContext db = new GameStoreDBContext();

        // GET: Cart
        [CustomAuthorize]
        public ActionResult Index()
        {
            int userId = CustomMembership.getCurrentUser().UserId;
            var cart = db.Carts.Where(el => el.UserId == userId).Include(el => el.Product);
            return View(cart.ToList());
        }

        [CustomAuthorize]
        public JsonResult AddToCart(Cart cart)
        {
            JSONMassage msg = new JSONMassage();

            if (cart.ProductId == 0)
            {
                msg.ResponseType = "error";
                msg.Massage = "Помилка обробки даних";
                return Json(msg);
            }

            cart.UserId = CustomMembership.getCurrentUser().UserId;
            if (db.Carts.Any(el => el.UserId == cart.UserId && el.ProductId == cart.ProductId))
            {
                msg.ResponseType = "error";
                msg.Massage = "Товар уже міститься в корзині";
                return Json(msg);
            }

            if (cart.Count <= 0)
            {
                msg.ResponseType = "error";
                msg.Massage = "Мінімальна кількість товару 1";
                return Json(msg);
            }

            if (cart.Count > 20)
            {
                msg.ResponseType = "error";
                msg.Massage = "Максимальна кількість товару 20";
                return Json(msg);
            }

            int productCount = db.Products.Find(cart.ProductId).Count;
            if (cart.Count > productCount)
            {
                msg.ResponseType = "error";
                msg.Massage = "Товару на складі: " + productCount;
                return Json(msg);
            }

            msg.ResponseType = "success";
            msg.Massage = "Товар додано до кошика!";

            db.Carts.Add(cart);
            db.SaveChanges();

            return Json(msg);
        }

        [CustomAuthorize]
        [HttpPost]
        public JsonResult Delete(int? id)
        {
            JSONMassage msg = new JSONMassage();
            if (id == null)
            {
                msg.ResponseType = "error";
                msg.Massage = "Помилка обробки даних";
                return Json(msg);
            }

            int userId = CustomMembership.getCurrentUser().UserId;
            Cart cartEl = db.Carts.SingleOrDefault(p => p.ProductId == id && p.UserId == userId);

            if (cartEl == null)
            {
                msg.ResponseType = "error";
                msg.Massage = "Помилка обробки даних";
                return Json(msg);
            }

            msg.ResponseType = "success";
            msg.Massage = "Товар успішно видалено!";

            db.Carts.Remove(cartEl);
            db.SaveChanges();
            return Json(msg);
        }
    }
}
