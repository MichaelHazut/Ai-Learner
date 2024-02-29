using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.models
{
    public class Material
    {
        public int MaterialId { get; set; }
        public string? UserId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime UploadDate { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Question>? Questions { get; set; }

    }
}
