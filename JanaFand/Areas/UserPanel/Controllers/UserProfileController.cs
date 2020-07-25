using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace JanaFand.Areas.UserPanel.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private JanaFandEntities db = new JanaFandEntities();

        // GET: UserPanel/UserProfile
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Roles);
            return View(users.ToList());
        }





        // GET: UserPanel/UserProfile/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleTitle", users.RoleID);
            return View(users);
        }

        // POST: UserPanel/UserProfile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,RoleID,UserName,Email,Password,ActiveCode,IsActive,RegisterDate,Address,PhoneNumber,PostalCode")] Users users)
        {
            if (ModelState.IsValid)
            {
           
               
                    db.Entry(users).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                

            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleTitle", users.RoleID);
            return View(users);
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
