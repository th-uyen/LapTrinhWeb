using BanDoGiaDung.Models;
using BanDoGiaDung.Services;
using PagedList;
using PagedList.Mvc;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace BanDoGiaDung.Controllers
{
    public class NewsController : Controller
    {
        private NewsServices service = new NewsServices();

        public ActionResult TinTuc(int ?page)   
        {
            var list = service.GetAllNews();
            int pageSize = 6;
            int pageNumber = page ?? 1;
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id)
        {
            var item = service.GetById(id);
            if (item == null) return HttpNotFound();
            return View(item);
        }

    }
}
