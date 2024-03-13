using DataAccessLayer.Models.Entities;


namespace DataAccessLayer.Interfaces
{
    public interface IRefreshTokenRepo : IEntityDataAccess<RefreshToken>
    {
        Task<RefreshToken?> GetRefreshToken(string token);
        Task CreateRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken?> VarifyRefreshToken(string refreshToken);
    }
}
