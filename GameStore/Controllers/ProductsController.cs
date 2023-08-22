using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using GameStore.Models;
using GameStore.Controllers.Functions.Data;
using GameStore.Authentication.Attributes;
using GameStore.Controllers.FiltrationModels;
using PagedList;
using System.Data.Entity;

namespace GameStore.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private GameStoreDBContext db = new GameStoreDBContext();

        // GET: Products
        public ActionResult Index(int? page, ProductFilter filter, int filtering = 0)
        {
            if (filtering != 0)
            {
                return RedirectToAction("Index", "Products", new { Sort = filter.Sort, category = filter.Category, name = filter.Name });
            }

            var products = db.Products.Include(p => p.Category);

            if (!String.IsNullOrEmpty(filter.Name))
            {
                products = filter.FilterProductByText(products);
                ViewBag.NameSearch = filter.Name;
            }

            if (filter.Category != null && filter.Category != 0)
            {
                products = filter.FilterByCategory(products);
                ViewBag.CoosedCategory = filter.Category;
            }

            //Full categories DropList
            if (filter.Category == null)
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
            return View(products.ToList().ToPagedList(pageNumber, 20));
        }


        // GET: Products/Create
        public ActionResult Create()
        {
            SelectList categories = new SelectList(db.Categories.OrderBy(el=>el.Name), "Id", "Name"); ;
            ViewBag.Categories = categories;
            return View();
        }


        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, string selectedImage, IEnumerable<string> imageFile)
        {
            IEnumerable<byte[]> imageList = new List<byte[]>();
            byte[] mainImage = null;

            if (selectedImage == null || !selectedImage.Any())
            {
                ModelState.AddModelError("", "Ви не обрали головне фото!");
            }
            else 
            {
               mainImage = convertImageToByte(selectedImage);
            }

            if (imageFile == null || !imageFile.Any())
            {
                ModelState.AddModelError("", "Ви не завантажили жодне фото!");
            }
            else
            {
                imageList = convertImageArrToByte(imageFile);
            }

            if ((imageFile != null && imageFile.Any() && !imageList.Any()) || 
                (selectedImage != null && selectedImage.Any() && mainImage == null))
            {
                 ModelState.AddModelError("", "Задані не коректні формати файлів!");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.SelectedImage = selectedImage;
                ViewBag.ImageFile = imageFile;
                SelectList categories = new SelectList(db.Categories.OrderBy(el => el.Name), "Id", "Name");
                ViewBag.Categories = categories;
                return View(product);
            }

            //create entities
            db.Products.Add(product);
            product.MainPhoto = mainImage;
            product.ProductImages = (List<ProductImage>)convertByteArrToProductImage(imageList, product.Id);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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
            SelectList categories = new SelectList(db.Categories.OrderBy(el => el.Name), "Id", "Name"); ;

            ViewBag.Categories = categories;
            ViewBag.SelectedImage = JSImageByteConverter.FromBase64ToImageString(product.MainPhoto);
            ViewBag.ImageFile = (JSImageByteConverter.ArrOfBase64ToImageString(product.ProductImages.Select(image => image.Url).ToList()));

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, string selectedImage, IEnumerable<string> imageFile)
        {
            IEnumerable<byte[]> imageList = new List<byte[]>();
            byte[] mainImage = null;

            if (selectedImage == null || !selectedImage.Any())
            {
                ModelState.AddModelError("", "Ви не обрали головне фото!");
            }
            else
            {
                mainImage = convertImageToByte(selectedImage);
            }

            if (imageFile == null || !imageFile.Any())
            {
                ModelState.AddModelError("", "Ви не завантажили жодне фото!");
            }
            else
            {
                imageList = convertImageArrToByte(imageFile);
            }

            if ((imageFile != null && imageFile.Any() && !imageList.Any()) ||
                (selectedImage != null && selectedImage.Any() && mainImage == null))
            {
                ModelState.AddModelError("", "Задані не коректні формати файлів!");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.SelectedImage = selectedImage;
                ViewBag.ImageFile = imageFile;
                SelectList categories = new SelectList(db.Categories.OrderBy(el => el.Name), "Id", "Name");
                ViewBag.Categories = categories;
                return View(product);
            }

            product.MainPhoto = mainImage;

            db.Database.ExecuteSqlCommand("DELETE FROM ProductImages WHERE ProductId = {0}", product.Id);
            List < ProductImage > list = (List < ProductImage > )convertByteArrToProductImage(imageList, product.Id);
            db.ProductImages.AddRange( list );

            db.Entry(product).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Products/Delete/5
        public void Delete(int? id)
        {
            if (id == null)
            {
                return;
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return;
            }

            db.Products.Remove(product);
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private byte[] convertImageToByte(string imageUrl) {
            if (imageUrl == null) return null;

            byte[] base64bytes = null;

            if (!JSImageByteConverter.CheckImgStringToBase64(imageUrl, out base64bytes)) return null;

            return base64bytes;
        }

        private IEnumerable<byte[]> convertImageArrToByte(IEnumerable<string> imagesUrl)
        {
            List<byte[]> imageList = new List<byte[]>();
            
            foreach(string imagesItem in imagesUrl) 
            {
                if (imagesItem == null) continue;

                byte[] image = convertImageToByte(imagesItem);

                if (image == null) continue;
                imageList.Add(image);
            }

            return imageList;
        }

        private ProductImage convertByteToProductImage(byte[] image, int productId)
        {
            ProductImage el = new ProductImage { ProductId = productId, Url = image };
            return el;
        }

        private IEnumerable<ProductImage> convertByteArrToProductImage(IEnumerable<byte[]> images, int productId)
        {
            List< ProductImage > imagesList = new List< ProductImage >();
            foreach (byte[] imagesItem in images) 
            {
                imagesList.Add(convertByteToProductImage(imagesItem, productId));

            }
            return imagesList;
        }
    }
}
