using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Boxing.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.Cookies.Get("adminToken") != null || Request.Cookies.Get("authToken") != null)
            {
                ViewBag.type = 1;
                ViewBag.id = Request.Cookies.Get("id").Value;
                return View("IndexSigned");
            }
            else {
                ViewBag.type = 0;
                return View("IndexUnsigned");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}