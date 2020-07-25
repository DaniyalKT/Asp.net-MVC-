using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace JanaFand.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        JanaFandEntities db = new JanaFandEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowContact()
        {
            return PartialView(db.Product_Comments.ToList());
        }

        public void DeleteCommant(int id)
        {
            var commant = db.Product_Comments.Find(id);
            if (db.Product_Comments.Any())
            {
                foreach (var subComment in db.Product_Comments.Where(c => c.PersonID == id))
                {
                    db.Product_Comments.Remove(subComment);
                }
            }
            db.Product_Comments.Remove(commant);
            db.SaveChanges();
        }


        public ActionResult ShowUserInDashbord()
        {
            return PartialView(db.Users.OrderByDescending(u => u.RegisterDate).Take(5).ToList());
        }

        public ActionResult Factor()
        {
            var factor = db.FactorDetails.ToList();
            ViewBag.factorDetail = factor.DistinctBy(f => f.Products.ProductTitle).ToList();
            
            return PartialView(factor);
        }
        public void DeleteFactor(int id)
        {
            var factor = db.Factors.Find(id);
            db.FactorDetails.Where(f => f.FactorID == factor.FactorID).ToList().ForEach(f => db.FactorDetails.Remove(f));
            db.Factors.Remove(factor);
            db.SaveChanges();

        }


    }
}