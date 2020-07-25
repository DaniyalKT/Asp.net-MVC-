using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
namespace JanaFand.Areas.Admin.Controllers
{
    public class SearchController : Controller
    {
        JanaFandEntities db = new JanaFandEntities();
        // GET: Admin/Search

      
        public ActionResult SearchUser(string q)
        {
            List<Users> list = new List<Users>();

            list.AddRange(db.Users.Where(u => u.UserName.Contains(q) || u.Email.Contains(q)).ToList());
    

            ViewBag.search = q;
            return View(list.Distinct());
        }

        public ActionResult SearchProduct(string P)
        {
            List<Products> list = new List<Products>();

            list.AddRange(db.Products.Where(p => p.ProductTitle.Contains(P) || p.Text.Contains(P) || p.ShortDescription.Contains(P) ).ToList());
            list.AddRange(db.Product_Tags.Where(t => t.Tags == P).Select(t => t.Products).ToList());

            return View(list.Distinct());
        }


   
    }
}