using DataAccessLayer.dbContext;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RefreshTokenRepo(AiLearnerDbContext context) : RepositoryBase<RefreshToken>(context), IRefreshTokenRepo
    {
        public async Task<RefreshToken?> GetRefreshToken(string token)
        {
            return await _context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task CreateRefreshToken(RefreshToken refreshToken)
        {
            await _context.Set<RefreshToken>().AddAsync(refreshToken);
        }

        public async Task<RefreshToken?> VarifyRefreshToken(string refreshToken)
        {
            RefreshToken? currentRefreshToken = await GetRefreshToken(refreshToken);

            if (currentRefreshToken == null) return null;
        
            if (currentRefreshToken.Expiration < DateTime.UtcNow) return null;

            if (currentRefreshToken.Revoked) return null;

            return currentRefreshToken;
        }
    }
}
