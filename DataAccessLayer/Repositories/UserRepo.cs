using DataAccessLayer.dbContext;
using DataAccessLayer.Services;
using DataAccessLayer.Models.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Repositories
{
    public class UserRepo(AiLearnerDbContext context) : RepositoryBase<User>(context), IUserRepo
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

        public async Task<bool> LogIn(string email, string password)
        {
            
            User? user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                return false;
            }
            return PasswordService.VerifyPassword(password, user.HashedPassword!);
        }

    }

}
