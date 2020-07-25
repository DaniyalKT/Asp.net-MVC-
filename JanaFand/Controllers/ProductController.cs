using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Newtonsoft.Json;

namespace JanaFand.Controllers
{
    public class ProductController : Controller
    {
        JanaFandEntities db = new JanaFandEntities();
        // GET: Product
        

        public ActionResult ShowGroups()
        {
            return PartialView(db.Product_Groups.ToList());
        }

        public ActionResult LastProduct()
        {
            return PartialView(db.Products.OrderByDescending(p => p.CreateDate).Take(4));
        }

        [Route("ProductView/{id}")]
        public ActionResult ProductView(int id)
        {
            var product = db.Products.Find(id);
           
            ViewBag.ProductFeatures = product.Product_Features.DistinctBy(f => f.FeatureID).Select(f => new ProductFeaturesViewModal()
            {
                FeatureTitle = f.Features.FeatureTitle,
                Value = db.Product_Features.Where(fe => fe.FeatureID == f.FeatureID && product.ProductID == fe.ProductID).Select(fe => fe.Value).ToList()
            }).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        public ActionResult OrderProduct(  )
        {
         

            return PartialView();
        }

        public ActionResult ShowComments(int id)
        {

            return PartialView(db.Product_Comments.Where(c => c.ProductID == id));

        }

        public ActionResult CreateComment(int id)
        {

            return PartialView(new Product_Comments()
            {
                ProductID = id,

            });

        }
        [HttpPost]
        public ActionResult CreateComment(Product_Comments productComment)
        {

            //Start CreateComment code 
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {

                    productComment.CreateDate = DateTime.Now;
                    productComment.Name = User.Identity.Name;
                    db.Product_Comments.Add(productComment);
                    db.SaveChanges();
                }


                return PartialView("ShowComments", db.Product_Comments.Where(c => c.ProductID == productComment.ProductID));
            }
            return PartialView(productComment);
        }

 
        [Route("Archive")]
        public ActionResult ArchiveProduct(int pageId = 1, int price_first = 0, int price_second = 0, List<int> selectedGroup = null)
        {
            ViewBag.GroupsFilter = db.Product_Groups.ToList();
            ViewBag.price_first = price_first;
            ViewBag.price_second = price_second;
            ViewBag.selected = selectedGroup;
            ViewBag.pageId = pageId;
            List<Products> list = new List<Products>();

            if (selectedGroup != null && selectedGroup.Any())
            {
                foreach (int group in selectedGroup)
                {
                    list.AddRange(db.Product_Selected_Groups.Where(g => g.GroupID == group).Select(g => g.Products).ToList());
                }
               list=list.Distinct().ToList();
            }
            else
            {
                list.AddRange(db.Products.ToList());
            }

            if (price_first > 0)
            {
                list = list.Where(p => p.Price >= price_first).ToList();
            }
            if (price_second > 0)
            {
                list = list.Where(p => p.Price <= price_second).ToList();
            }


            //Paging Code 

            int take =9;
            int skip = (pageId - 1) * take;
            ViewBag.pageCount = list.Count() / take;
            return PartialView(list.OrderByDescending(g=>g.CreateDate).Skip(skip).Take(take).ToList());
        }

  

        public void DeleteComment(int id)
        {
            if (ModelState.IsValid)
            {

                var comment = db.Product_Comments.Find(id);
                if (db.Product_Comments.Any())
                {
                    foreach (var subComment in db.Product_Comments.Where(c => c.PersonID == id))
                    {
                        db.Product_Comments.Remove(subComment);
                    }
                }
                db.Product_Comments.Remove(comment);
                db.SaveChanges();

            }
        }




    }

}