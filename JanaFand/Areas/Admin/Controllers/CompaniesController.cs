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
    public class CompaniesController : Controller
    {
        private JanaFandEntities db = new JanaFandEntities();

        // GET: Admin/Companies
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        // GET: Admin/Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Companies companies = db.Companies.Find(id);
            if (companies == null)
            {
                return HttpNotFound();
            }
            return View(companies);
        }

        // GET: Admin/Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyID,Title,ImageName")] Companies companies, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if (imgUp != null && imgUp.IsImage())
                {
                    companies.ImageName = Guid.NewGuid() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/CompaniesLogo/" + companies.ImageName));
                }
                db.Companies.Add(companies);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companies);
        }

        // GET: Admin/Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Companies companies = db.Companies.Find(id);
            if (companies == null)
            {
                return HttpNotFound();
            }
            return View(companies);
        }

        // POST: Admin/Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyID,Title,ImageName")] Companies companies, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if (imgUp != null && imgUp.IsImage())
                {
                    if(companies.ImageName != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/CompaniesLogo/" + companies.ImageName));

                    }
                    companies.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/CompaniesLogo/" + companies.ImageName));
                }
                db.Entry(companies).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companies);
        }

        // GET: Admin/Companies/Delete/5
        public void Delete(int id)
        {
            var company = db.Companies.Find(id);
            System.IO.File.Delete(Server.MapPath("/Images/CompaniesLogo/" + company.ImageName));
            db.Companies.Remove(company);
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
