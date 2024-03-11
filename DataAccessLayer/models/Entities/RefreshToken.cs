using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string? UserId { get; set; } 
        public string? Token { get; set; } 
        public DateTime Expiration { get; set; }
        public DateTime Created { get; set; }
        public bool Revoked { get; set; }
        public int? ReplacedByTokenId { get; set; } 
        public virtual User? User { get; set; } 
        public virtual RefreshToken? ReplacedByToken { get; set; }
    }

}
