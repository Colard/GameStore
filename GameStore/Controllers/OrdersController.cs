using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GameStore.Authentication;
using GameStore.Authentication.Attributes;
using GameStore.Models;

namespace GameStore.Controllers
{
    [CustomAuthorize]
    public class OrdersController : Controller
    {
        private GameStoreDBContext db = new GameStoreDBContext();

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult Create()
        {
            int userId = CustomMembership.getCurrentUser().UserId;
            User user = db.Users.Find(userId);


            IQueryable<Cart> cartItems = getUserCart(userId);
            if (cartItems.Count() == 0)
            {
                return RedirectToAction("Index","Cart");
            }

            List<SelectListItem> addresses = db.UsersAdresses
            .Where(el => el.UserId == userId)
            .OrderBy(el => el.Address)
            .Select(el => new SelectListItem
            {
                Text = el.Address,
                Value = el.Address,
            })
            .ToList();

            Order initalOrder = new Order
            {
                FullName = user.LastName + " " + user.FirstName + " " + user.MiddleName,
                Phone = user.Phone,
            };

            ViewBag.Address = addresses;
            ViewBag.Carts = cartItems.ToList();
            return View(initalOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            int userId = CustomMembership.getCurrentUser().UserId;
            User user = db.Users.Find(userId);

            IQueryable<Cart> cartItems = getUserCart(userId);
            if (cartItems.Count() == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            if (ModelState.IsValidField("FullName") && ModelState.IsValidField("Phone") && ModelState.IsValidField("Address"))
            {
                order.UserId = userId;
                order.Date = DateTime.Now;
                db.Orders.Add(order);

                List<Cart> cartList = cartItems.ToList();

                foreach (Cart cart in cartList)
                {
                    //add elemet to order
                    db.OrderProducts.Add(new OrderProduct { ProductId = cart.ProductId, OrderId = order.Id, Count = cart.Count });

                    //reduse product count
                    Product product = db.Products.Find(cart.ProductId);
                    product.Count -= cart.Count;
                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;

                    //Remove elements from cart
                    db.Carts.Remove(cart);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<SelectListItem> addresses = db.UsersAdresses
                .Where(el => el.UserId == userId)
                .OrderBy(el => el.Address)
                .Select(el => new SelectListItem
                {
                    Text = el.Address,
                    Value = el.Address,
                })
                .ToList();

            ViewBag.Address = addresses;
            ViewBag.Carts = cartItems.ToList();
            return View(order);
        }

        private IQueryable<Cart> getUserCart(int userId)
        {
            return db.Carts
                .Include(el => el.Product)
                .Where(el => el.UserId == userId && el.Product.Count >= el.Count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
