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

        public ActionResult Index()
        {
            return View();
        }

        // Trang chi tiết sản phẩm
        public ActionResult Details(int id)
        {
            // View Details sẽ tự dùng API để lấy dữ liệu
            ViewBag.Id = id;
            return View();
        }

        // Trang demo Ajax
        public ActionResult Ajax()
        {
            return View();
        }
    }
}