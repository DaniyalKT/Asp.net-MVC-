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
    public class SlidersController : Controller
    {
        private JanaFandEntities db = new JanaFandEntities();

        // GET: Admin/Sliders
        public ActionResult Index()
        { 
            return View(db.Slider.ToList());
        }

        // GET: Admin/Sliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SliderID,Title,Url,ShortDescription,ImageName,EndDate,SratrDate,IsActive")] Slider slider, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if(imgUp == null)
                {
                    ModelState.AddModelError("ImageName", "لطفا تصویر را وارد کنید ");
                    return View();
                }
                slider.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                imgUp.SaveAs(Server.MapPath("/Images/Sliders/" + slider.ImageName));

                db.Slider.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        // GET: Admin/Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SliderID,Title,Url,ShortDescription,ImageName,EndDate,SratrDate,IsActive")] Slider slider, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if(imgUp != null)
                {
                    System.IO.File.Delete(Server.MapPath("/Images/Sliders/" + slider.ImageName));
                    slider.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/Sliders/" + slider.ImageName));
                }
                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        public ActionResult SlidersGallery(int id)
        {
            ViewBag.SliderImages = db.Slider_Galleries.Where(s => s.SliderID == id).ToList();
            return View(new Slider_Galleries() {
                SliderID = id
            });
        }

        [HttpPost]
        public ActionResult SlidersGallery(Slider_Galleries sliderGallery, HttpPostedFileBase galleryImage)
        {
            if(ModelState.IsValid)
            {
        
                if(galleryImage != null && galleryImage.IsImage())
                {
                    
                    sliderGallery.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(galleryImage.FileName);
                    galleryImage.SaveAs(Server.MapPath("/Images/Sliders/" + sliderGallery.ImageName));
                    db.Slider_Galleries.Add(sliderGallery);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("SlidersGallery", new {id = sliderGallery.SliderID });
                 
        }

        public ActionResult DeleteGallery(int id)
        {
            var gallery = db.Slider_Galleries.Find(id);

            System.IO.File.Delete(Server.MapPath("/Images/Sliders/" + gallery.ImageName));
            db.Slider_Galleries.Remove(gallery);
            db.SaveChanges();

            return RedirectToAction("SlidersGallery", new { id = gallery.SliderID });

        }
        // GET: Admin/Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Slider.Find(id);
            db.Slider.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
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
