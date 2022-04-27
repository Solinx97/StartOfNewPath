using Microsoft.EntityFrameworkCore;
using StartOfNewPath.DataAccessLayer.Data;
using StartOfNewPath.DataAccessLayer.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StartOfNewPath.DataAccessLayer.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel>
        where TModel : class
    {
        private readonly SONPContext _context;

        public GenericRepository(SONPContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(TModel item)
        {
            await _context.Set<TModel>().AddAsync(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        public async Task<int> DeleteAsync(TModel item)
        {
            _context.Set<TModel>().Remove(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync() => await _context.Set<TModel>().AsNoTracking().ToListAsync();

        public async Task<TModel> GetByIdAsync(int id)
        {
            var entity = await _context.Set<TModel>().FindAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public async Task<int> UpdateAsync(TModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }
    }
}
