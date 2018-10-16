using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BrainTraining.Models
{
    public class PasswordHashing
    {
        public static string Hash(string value)
        {
            var bytes = new UTF8Encoding().GetBytes(value);
            byte[] hashBytes;
            using (var Sha512Alg = new System.Security.Cryptography.SHA512Managed())
            { hashBytes = Sha512Alg.ComputeHash(bytes); }
            string Hash = Convert.ToBase64String(hashBytes);
            return Hash;
        }
    }
}