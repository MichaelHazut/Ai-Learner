using DataAccessLayer.dbContext;
using DataAccessLayer.models;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Repositories
{
    public class UserRepo(AiLearnerDbContext context) : RepositoryBase<User>(context)
    {
        
    }
}
