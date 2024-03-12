using DataAccessLayer.dbContext;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;


namespace DataAccessLayer.Repositories
{
    public class MaterialRepo(AiLearnerDbContext context) : RepositoryBase<Material>(context), IMaterialRepo
    {

        public async Task<List<Material>> GetMaterials(string userId)
        {
            //Get the materials by the user id
            return await _context.Set<Material>().Where(material => material.UserId == userId).ToListAsync();
        }


        public async Task<Material> CreateMaterial(string userId, string topic, string content, string summery)
        {
            Material material = new()
            {
                UserId = userId,
                Topic = topic,
                Content = content,
                Summery = summery,
                UploadDate = DateTime.Now
            };

            await _context.AddAsync(material);

            return material;
        }
    }
}
