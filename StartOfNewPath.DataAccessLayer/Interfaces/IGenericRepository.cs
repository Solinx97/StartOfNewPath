using System.Collections.Generic;
using System.Threading.Tasks;

namespace StartOfNewPath.DataAccessLayer.Interfaces
{
    public interface IGenericRepository<TModel>
        where TModel : class
    {
        Task<int> CreateAsync(TModel item);

        Task<int> UpdateAsync(TModel item);

        Task<int> DeleteAsync(TModel item);

        Task<TModel> GetByIdAsync(int id);

        Task<IEnumerable<TModel>> GetAllAsync();
    }
}
