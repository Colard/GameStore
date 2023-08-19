using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameStore.Models;
using GameStore.Controllers.Functions.Data;
using PagedList;
using GameStore.Controllers.FiltrationModels;
using System.Diagnostics;
using System.Web.Services.Description;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace GameStore.Controllers
{
    public class ShopController : Controller
    {
        private GameStoreDBContext db = new GameStoreDBContext();

        // GET: Shop
        public ActionResult Index(int? page, ProductFilter filter, int filtering = 0 )
        {
            var products = db.Products.Include(p => p.Category);

            filter.FilterProductByText(products);

            if (filter.Category != null && filter.Category != 0)
            {   
                products = filter.FilterByCategory(products);
                ViewBag.CoosedCategory = filter.Category;
            }

            if(filtering != 0)
            {
                page = 1;
            }

            //Full categories DropList
            if(filter.Category == null)
            {
                ViewBag.CategoriesFilter = filter.CreateSelectedCategoryFilterList(db.Categories);
            } 
            else
            {
                ViewBag.CategoriesFilter = ProductFilter.CreateCategoryFilterList(db.Categories);
            }

            //Full sorting DropList
            if (filter.Sort != null && Enum.IsDefined(typeof(OrderTypes), filter.Sort)) 
            {
                products = filter.SortByFilter(products);
                ViewBag.Sort = filter.Sort;
            }
            ViewBag.SortFilter = ProductFilter.CreateOrderSelectList();

            int pageNumber = (page ?? 1);
            return View(products.ToList().ToPagedList(pageNumber, 6));
        }

        // GET: Shop/Details/5
        public ActionResult Buy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            product.ProductImages = db.ProductImages.Where(el => el.ProductId == product.Id).ToList();
            return View(product);
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
