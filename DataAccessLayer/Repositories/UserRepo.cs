using DataAccessLayer.dbContext;
using DataAccessLayer.Services;
using DataAccessLayer.models.Entities;


namespace DataAccessLayer.Repositories
{
    public class UserRepo(AiLearnerDbContext context) : RepositoryBase<User>(context)
    {
        public async Task<User> NewUser(string email, string password)
        {
            User user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                HashedPassword = PasswordService.HashPassword(password),
                CreateDate = DateTime.Now,
            };

            await _context.AddAsync(user);
            return user;
        }

    }

}
