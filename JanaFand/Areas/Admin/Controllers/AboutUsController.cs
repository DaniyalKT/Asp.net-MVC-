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
    public class AboutUsController : Controller
    {
        private JanaFandEntities db = new JanaFandEntities();

        // GET: Admin/AboutUs
        public ActionResult Index()
        {
            return View(db.AboutUs.ToList());
        }

        // GET: Admin/AboutUs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AboutUs aboutUs = db.AboutUs.Find(id);
            if (aboutUs == null)
            {
                return HttpNotFound();
            }
            return View(aboutUs);
        }

        // GET: Admin/AboutUs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/AboutUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AboutID,Title,Text")] AboutUs aboutUs)
        {
            if (ModelState.IsValid)
            {
      
                db.AboutUs.Add(aboutUs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aboutUs);
        }

        // GET: Admin/AboutUs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AboutUs aboutUs = db.AboutUs.Find(id);
            if (aboutUs == null)
            {
                return HttpNotFound();
            }
            return View(aboutUs);
        }

        // POST: Admin/AboutUs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AboutID,Title,Text")] AboutUs aboutUs)
        {
            if (ModelState.IsValid)
            {
         
                db.Entry(aboutUs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aboutUs);
        }

        // GET: Admin/AboutUs/Delete/5
        public void Delete(int id)
        {
            var about = db.AboutUs.Find(id);
            db.AboutUs.Remove(about);
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
