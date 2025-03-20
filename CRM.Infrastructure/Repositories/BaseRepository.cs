using CRM.Domain.Entities;
using CRM.Domain.Models;
using CRM.Infrastructure.Context;
using CRM.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CRM.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public async Task RemoveAsync(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public IQueryable<TEntity?> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet
                .AsNoTracking()
                .Where(predicate);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public async Task<(IEnumerable<TEntity> entities, int totalCount)> GetFilteredAsync<TFilterParams>(
        TFilterParams filterParams,
        Func<IQueryable<TEntity>, TFilterParams, IQueryable<TEntity>> filterFunction)
        where TFilterParams : BaseFilterParams
        {
            IQueryable<TEntity> query = filterFunction(_dbSet.AsNoTracking(), filterParams);

            int totalCount = await query.CountAsync();

            var entities = await query
                .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToListAsync();

            return (entities, totalCount);
        }
    }
}