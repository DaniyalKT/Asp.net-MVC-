using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace JanaFand.Controllers
{
    public class ContactUsController : Controller
    {
        DataLayer.JanaFandEntities db = new DataLayer.JanaFandEntities();
        // GET: ContactUs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowContactUs()
        {
           
            return PartialView(db.ContactUs.ToList());
        }

      
    }
}