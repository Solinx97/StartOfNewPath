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

        async Task<int> IGenericRepository<TModel>.CreateAsync(TModel item)
        {
            await _context.Set<TModel>().AddAsync(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        async Task<int> IGenericRepository<TModel>.DeleteAsync(TModel item)
        {
            _context.Set<TModel>().Remove(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        async Task<IEnumerable<TModel>> IGenericRepository<TModel>.GetAllAsync() => await _context.Set<TModel>().AsNoTracking().ToListAsync();

        async Task<TModel> IGenericRepository<TModel>.GetByIdAsync(int id)
        {
            var entity = await _context.Set<TModel>().FindAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        async Task<int> IGenericRepository<TModel>.UpdateAsync(TModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }
    }
}
