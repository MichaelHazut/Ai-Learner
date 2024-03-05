using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DataAccessLayer.models
{
    public class User
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? HashedPassword { get; set; }
    }
    //public class User(string email, string password)
    //{
    //    public string? Id { get; }
    //    public string Email { get; } = email;
    //    public string NormalizedEmail { get; } = email.ToUpperInvariant();
    //    public string? PhoneNumber { get; }
    //    public string HashedPassword
    //    {
    //        get { return HashedPassword; }
    //        set
    //        {
    //            using (SHA256 sha256Hash = SHA256.Create())
    //            {
    //                // Compute the hash of the password string converted to a byte array.
    //                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

    //                // Convert the byte array to a string representation.
    //                StringBuilder builder = new StringBuilder();
    //                for (int i = 0; i < bytes.Length; i++)
    //                {
    //                    builder.Append(bytes[i].ToString("x2"));
    //                }
    //                HashedPassword = builder.ToString();
    //            }
    //        }
    //    }
    //}
}
