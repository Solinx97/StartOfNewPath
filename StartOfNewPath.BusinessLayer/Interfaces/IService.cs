using System.Collections.Generic;
using System.Threading.Tasks;

namespace StartOfNewPath.BusinessLayer.Interfaces
{
    public interface IService<TModel>
        where TModel : class
    {
        Task<int> CreateAsync(TModel item);

        Task<int> UpdateAsync(TModel item);

        Task<int> DeleteAsync(TModel item);

        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetByIdAsync(int id);
    }
}
