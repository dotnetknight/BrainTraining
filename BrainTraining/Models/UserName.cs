using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTraining.Models
{
    public class UserName
    {
        public static string Name(string Email)
        {
            string Name = "";
            using (BrainTrainingEntities bte = new BrainTrainingEntities())
            {
                string email = Email;
                var v = bte.Users.Where(a => a.Email == email).FirstOrDefault();
                if (v != null) { Name = v.FirstName; }
                return Name;
            }
        }
        public static string Email(string Email)
        {
            string mail = "";
            using (BrainTrainingEntities bte = new BrainTrainingEntities())
            {
                string email = mail;
                var v = bte.Users.Where(a => a.Email == email).FirstOrDefault();
                if (v != null) { mail = v.Email; }
                return mail;
            }
        }
    }
}