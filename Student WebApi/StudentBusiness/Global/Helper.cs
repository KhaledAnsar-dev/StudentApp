using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace StudentBusiness.Global
{
    public class Helper
    {
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash).ToLower();
        }
        public static bool CheckPassword(string entredPassword, string hashedPassword)
        {
            var currentPassword = HashPassword(entredPassword);

            return currentPassword == hashedPassword;
        }
    }
}
