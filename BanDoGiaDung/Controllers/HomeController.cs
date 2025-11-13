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
    }
}