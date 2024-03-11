using DataAccessLayer.Models.Entities;


namespace DataAccessLayer.Interfaces
{
    public interface IRefreshTokenRepo : IEntityDataAccess<RefreshToken>
    {
        void Add(RefreshToken refreshToken);
    }
}
