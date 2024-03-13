using DataAccessLayer.Models.Entities;
using DataAccessLayer.Repositories;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepo : IEntityDataAccess<User>
    {
        Task<User> NewUser(string email, string password);
        Task<User?> LogIn(string email, string password);
    }
}
