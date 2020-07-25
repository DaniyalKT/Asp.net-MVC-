using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace JanaFand.Controllers
{
    public class HomeController : Controller
    {
        JanaFandEntities db = new JanaFandEntities();
        // GET: Home

        public ActionResult Index(int pageId = 1)
        {


            int take = 12;
            int skip = (pageId - 1) * take;
            ViewBag.pageCount = db.Products.Count() / take;
            return View(db.Products.OrderByDescending(g => g.CreateDate).Skip(skip).Take(take).ToList());
        }

        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult Slider()
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            return PartialView(db.Slider.Where(s => s.IsActive && s.SratrDate <= dt && s.EndDate >= dt));
        }

        public ActionResult ShowGroupInMenu()
        {
            ViewBag.Groups = db.Product_Groups.ToList();

            return PartialView();
        }

        public ActionResult ShowCommentsInMainPage()
        {
            var comment = db.Product_Comments.ToList();
            ViewBag.user = db.Users.ToList();
            return PartialView(comment);
        }

        public ActionResult ShowProductsInMainPage()
        {
            return PartialView(db.Products.ToList());

        }

        public ActionResult Groups()
        {
            return PartialView(db.Product_Groups.ToList());
        }

        public ActionResult Companies()
        {
            var company = db.Companies.ToList();
            return PartialView(company);
        }

        public ActionResult OurChoice()
        {
            var choice = db.OurChoice.Take(4).ToList();
            return PartialView(choice);
        }



        public ActionResult SocialNetwork()
        {

            return PartialView(db.SocialNetwork.ToList());
        }
        public ActionResult ContactUsInMainPage()
        {
            return PartialView(db.ContactUs.ToList());
        }

        public ActionResult ShowGroupInFoorer()
        {
            return PartialView(db.Product_Groups.ToList());
        }

    }
}