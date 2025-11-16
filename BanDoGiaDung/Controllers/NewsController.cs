using BanDoGiaDung.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using BanDoGiaDung.Services;

namespace BanDoGiaDung.Controllers
{
    public class NewsController : Controller
    {
        private NewsServices service = new NewsServices();

        public ActionResult TinTuc()
        {
            var list = service.GetAllNews();
            return View(list);
        }

        public ActionResult Details(int id)
        {
            var item = service.GetById(id);
            if (item == null) return HttpNotFound();
            return View(item);
        }

    }
}
