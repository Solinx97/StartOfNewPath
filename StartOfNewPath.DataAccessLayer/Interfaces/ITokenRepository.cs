using StartOfNewPath.DataAccessLayer.Entities;
using System.Threading.Tasks;

namespace StartOfNewPath.DataAccessLayer.Interfaces
{
    public interface ITokenRepository
    {
        Task<int> CreateAsync(RefreshToken item);

        Task<int> UpdateAsync(RefreshToken item);

        Task<int> DeleteAsync(RefreshToken item);

        Task<RefreshToken> Get(string token);
    }
}
