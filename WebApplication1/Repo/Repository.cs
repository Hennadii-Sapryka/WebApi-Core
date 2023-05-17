using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using WebApi.Data;

namespace WebApi.Repo
{
    public sealed class Repository<TEntity> where TEntity : class
    {
        private readonly Context _context;
        private readonly DbSet<TEntity> _dbEntities;
        public Repository(Context context)
        {
            _context = context;
            _dbEntities = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes)
        {
            var dbSet = _context.Set<TEntity>();
            var query = includes.Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(dbSet, (current, include) => current.Include(include));
            return query ?? dbSet;
        }

        public async Task<TEntity> AddAsync(TEntity entity) => (await _dbEntities.AddAsync(entity)).Entity;

        public void Delete(TEntity entity) => _context.Entry(entity).State = EntityState.Deleted;

        public async Task<TEntity> UpdateAsync(TEntity entity) =>
            await Task.Run(() => _dbEntities.Update(entity).Entity);

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
        public void Detach(TEntity entity) => _context.Entry(entity).State = EntityState.Detached;
    }
}
