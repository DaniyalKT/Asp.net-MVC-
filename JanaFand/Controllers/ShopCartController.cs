using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace JanaFand.Controllers
{
    public class ShopCartController : Controller
    {
        JanaFandEntities db = new JanaFandEntities();
        // GET: ShopCart
        public ActionResult ShowCart()
        {
            List<ShopCartItemViewModel> list = new List<ShopCartItemViewModel>();

            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> listShop = (List<ShopCartItem>)Session["ShopCart"];

                foreach (var item in listShop)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new
                    {
                        p.ImageName,
                        p.Price,
                        p.ProductTitle

                    }).Single();

                    list.Add(new ShopCartItemViewModel()
                    {
                        Count = item.Count,
                        ProductID = item.ProductID,
                        Title = product.ProductTitle,
                        ImageName = product.ImageName,
                        Sum = item.Count * product.Price



                    });
                }
            }

            return PartialView(list);
        }

        public ActionResult Index()
        {
            ViewBag.AccountName = "فاکتور سبد خرید";
            return View();
        }

        List<ShowOrderViewModel> getListFactor()
        {
            List<DataLayer.ShowOrderViewModel> list = new List<DataLayer.ShowOrderViewModel>();

            if (Session["ShopCart"] != null)
            {
                List<DataLayer.ShopCartItem> listShop = Session["ShopCart"] as List<DataLayer.ShopCartItem>;

                foreach (var item in listShop)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new
                    {
                        p.ImageName,
                        p.Price,
                        p.ProductTitle


                    }).Single();
                    list.Add(new DataLayer.ShowOrderViewModel()
                    {
                        Count = item.Count,
                        ProductID = item.ProductID,
                        ImageName = product.ImageName,
                        Title = product.ProductTitle,
                        Price = product.Price,
                        Sum = item.Count * product.Price,
                    });
                }

            }
            return list;
        }

        public ActionResult Factor()
        {
            return PartialView(getListFactor());
        }


        public ActionResult CommandFactor(int id, int count)
        {
            List<ShopCartItem> listShop = Session["ShopCart"] as List<ShopCartItem>;
            int index = listShop.FindIndex(p => p.ProductID == id);
            if (count == 0)
            {
                listShop.RemoveAt(index);
            }
            else
            {
                listShop[index].Count = count;
            }
            Session["ShopCart"] = listShop;

            return PartialView("Factor", getListFactor());
        }

        [Authorize]
        public ActionResult Payment()
        {
            int userId = db.Users.Single(u => u.UserName == User.Identity.Name).UserID;
         
            DataLayer.Factors factor = new DataLayer.Factors()
            {
                UserID = userId,
                Date = DateTime.Now,
                IsFinally = false
            };
            db.Factors.Add(factor);

            var listDetail = getListFactor();

            foreach (var item in listDetail)
            {
                var user = db.Users.Where(u => u.UserName == User.Identity.Name && u.UserID == factor.UserID).Select(u => new { u.Address, u.PhoneNumber, u.PostalCode }).Single();
                db.FactorDetails.Add(new FactorDetails()
                {
                    Count = item.Count,
                    FactorID = factor.FactorID,
                    Price = item.Price,
                    ProductID = item.ProductID


                });


            }
            db.SaveChanges();

            //TODO : Online Payment

            return null;
        }




    }
}