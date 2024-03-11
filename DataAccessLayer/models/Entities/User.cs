using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DataAccessLayer.Models.Entities
{
    public class User
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? HashedPassword { get; set; }
        public DateTime CreateDate { get; set; }

        public static implicit operator User(bool v)
        {
            throw new NotImplementedException();
        }
    }

}
