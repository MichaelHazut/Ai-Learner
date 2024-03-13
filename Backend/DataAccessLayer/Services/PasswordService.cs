using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Security.Cryptography;
using System.Drawing;
namespace DataAccessLayer.Services
{
    internal static class PasswordService
    {
        public static string HashPassword(string password)
        {
           return BCrypt.Net.BCrypt.HashPassword(password);
        }
        
        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
