using BanDoGiaDung.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDoGiaDung.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private readonly GiaDungDbContext db = new GiaDungDbContext();

        public ActionResult Index(int? brand, int? genre, int? min, int? max, string sort, string search, int page = 1)
        {
            int pageSize = 12;
            var products = db.Products.Where(p => p.status == "1");

            // ====== LỌC THEO BRAND ======
            if (brand.HasValue)
                products = products.Where(p => p.brand_id == brand.Value);

            // ====== LỌC THEO GENRE ======
            if (genre.HasValue)
                products = products.Where(p => p.genre_id == genre.Value);

            // ====== LỌC GIÁ ======
            if (min.HasValue)
                products = products.Where(p => p.price >= min);

            if (max.HasValue)
                products = products.Where(p => p.price <= max);

            // ====== TÌM KIẾM ======
            if (!string.IsNullOrEmpty(search))
                products = products.Where(p => p.product_name.Contains(search));

            // ====== SẮP XẾP ======
            switch (sort)
            {
                case "az":
                    products = products.OrderBy(x => x.product_name);
                    break;

                case "za":
                    products = products.OrderByDescending(x => x.product_name);
                    break;

                case "price_asc":
                    products = products.OrderBy(x => x.price);
                    break;

                case "price_desc":
                    products = products.OrderByDescending(x => x.price);
                    break;

                case "newest":
                    products = products.OrderByDescending(x => x.create_at);
                    break;

                case "oldest":
                    products = products.OrderBy(x => x.create_at);
                    break;

                default:
                    products = products.OrderBy(x => x.product_id);
                    break;
            }

            // ====== PHÂN TRANG ======
            int totalItems = products.Count();
            var items = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Đưa dữ liệu sang ViewBag
            ViewBag.Brands = db.Brands.OrderBy(b => b.brand_name).ToList();
            ViewBag.Genres = db.Genres.OrderBy(g => g.genre_name).ToList();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = totalItems;

            ViewBag.Brand = brand;
            ViewBag.Genre = genre;
            ViewBag.Min = min;
            ViewBag.Max = max;
            ViewBag.Sort = sort;
            ViewBag.Search = search;

            return View(items);
        }

        public ActionResult Details(int id)
        {
            using (var db = new GiaDungDbContext())
            {
                var product = db.Products
                    .Include("Brand")
                    .Include("Genre")
                    .FirstOrDefault(p => p.product_id == id);

                if (product == null)
                    return HttpNotFound();

                // Lấy sản phẩm liên quan cùng genre
                var related = db.Products
                    .Where(p => p.genre_id == product.genre_id && p.product_id != id)
                    .Take(6)
                    .ToList();

                ViewBag.Related = related;

                return View(product);
            }
        }


    }
}