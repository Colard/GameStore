using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using GameStore.Models;
using static System.Net.Mime.MediaTypeNames;

namespace GameStore.Controllers
{
    public class ProductsController : Controller
    {
        private GameStoreDBContext db = new GameStoreDBContext();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
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
            return View(product);
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
            ViewBag.SelectedImage = convertFromBase64ToImageString(product.MainPhoto);
            ViewBag.ImageFile = convertArrOfBase64ToImageStringArr(product.ProductImages.Select(image => image.Url).ToList());

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

            if (!checkImgAndReturnBase64String(imageUrl, out base64bytes)) return null;

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

        private bool checkFileFormat(string file, Regex exp)
        {
            return exp.IsMatch(file);
        }

        private bool TryGetFromBase64String(string input, out byte[] output)
        {
            output = null;
            try
            {
                output = Convert.FromBase64String(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool checkImgAndReturnBase64String(string input, out byte[] output)
        {
            Regex exp = new Regex(@"^data:image/jpeg;base64,|^data:image/png;base64,");
            if (checkFileFormat(input, exp))
            {
                string base64String = exp.Replace(input, String.Empty);
                return TryGetFromBase64String(base64String, out output);
            }

            output = null;
            return false;
        }

        private string convertFromBase64ToImageString(byte[] input)
        {
            return "data:image/png;base64," +  Convert.ToBase64String(input, 0, input.Length);
        }

        private IEnumerable<string> convertArrOfBase64ToImageStringArr(IEnumerable<byte[]> input) 
        { 
            List<string> output = new List<string> ();
            foreach (byte[] byteArr in input) 
            {
                output.Add(convertFromBase64ToImageString(byteArr));
            }

            return output;
        }
    }
}
