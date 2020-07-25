using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace JanaFand.Controllers
{
    public class LikeProductController : Controller
    {
        JanaFandEntities db = new JanaFandEntities();
        // GET: LikeProduct
        public ActionResult Index()
        {
            List<LikeProductList> list = new List<LikeProductList>();
            if (Session["LikeProduct"] != null)
            {
                list = Session["LikeProduct"] as List<LikeProductList>;
            }

            return View(list);
        }

        public ActionResult AddProductInList(int id)
        {
            List<LikeProductList> list = new List<LikeProductList>();
            if(Session["LikeProduct"]!= null)
            {
                list = Session["LikeProduct"] as List<LikeProductList>;
            }
            if(!list.Any(p=>p.ProductID == id))
            {
                var product = db.Products.Where(p => p.ProductID == id).Select(p => new { p.ProductTitle, p.ImageName, p.Price }).Single();
                list.Add(new LikeProductList() {

                    ProductID = id,
                    ImageName = product.ImageName,
                    Price = product.Price,
                    Title = product.ProductTitle
                });
            }
            Session["LikeProduct"] = list;
            return PartialView("ListLikeProduct",list);
        }

        public ActionResult ListLikeProduct()
        {
            List<LikeProductList> list = new List<LikeProductList>();
            if (Session["LikeProduct"] != null)
            {
                list = Session["LikeProduct"] as List<LikeProductList>;
            }
            return PartialView(list);
        }

        public ActionResult DeleteFromLikeProduct(int id)
        {
            List<LikeProductList> list = new List<LikeProductList>();
            if (Session["LikeProduct"] != null)
            {
                list = Session["LikeProduct"] as List<LikeProductList>;

                
                int index = list.FindIndex(p => p.ProductID == id);
                list.RemoveAt(index);
                Session["LikeProduct"] = list;
            }
            return PartialView("ListLikeProduct", list);
        }
    }
}