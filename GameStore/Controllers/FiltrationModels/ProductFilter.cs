using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace GameStore.Controllers.FiltrationModels
{
    public class ProductFilter
    {
        public string Name { get; set; }
        public int? Category { get; set; }
        public int? Sort { get; set; }

        public IQueryable<Product> FilterProductByText(IQueryable<Product> products)
        {
            if (String.IsNullOrEmpty(Name)) return products;
            return products.Where(s => s.Name.Contains(Name));
        }

        public IQueryable<Product> FilterByCategory(IQueryable<Product> products)
        {
            if(Category == null || Category == 0) return products;
            return products.Where(s => s.Category.Id == Category);
        }

        public IQueryable<Product> SortByFilter(IQueryable<Product> products)
        {
            if (!Enum.IsDefined(typeof(OrderTypes), Sort)) return products;

            switch ((OrderTypes)Sort)
            {
                case OrderTypes.newer_one: break;
                case OrderTypes.cheaper: products = sortByLowPrice(products); break;
                case OrderTypes.expensive: products = sortByHightPrice(products); break;
                case OrderTypes.by_name: products = sortByName(products); break;
            }

            return products;
        }

        public List<SelectListItem> CreateSelectedCategoryFilterList(IQueryable<Category> categories)
        {
            List<SelectListItem> list =  CreateCategoryFilterList(categories);

            if (Category != null)
            {
                SelectListItem sli = list.FirstOrDefault(el => el.Value == Category.ToString());
                if (sli != null)
                {
                    sli.Selected = true;
                }
            }

            return list;
        }

        public static List<SelectListItem> CreateCategoryFilterList(IQueryable<Category> categories) 
        {
            List<SelectListItem> list = categories.Select(category => new SelectListItem
            {
                Text = category.Name,
                Value = category.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem { Text = "Будь-яка", Value = "0" });

            return list;
        }

        public static List<SelectListItem> CreateOrderSelectList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Спочатку нові", Value = ((int)OrderTypes.newer_one).ToString() },
                new SelectListItem { Text = "Спочатку дешевші", Value = ((int)OrderTypes.cheaper).ToString() },
                new SelectListItem { Text = "Спочатку дорожчі", Value = ((int)OrderTypes.expensive).ToString() },
                new SelectListItem { Text = "По назві", Value = ((int)OrderTypes.by_name).ToString() }
            };
        }

        private IQueryable<Product> sortByLowPrice(IQueryable<Product> product)
        {
            return product.OrderBy(el=> el.Price);
        }

        private IQueryable<Product> sortByHightPrice(IQueryable<Product> product)
        {
            return product.OrderByDescending(el => el.Price);
        }

        private IQueryable<Product> sortByName(IQueryable<Product> product)
        {
            return product.OrderBy(el => el.Name);
        }
    }
}