using DataAccessLayer.dbContext;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RefreshTokenRepo(AiLearnerDbContext context) : RepositoryBase<RefreshToken>(context), IRefreshTokenRepo
    {
        public void Add(RefreshToken refreshToken)
        {
            _context.Set<RefreshToken>().Add(refreshToken);
        }
    }
}
