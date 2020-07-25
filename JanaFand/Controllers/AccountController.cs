using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using System.Web.Security;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using JanaFand;
using System.Configuration;
using System.Data.Entity;

namespace JanaFand.Controllers
{
    public class AccountController : Controller
    {
        JanaFandEntities db = new JanaFandEntities();
        // GET: Account     



        [Route("Register")]
        public ActionResult Register()
        {
            ViewBag.AccountText = "با ثبت نام در سایت ، از مزایای کاربران سایت بهره مند شوید";
            ViewBag.AccountName = "ثبت نام ";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public ActionResult Register(RegisterViewModel register, FormCollection form)
        {

            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = ConfigurationManager.AppSettings["recaptchaPrivateKey"]; // change this
            string gRecaptchaResponse = form["g-recaptcha-response"];

            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            if (!captChaesponse.Success)
            {
                ViewBag.Message = "لطفا هویت خود را تایید کنید ";
                return View();
            }

            //Start Register Code
            if (ModelState.IsValid)
            {
                if (!db.Users.Any(u => u.Email == register.Email.Trim().ToLower()))
                {

                    if (!db.Users.Any(u => u.UserName == register.UserName))
                    {
                        Users user = new Users()
                        {
                            Email = register.Email.Trim().ToLower(),
                            Password = FormsAuthentication.HashPasswordForStoringInConfigFile(register.Password, "MD5"),
                            ActiveCode = Guid.NewGuid().ToString(),
                            IsActive = false,
                            UserName = register.UserName,
                            Address = register.Address,
                            PhoneNumber = register.PhoneNumber,
                            RoleID = 1,
                            RegisterDate = DateTime.Now
                        };
                        db.Users.Add(user);
                        db.SaveChanges();

                        //Send Active Email

                        string body = PartialToStringClass.RenderPartialView("ManageEmail", "ActivationEmail", user);
                        SendEmail.Send(user.Email, "فعالسازی حساب کاربری", body);

                        //End Send Active Email


                        return View("SuccessRegister", user);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "نام کاربری وارد شده قبلا توسط شخص دیگری ثبت شده است !");
                    }
                }
                else
                {

                    ModelState.AddModelError("Email", "ایمیل وارد شده قبلا توسط شخص دیگری ثبت شده است! ");

                }
            }
            ViewBag.AccountText = "با ثبت نام در سایت ، از مزایای کاربران سایت بهره مند شوید";
            ViewBag.AccountName = "ثبت نام ";
            return View(register);
        }

        [Route("LogIn")]
        public ActionResult Login()
        {
            ViewBag.AccountText = "با ورود به حساب کاربری خود ، از مزایای کاربران سایت بهره مند شوید";
            ViewBag.AccountName = "ورود به سایت";
            return View();
        }

        [HttpPost]
        [Route("LogIn")]
        public ActionResult Login(LoginViewModel login, FormCollection form, string ReturnUrl = "/")
        {


            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = ConfigurationManager.AppSettings["recaptchaPrivateKey"]; // change this
            string gRecaptchaResponse = form["g-recaptcha-response"];

            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            if (!captChaesponse.Success)
            {
                ViewBag.Message = "لطفا هویت خود را تایید کنید ";
                return View();
            }


            //Start LogIn Code
            if (ModelState.IsValid)
            {
                string hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Password, "MD5");
                var user = db.Users.SingleOrDefault(u => u.Email == login.Email && u.Password == hashPassword);
                if (user != null)
                {
                    if (user.IsActive)
                    {

                        FormsAuthentication.SetAuthCookie(user.UserName, login.RememmberMe);
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "متاسفانه حساب کاربری شما فعان نشده است");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "کاربری با اطلاعات وارد شده یافت نشد!");
                }
            }
            ViewBag.AccountText = "با ورود به حساب کاربری خود ، از مزایای کاربران سایت بهره مند شوید";
            ViewBag.AccountName = "ورود به سایت";
            return View(login);
        }

        public ActionResult ActiveUser(string id)
        {
            var user = db.Users.SingleOrDefault(u => u.ActiveCode == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.IsActive = true;
            user.ActiveCode = Guid.NewGuid().ToString();
            db.SaveChanges();
            ViewBag.UserName = user.UserName;
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [Route("ForgotPassword")]
        public ActionResult ForgotPassword()
        {
            ViewBag.AccountText = "پس از وارد کردن ایمیل خود لینک تغییر گذرواژه به ایمیل شما ارسال می شود.";
            ViewBag.AccountName = "بازیابی گذرواژه";
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgot, FormCollection form)
        {
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = ConfigurationManager.AppSettings["recaptchaPrivateKey"]; // change this
            string gRecaptchaResponse = form["g-recaptcha-response"];

            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            if (!captChaesponse.Success)
            {
                ViewBag.Message = "لطفا هویت خود را تایید کنید ";
                return View();
            }

            //Start ForgetPasswords code
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Email == forgot.Email);
                if (user != null)
                {
                    if (user.IsActive)
                    {
                        //Send Email

                        string bodyEmail = PartialToStringClass.RenderPartialView("ManageEmail", "RecoveryPassword", user);
                        SendEmail.Send(user.Email, "بازیابی گذرواژه", bodyEmail);
                        //End Send Email
                        return View("SuccesForgotPassword", user);

                    }
                    else
                    {
                        ModelState.AddModelError("Email", "متاسفانه حساب کاربری شما فعان نشده است");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "کاربری با اطلاعات وارد شده یافت نشد !");
                }
            }
            ViewBag.AccountText = "پس از وارد کردن ایمیل خود لینک تغییر گذرواژه به ایمیل شما ارسال می شود.";
            ViewBag.AccountName = "بازیابی گذرواژه";
            return View();
        }


        public ActionResult RecoveryPassword(string id)
        {
            ViewBag.AccountName = "بازیابی گذرواژه";
            return View();
        }

        [HttpPost]
        public ActionResult RecoveryPassword(string id, RecoveryPasswordViewModel recovery, FormCollection form)
        {
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = ConfigurationManager.AppSettings["recaptchaPrivateKey"]; // change this
            string gRecaptchaResponse = form["g-recaptcha-response"];

            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            if (!captChaesponse.Success)
            {
                ViewBag.Message = "لطفا هویت خود را تایید کنید ";
                return View();
            }


            //Start Recovery Passwords code
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.ActiveCode == id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(recovery.Password, "MD5");
                user.ActiveCode = Guid.NewGuid().ToString();
                db.SaveChanges();

                return Redirect("/LogIn?recovery=true");

            }
            ViewBag.AccountName = "بازیابی گذرواژه";
            return View();
        }


    }
}

