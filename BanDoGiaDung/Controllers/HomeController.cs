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
            return View();
        }

        public ActionResult BestSeller()
        {
            return View();
        }
    }
}