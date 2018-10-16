using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainTraining.Models;
using System.Web.Security;
using System.Data.Entity.Validation;

namespace BrainTraining.Controllers
{
    public class AccountController : Controller
    {
        public static string Name = "";
        public static string LastName = "";
        public static string Email = "";

        [HttpGet]
        public ActionResult Registration() { if (User.Identity.IsAuthenticated) { return RedirectToAction("Index", "Home"); } else { return View(); } }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                var EmailExists = IsEmailExist(user.Email);
                if (EmailExists) { ModelState.AddModelError("EmailExists", "User with this email is already registered"); return View(user); }

                GameSave sv = new GameSave();
                sv.UserId = user.Email;
                sv.LeftOrRightRound = 1;
                sv.LeftOrRightPoints = 0;
                sv.MemoryPowerPoints = 0;
                sv.MemoryPowerRound = 1;
                sv.RememberFacesPoints = 0;
                sv.RememberFacesRound = 1;
                sv.MultiTaskingPoints = 0;
                sv.MultiTaskingRound = 1;

                user.PasswordHash = PasswordHashing.Hash(user.PasswordHash);
                user.ConfirmPassword = PasswordHashing.Hash(user.ConfirmPassword);

                using (BrainTrainingEntities BTE = new BrainTrainingEntities())
                {
                    BTE.Users.Add(user);
                    BTE.GameSaves.Add(sv);

                    BTE.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }

            }
            else { ViewBag.ErrMsg = "Error while processing your request"; return View(); }
        }

        [NonAction]
        public Boolean IsEmailExist(string EmailId)
        {
            using (BrainTrainingEntities BTE = new BrainTrainingEntities())
            {
                var mail = BTE.Users.Where(userEmail => userEmail.Email == EmailId).FirstOrDefault();
                return mail != null;
            }
        }

        [HttpGet]
        public ActionResult Login() { if (User.Identity.IsAuthenticated) { return RedirectToAction("Index", "Home"); } else { return View(); } }

        [HttpPost]
        public ActionResult Login(Login login, string ReturnUrl)
        {
            string Message = "";
            if (ModelState.IsValid)
            {
                using (BrainTrainingEntities bte = new BrainTrainingEntities())
                {
                    var User = bte.Users.Where(email => email.Email == login.Email).FirstOrDefault();
                    if (User != null)
                    {
                        if (string.Compare(PasswordHashing.Hash(login.Password), User.PasswordHash) == 0)
                        {
                            int TimeOut = 525600;
                            var ticket = new FormsAuthenticationTicket(login.Email, true, TimeOut);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(TimeOut);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);

                            Message = "You have successfully signed in";
                            Name = User.FirstName;
                            if (Url.IsLocalUrl(ReturnUrl)) { return Redirect(ReturnUrl); }
                            else { return RedirectToAction("Index", "Home"); }
                        }
                        else { Message = "Invalid credential provided"; }
                    }
                    else { Message = "Invalid credential provided"; }
                }
            }
            else { return View(); }
            ViewBag.Message = Message;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout() { FormsAuthentication.SignOut(); return RedirectToAction("Index", "Home"); }

        [HttpGet]
        [Authorize]
        public ActionResult MyProfile()
        {
            using (BrainTrainingEntities bte = new BrainTrainingEntities())
            {
                string UserEmail = HttpContext.User.Identity.Name;
                var UserCredentials = bte.Users.Where(a => a.Email == UserEmail).FirstOrDefault();
                if (UserCredentials != null) { Name = UserCredentials.FirstName; LastName = UserCredentials.LastName; Email = UserCredentials.Email; }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(MyProfileModel MPM)
        {
            string pwd = MPM.Password;
            string NewPWD = MPM.NewPassword;
            string mail = MPM.Email;
            string ConfirmPass = MPM.ConfirmPassword;

            if (mail != null) { var EmailExists = IsEmailExist(mail); if (EmailExists) { ModelState.AddModelError("EmailExists", "User with this email is already registered"); return View(MPM); } }

            if (pwd != null)
            {
                string UserCurrentPWD = PasswordHashing.Hash(pwd);
                try
                {
                    using (BrainTrainingEntities bte = new BrainTrainingEntities())
                    {
                        var User = bte.Users.SingleOrDefault(email => email.Email == Email);
                        if (string.Compare(UserCurrentPWD, User.PasswordHash) == 0)
                        {
                            if (NewPWD != null && ConfirmPass != null)
                            {
                                if (string.Compare(PasswordHashing.Hash(NewPWD), PasswordHashing.Hash(ConfirmPass)) == 0)
                                {
                                    User.PasswordHash = PasswordHashing.Hash(NewPWD);
                                    User.ConfirmPassword = PasswordHashing.Hash(ConfirmPass);

                                    bte.GetValidationErrors();
                                    bte.SaveChanges();

                                    ViewBag.PwdMSG = "Password was successfully changed";
                                }
                                else { ModelState.AddModelError("PasswordMisMatch", "Current password doesn't match"); return View(MPM); }
                            }
                            else { ModelState.AddModelError("NoNewPWD", "Please set new password to change"); return View(MPM); }
                        }
                        else { ModelState.AddModelError("PasswordMisMatch", "Current password doesn't match"); return View(MPM); }
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        ViewBag.PwdMSG1 = ("Entity of type \"{0}\" in state \"{1}\" has the following validation errors: " + eve.Entry.Entity.GetType().Name + " " + eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors) { ViewBag.PwdMSG = ("- Property: \"{0}\", Error: \"{1}\"" + ", " + ve.PropertyName + " " + ve.ErrorMessage); }
                    }
                }
            }
            return View();
        }
    }
}