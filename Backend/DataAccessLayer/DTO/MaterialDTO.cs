using DataAccessLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class MaterialDTO
    {
        public int Id { get; set; }
        public DateTime UploadDate { get; set; }
        public string? Topic { get; set; }
        public string? Summery { get; set; }
        public string? Content { get; set; }

        public static MaterialDTO FromMaterial(Material material)
        {
            return new MaterialDTO
            {
                Id = material.MaterialId,
                UploadDate = material.UploadDate,
                Topic = material.Topic,
                Content = material.Content,
                Summery = material.Summery,
            };
        }
    }
}
