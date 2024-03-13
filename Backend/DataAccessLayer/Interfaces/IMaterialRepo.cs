using DataAccessLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IMaterialRepo : IEntityDataAccess<Material>
    {
        Task<List<Material>> GetMaterials(string userId);
        Task<Material> CreateMaterial(string userId, string topic, string content, string summery);
    }
}
