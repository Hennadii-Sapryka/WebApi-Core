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
        public Repository(Context context) {
            _context = context;
            _dbEntities = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes)
        {
            var dbSet = _context.Set<TEntity>();
            var query = includes.Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(dbSet, (current, include)=>current.Include(include));
            return query ?? dbSet;
        }
    }
}
