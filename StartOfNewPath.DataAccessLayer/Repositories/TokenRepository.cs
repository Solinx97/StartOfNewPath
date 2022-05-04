using Microsoft.EntityFrameworkCore;
using StartOfNewPath.DataAccessLayer.Data;
using StartOfNewPath.DataAccessLayer.Entities;
using StartOfNewPath.DataAccessLayer.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace StartOfNewPath.DataAccessLayer.Repositories
{
    public class TokenRepository: ITokenRepository
    {
        private readonly SONPContext _context;

        public TokenRepository(SONPContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(RefreshToken item)
        {
            await _context.Set<RefreshToken>().AddAsync(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        public async Task<int> DeleteAsync(RefreshToken item)
        {
            _context.Set<RefreshToken>().Remove(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        public async Task<RefreshToken> Get(string token)
        {
            var allTokens = await _context.Set<RefreshToken>().AsNoTracking().ToListAsync();
            if (!allTokens.Any())
            {
                return null;
            }

            var foundToken = allTokens.Find(refreshToken => refreshToken.Token == token);
            return foundToken;
        }

        public async Task<int> UpdateAsync(RefreshToken item)
        {
            _context.Entry(item).State = EntityState.Modified;
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }
    }
}
