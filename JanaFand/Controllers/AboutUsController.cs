using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JanaFand.Controllers
{
    public class AboutUsController : Controller
    {
        DataLayer.JanaFandEntities db = new DataLayer.JanaFandEntities();
        // GET: AboutUs


        public ActionResult Index()
        {
            return View(db.AboutUs.ToList());
        }
    }
}