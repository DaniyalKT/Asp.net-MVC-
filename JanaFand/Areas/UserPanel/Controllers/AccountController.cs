using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using System.Web.Security;

namespace JanaFand.Areas.UserPanel.Controllers
{
    public class AccountController : Controller
    {
        JanaFandEntities db = new JanaFandEntities();
        // GET: UserPanel/Account
        public ActionResult ChangePassword()
        {
            ViewBag.AccountName = "تغییر گذرواژه";
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (ModelState.IsValid)
            {
                ViewBag.AccountName = "تغییر گذرواژه";
                var user = db.Users.Single(u => u.UserName == User.Identity.Name);
                string hashOldPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(change.OldPassword, "MD5");
                if(user.Password == hashOldPassword)
                {
                    string hashNewPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(change.Password, "MD5");
                    user.Password = hashNewPassword;
                    db.SaveChanges();

                    ViewBag.Success = true;
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "گذرواژه فعلی صحیح نمیباشد .");
                }
            }
            return View();
        }



    }
}