using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainTraining.Models;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace BrainTraining.Controllers
{
    public class ContactController : Controller
    {
        public static string Name = "";
        [HttpGet]
        public ActionResult Index() { Name = UserName.Name(HttpContext.User.Identity.Name); return View(); }
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(string FirstName, string EmailAddress, string PhoneNumber, string Message)
        {
            string Result = "";
            string Name = FirstName;
            string Email = EmailAddress;
            string Phone = PhoneNumber;
            string Msg = Message;

            if (Name != "" && Email != "" && Phone != "" && Msg != "")
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("nikabika10@gmail.com"));
                message.From = new MailAddress("skott56@outlook.com");
                message.Subject = "BrainTraining Email";
                message.Body = string.Format(body, Name, Email, Msg);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "skott56@outlook.com",
                        Password = "dotnetknight983"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    Result = "Your message has been sent";
                    ViewBag.msg = Result;
                }
            }

            return View("Index");
        }
    }
}