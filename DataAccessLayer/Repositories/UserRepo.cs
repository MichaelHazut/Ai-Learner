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
        private async Task<bool> CheckIfExist(string email)
        {
            User? user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);

            if (user is null) return false;

            return true;
        }

        public async Task<User> NewUser(string email, string password)
        {
            if (await CheckIfExist(email) is true) throw new InvalidOperationException("User already exists");

            User user = new()
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
            if (await CheckIfExist(email) is false) throw new InvalidOperationException($"{email} is not a registered user");

            User? user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);

            return PasswordService.VerifyPassword(password, user!.HashedPassword!);
        }

        
    }

}
