using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace JanaFand.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private JanaFandEntities db = new JanaFandEntities();

        // GET: Admin/Products
        public ActionResult Index()
        {

            return View(db.Products.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.Groups = db.Product_Groups.ToList();
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductTitle,ShortDescription,Text,OldPrice,Price,ImageName,CreateDate")] Products products, List<int> SelectedGroup, HttpPostedFileBase productImage, string tags)
        {
            if (ModelState.IsValid)
            {
                if (SelectedGroup == null)
                {
                    ViewBag.ErrorSelectedGroup = true;
                    ViewBag.Groups = db.Product_Groups.ToList();
                    return View(products);
                }
                products.CreateDate = DateTime.Now;
                products.ImageName = "NoPhoto.png";

                if (productImage != null && productImage.IsImage())
                {

                    products.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(productImage.FileName);
                    productImage.SaveAs(Server.MapPath("/Images/ProductImages/" + products.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + products.ImageName),
                        Server.MapPath("/Images/ProductImages/Thumb/" + products.ImageName));
                }

                db.Products.Add(products);

                foreach (int selectedGroup in SelectedGroup)
                {
                    db.Product_Selected_Groups.Add(new Product_Selected_Groups()
                    {
                        ProductID = products.ProductID,
                        GroupID = selectedGroup
                    });
                }
                if (!string.IsNullOrEmpty(tags))
                {
                    string[] tag = tags.Split(',');
                    foreach (var t in tag)
                    {
                        db.Product_Tags.Add(new Product_Tags()
                        {
                            ProductID = products.ProductID,
                            Tags = t.Trim()
                        });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Groups = db.Product_Groups.ToList();
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.SelectedGroups = products.Product_Selected_Groups.ToList();
            ViewBag.Groups = db.Product_Groups.ToList();
            ViewBag.Tags = string.Join(",", products.Product_Tags.Select(t => t.Tags).ToList());
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductTitle,ShortDescription,Text,OldPrice,Price,ImageName,CreateDate")] Products products, List<int> SelectedGroup, HttpPostedFileBase productImage, string tags)
        {
            if (ModelState.IsValid)
            {
                if (productImage != null && productImage.IsImage())
                {
                    if (products.ImageName != "NoPhoto.png")
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/ProductImages/" + products.ImageName));
                        System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/" + products.ImageName));

                    }
                    products.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(productImage.FileName);
                    productImage.SaveAs(Server.MapPath("/Images/ProductImages/" + products.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + products.ImageName),
                        Server.MapPath("/Images/ProductImages/Thumb/" + products.ImageName));
                }

                db.Product_Tags.Where(t => t.ProductID == products.ProductID).ToList().ForEach(t => db.Product_Tags.Remove(t));
                if (!string.IsNullOrEmpty(tags))
                {
                    string[] tag = tags.Split(',');
                    foreach (var t in tag)
                    {
                        db.Product_Tags.Add(new Product_Tags()
                        {
                            ProductID = products.ProductID,
                            Tags = t.Trim()
                        });
                    }
                }

                db.Product_Selected_Groups.Where(g => g.ProductID == products.ProductID).ToList().ForEach(g => db.Product_Selected_Groups.Remove(g));
                if (SelectedGroup != null && SelectedGroup.Any())
                {
                    foreach (int selectedGroup in SelectedGroup)
                    {
                        db.Product_Selected_Groups.Add(new Product_Selected_Groups()
                        {
                            ProductID = products.ProductID,
                            GroupID = selectedGroup
                        });
                    }
                }

                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SelectedGroups = SelectedGroup;
            ViewBag.Groups = db.Product_Groups.ToList();
            ViewBag.Tags = tags;
            return View(products);
        }

        // GET: Admin/Products/Delete/5
        public void Delete(int id)
        {
            var products = db.Products.Find(id);
            if (products.ImageName != "NoPhoto.png")
            {

                System.IO.File.Delete(Server.MapPath("/Images/ProductImages/" + products.ImageName));
                System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/" + products.ImageName));
            }
    
            db.Product_Tags.Where(ta => ta.ProductID == products.ProductID).ToList().ForEach(ta => db.Product_Tags.Remove(ta));
            db.Product_Selected_Groups.Where(gr => gr.ProductID == products.ProductID).ToList().ForEach(gr => db.Product_Selected_Groups.Remove(gr));
            db.Product_Features.Where(f => f.ProductID == products.ProductID).ToList().ForEach(f => db.Product_Features.Remove(f));
            db.Product_Galleries.Where(g => g.ProductID == products.ProductID).ToList().ForEach(g => db.Product_Galleries.Remove(g));
            db.Product_Comments.Where(ca => ca.ProductID == products.ProductID).ToList().ForEach(ca => db.Product_Comments.Remove(ca));
            db.Product_Galleries.Where(ga => ga.ProductID == products.ProductID).ToList().ForEach(ga => {
                System.IO.File.Delete(Server.MapPath("/Images/ProductImages/" + ga.ImageName));
                System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/" + ga.ImageName));
            });

            db.Products.Remove(products);
            db.SaveChanges();

        }

        public ActionResult ArchiveProduct(int pageId = 1, string title = "", int minPrice = 0, int maxPrice = 0, List<int> selectedGroup = null)
        {
            ViewBag.Group = db.Product_Groups.ToList();

            ViewBag.ProductTitle = title;
            IQueryable<Products> list = db.Products;

            if (title != "")
            {
                list = list.Where(t => t.ProductTitle.Contains(title));
            }

            return View(list.ToList());
        }

        public ActionResult Gallery(int id)
        {
            ViewBag.Galleries = db.Product_Galleries.Where(g => g.ProductID == id).ToList();
            return View(new Product_Galleries()
            {

                ProductID = id
            });
        }

        [HttpPost]
        public ActionResult Gallery(Product_Galleries galleries, HttpPostedFileBase imgUpGalleries)
        {
            if (ModelState.IsValid)
            {
                if (imgUpGalleries != null && imgUpGalleries.IsImage())
                {
                    galleries.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUpGalleries.FileName);
                    imgUpGalleries.SaveAs(Server.MapPath("/Images/ProductImages/" + galleries.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + galleries.ImageName),
                        Server.MapPath("/Images/ProductImages/Thumb/" + galleries.ImageName));
                    db.Product_Galleries.Add(galleries);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("Gallery", new { id = galleries.ProductID });
        }

        public ActionResult DeleteGallery(int id)
        {
            var gallery = db.Product_Galleries.Find(id);

            System.IO.File.Delete(Server.MapPath("/Images/ProductImages/" + gallery.ImageName));
            System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/" + gallery.ImageName));


            db.Product_Galleries.Remove(gallery);
            db.SaveChanges();
            return RedirectToAction("Gallery", new { id = gallery.ProductID });
        }

        public ActionResult ProductFeature(int id)
        {
            ViewBag.Features = db.Product_Features.Where(f => f.ProductID == id).ToList();
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureTitle");
            return View(new Product_Features()
            {
                ProductID = id
            });
        }

        [HttpPost]
        public ActionResult ProductFeature(Product_Features freatures)
        {
            if (ModelState.IsValid)
            {
                db.Product_Features.Add(freatures);
                db.SaveChanges();
            }
            return RedirectToAction("ProductFeature", new { id = freatures.ProductID });
        }



        public void DeleteFeature(int id)
        {
            var feature = db.Product_Features.Find(id);
            db.Product_Features.Remove(feature);
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
