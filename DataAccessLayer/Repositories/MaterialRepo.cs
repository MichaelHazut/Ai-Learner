using DataAccessLayer.dbContext;
using DataAccessLayer.models.Entities;
using System.Runtime.InteropServices;


namespace DataAccessLayer.Repositories
{
    public class MaterialRepo(AiLearnerDbContext context) : RepositoryBase<Material>(context)
    {
        public async Task<Material> CreateMaterial(string userId, string topic, string content, string summery)
        {
            Material material = new()
            {
                UserId = userId,
                Topic = topic,
                Content = content,
                summery = summery,
                UploadDate = DateTime.Now
            };

            await this.CreateAsync(material);

            return material;
        }
    }
}
