using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Parichay.MVC.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome! You are now logged-in to Parichay. Please click on Home button on top navigation bar to go to your home page.";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
