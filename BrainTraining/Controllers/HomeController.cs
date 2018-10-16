using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainTraining.Models;

namespace BrainTraining.Controllers
{
    public class HomeController : Controller
    {
        public static string Name = "";
        public static string Email = "";
        public ActionResult Index() { if (User.Identity.IsAuthenticated) { Name = UserName.Name(HttpContext.User.Identity.Name); } return View(); }
    }
}