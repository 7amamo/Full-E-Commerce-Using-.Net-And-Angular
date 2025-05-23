using ECom.Core.Interfaces;
using ECom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;

        public GenericRepository( AppDbContext DbContext) {
            _dbContext = DbContext;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        
             

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
         => await _dbContext.Set<T>().AsNoTracking().ToListAsync();


        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var Query = _dbContext.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                Query = Query.Include(item);
            }
            return await Query.ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
          => await _dbContext.Set<T>().FindAsync(id);


        public Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var Query = _dbContext.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                Query = Query.Include(item);
            }
            var entity =  Query.FirstOrDefaultAsync( x => EF.Property<int>(x,"Id") == id);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

    }
}
