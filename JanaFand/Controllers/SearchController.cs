using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace JanaFand.Controllers
{
    public class SearchController : Controller
    {
        JanaFandEntities db = new JanaFandEntities();
        public ActionResult Index(string q)
        {
            
            List<Products> list = new List<Products>();

            list.AddRange(db.Product_Tags.Where(t => t.Tags == q).Select(t => t.Products).ToList());
            list.AddRange(db.Products.Where(p => p.ProductTitle.Contains(q) || p.ShortDescription.Contains(q) || p.Text.Contains(q)).ToList());
         

    
            
            return View(list.Distinct());
        }
    }
}