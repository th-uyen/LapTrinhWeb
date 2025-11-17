using BanDoGiaDung.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace BanDoGiaDung.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private readonly GiaDungDbContext db = new GiaDungDbContext();
        public ActionResult Index()
        {
            ViewBag.Genres = db.Genres.OrderBy(g => g.genre_name).ToList();
            return View();
        }

        public ActionResult Banner()
        {
            return View();
        }

        public ActionResult Benefits()
        {
            return View();
        }

        public ActionResult NewProducts()
        {
            var list = db.Products
                 .OrderByDescending(x => x.create_at)
                 .Take(10)
                 .ToList();

            return PartialView(list);
        }

        public ActionResult Categories()
        {
            return PartialView();
        }


        public ActionResult BestSeller()
        {
            var list = db.Products
                         .OrderByDescending(p => p.buyturn)
                         .Take(10)
                         .ToList();

            return PartialView(list);
        }

    }
}