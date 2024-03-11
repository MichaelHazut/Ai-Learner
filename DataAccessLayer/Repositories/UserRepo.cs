using DataAccessLayer.dbContext;
using DataAccessLayer.Services;
using DataAccessLayer.Models.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;


namespace DataAccessLayer.Repositories
{
    public class UserRepo(AiLearnerDbContext context) : RepositoryBase<User>(context), IUserRepo
    {
        private async Task<User?> CheckIfExist(string email)
        {
            User? user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<User> NewUser(string email, string password)
        {
            User? user = await CheckIfExist(email);
            if (user is not null) return user;

            user = new()
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

        public async Task<User?> LogIn(string email, string password)
        {
            User? user = await CheckIfExist(email);
            if(user is null) return false;
            
            bool isVerified = PasswordService.VerifyPassword(password, user!.HashedPassword!);
            if(isVerified is false) return null;

            return user;
        }

        
    }

}
