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
    public class OurChoicesController : Controller
    {
        private JanaFandEntities db = new JanaFandEntities();

        // GET: Admin/OurChoices
        public ActionResult Index()
        {
            return View(db.OurChoice.ToList());
        }

        // GET: Admin/OurChoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OurChoice ourChoice = db.OurChoice.Find(id);
            if (ourChoice == null)
            {
                return HttpNotFound();
            }
            return View(ourChoice);
        }

        // GET: Admin/OurChoices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/OurChoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChoiceID,Title,ShortDescription,ImageName")] OurChoice ourChoice, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if(imgUp != null && imgUp.IsImage())
                {
                    ourChoice.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/LogoChoice/" + ourChoice.ImageName));
                }
                db.OurChoice.Add(ourChoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ourChoice);
        }

        // GET: Admin/OurChoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OurChoice ourChoice = db.OurChoice.Find(id);
            if (ourChoice == null)
            {
                return HttpNotFound();
            }
            return View(ourChoice);
        }

        // POST: Admin/OurChoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChoiceID,Title,ShortDescription,ImageName")] OurChoice ourChoice, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if(imgUp != null && imgUp.IsImage())
                {
                    if(ourChoice.ImageName != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/LogoChoice/" + ourChoice.ImageName));
                    }
                    ourChoice.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/LogoChoice/" + ourChoice.ImageName));
                }
                db.Entry(ourChoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ourChoice);
        }

        // GET: Admin/OurChoices/Delete/5
        public void Delete(int id)
        {
            var choice = db.OurChoice.Find(id);
            if (choice.ImageName != null)
            {
                System.IO.File.Delete(Server.MapPath("/Images/LogoChoice/" + choice.ImageName));
            }
            db.OurChoice.Remove(choice);
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
