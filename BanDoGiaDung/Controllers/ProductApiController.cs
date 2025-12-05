using BanDoGiaDung.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace BanDoGiaDung.Controllers
{
    public class ProductApiController : ApiController
    {
        private readonly GiaDungDbContext db = new GiaDungDbContext();

        // =======================================================
        // 1️⃣ API DANH SÁCH SẢN PHẨM (FILTER + SORT + PAGE)
        // =======================================================
        [HttpGet]
        [Route("api/productapi")]
        public IHttpActionResult GetProducts(
            int? brand = null,
            int? genre = null,
            int? min = null,
            int? max = null,
            string search = null,
            string sort = "default",
            int page = 1,
            int pageSize = 12)
        {
            var products = db.Products.Where(p => p.status == "1");

            if (brand.HasValue)
                products = products.Where(p => p.brand_id == brand.Value);

            if (genre.HasValue)
                products = products.Where(p => p.genre_id == genre.Value);

            if (min.HasValue)
                products = products.Where(p => p.price >= min.Value);

            if (max.HasValue)
                products = products.Where(p => p.price <= max.Value);

            if (!string.IsNullOrEmpty(search))
                products = products.Where(p => p.product_name.Contains(search));

            switch (sort)
            {
                case "az": products = products.OrderBy(p => p.product_name); break;
                case "za": products = products.OrderByDescending(p => p.product_name); break;
                case "price_asc": products = products.OrderBy(p => p.price); break;
                case "price_desc": products = products.OrderByDescending(p => p.price); break;
                case "newest": products = products.OrderByDescending(p => p.create_at); break;
                case "oldest": products = products.OrderBy(p => p.create_at); break;
                default: products = products.OrderBy(p => p.product_id); break;
            }

            int totalItems = products.Count();
            var data = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    p.product_id,
                    p.product_name,
                    p.price,
                    p.image
                })
                .ToList();

            return Ok(new
            {
                totalItems,
                page,
                pageSize,
                totalPage = (int)System.Math.Ceiling((double)totalItems / pageSize),
                data
            });
        }


        // =======================================================
        // 2️⃣ API CHI TIẾT SẢN PHẨM
        //      GET /api/productapi/5
        // =======================================================
        [HttpGet]
        [Route("api/productapi/{id}")]
        public IHttpActionResult GetProduct(int id)
        {
            var p = db.Products
                .Where(x => x.product_id == id)
                .Select(x => new
                {
                    x.product_id,
                    x.product_name,
                    x.price,
                    x.image,
                    x.description,
                    brand_name = x.Brand.brand_name,
                    genre_name = x.Genre.genre_name,
                    x.genre_id,
                    create_at = x.create_at   // KHÔNG format ở đây
                })
                .FirstOrDefault();

            if (p == null)
                return NotFound();

            return Ok(p);
        }


        // =======================================================
        // 3️⃣ API SẢN PHẨM LIÊN QUAN
        //      GET /api/productapi/related/5
        // =======================================================
        [HttpGet]
        [Route("api/productapi/related/{id}")]
        public IHttpActionResult GetRelatedProducts(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return NotFound();

            var related = db.Products
                .Where(x => x.genre_id == product.genre_id && x.product_id != id)
                .Take(6)
                .Select(x => new
                {
                    x.product_id,
                    x.product_name,
                    x.price,
                    x.image
                })
                .ToList();

            return Ok(related);
        }
    }
}