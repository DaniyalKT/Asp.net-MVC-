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
    public class SocialNetworksController : Controller
    {
        private JanaFandEntities db = new JanaFandEntities();

        // GET: Admin/SocialNetworks
        public ActionResult Index()
        {
            return View(db.SocialNetwork.ToList());
        }

        // GET: Admin/SocialNetworks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialNetwork socialNetwork = db.SocialNetwork.Find(id);
            if (socialNetwork == null)
            {
                return HttpNotFound();
            }
            return View(socialNetwork);
        }

        // GET: Admin/SocialNetworks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/SocialNetworks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NetwordID,Logo,Url,Title")] SocialNetwork socialNetwork, HttpPostedFileBase logoName)
        {
            if (ModelState.IsValid)
            {
                if (logoName == null)
                {
                    ModelState.AddModelError("Logo", "لطفا لوگو را وارد کنید");
                    return View();
                }
                socialNetwork.Logo = Guid.NewGuid().ToString() + Path.GetExtension(logoName.FileName);
                logoName.SaveAs(Server.MapPath("/Images/SocialNetworkLogo/" + socialNetwork.Logo));
                db.SocialNetwork.Add(socialNetwork);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(socialNetwork);
        }

        // GET: Admin/SocialNetworks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialNetwork socialNetwork = db.SocialNetwork.Find(id);
            if (socialNetwork == null)
            {
                return HttpNotFound();
            }
            return View(socialNetwork);
        }

        // POST: Admin/SocialNetworks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NetwordID,Logo,Url,Title")] SocialNetwork socialNetwork, HttpPostedFileBase logoName)
        {
            if (ModelState.IsValid)
            {

                System.IO.File.Delete(Server.MapPath("/Images/SocialNetworkLogo/" + socialNetwork.Logo));


                socialNetwork.Logo = Guid.NewGuid().ToString() + Path.GetExtension(logoName.FileName);
                logoName.SaveAs(Server.MapPath("/Images/SocialNetworkLogo/" + socialNetwork.Logo));

                db.Entry(socialNetwork).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(socialNetwork);
        }

        // GET: Admin/SocialNetworks/Delete/5
        public void Delete(int id)
        {
            var network = db.SocialNetwork.Find(id);
            System.IO.File.Delete(Server.MapPath("/Images/SocialNetworkLogo/" + network.Logo));
            db.SocialNetwork.Remove(network);
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
