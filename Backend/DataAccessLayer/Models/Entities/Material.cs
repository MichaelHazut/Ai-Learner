using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Entities
{
    public class Material
    {
        public int MaterialId { get; set; }
        public string? UserId { get; set; }
        public string? Topic { get; set; }
        public string? Content { get; set; }
        public string? Summery { get; set; }
        public DateTime UploadDate { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Question>? Questions { get; set; }

    }
}
