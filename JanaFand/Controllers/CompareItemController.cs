using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JanaFand.Controllers
{
    public class CompareItemController : Controller
    {
        DataLayer.JanaFandEntities db = new DataLayer.JanaFandEntities();
        // GET: CompareItem
        public ActionResult Index()
        {
            ViewBag.AccountName = "لیست مقایسه ";
            List<DataLayer.CompareItem> list = new List<DataLayer.CompareItem>();
            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<DataLayer.CompareItem>;
            }
            List<DataLayer.Features> features = new List<DataLayer.Features>();
            List<DataLayer.Product_Features> productFeatures = new List<DataLayer.Product_Features>();
            foreach(var item in list)
            {
                features.AddRange(db.Product_Features.Where(f => f.ProductID == item.ProductID).Select(f => f.Features).ToList());
                productFeatures.AddRange(db.Product_Features.Where(fp => fp.ProductID == item.ProductID).ToList());
            }
            ViewBag.featurs = features.Distinct().ToList();
            ViewBag.productFeatur = productFeatures;

            return View(list);
        }

        public ActionResult AddToCompare(int id)
        {
            List<DataLayer.CompareItem> list = new List<DataLayer.CompareItem>();
            if(Session["Compare"]!= null)
            {
                list = Session["Compare"] as List<DataLayer.CompareItem>;
            }
            if(!list.Any(p=>p.ProductID == id))
            {
                var product = db.Products.Where(p => p.ProductID == id).Select(p => new { p.ProductTitle, p.ImageName, p.Price }).Single();
                list.Add(new DataLayer.CompareItem()
                {
                    ProductID =id,
                    ImageName = product.ImageName,
                    Title = product.ProductTitle,
                    Price = product.Price
                });
            }
            Session["Compare"] = list;

            return PartialView("ListCompare", list);
        }

        public ActionResult ListCompare()
        {
            List<DataLayer.CompareItem> list = new List<DataLayer.CompareItem>();
            if (Session["Compare"] != null)
            {
                list= Session["Compare"] as List<DataLayer.CompareItem>;
            }

            return PartialView(list);
        }

        public ActionResult DeleteFromCompare(int id)
        {
            List<DataLayer.CompareItem> list = new List<DataLayer.CompareItem>();
            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<DataLayer.CompareItem>;
                int index = list.FindIndex(p => p.ProductID == id);
                list.RemoveAt(index);
                Session["Compare"] = list;
            }
            return PartialView("ListCompare", list);

        }


    }
}