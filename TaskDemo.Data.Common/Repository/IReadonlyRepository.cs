using System;
using System.Linq;
using System.Linq.Expressions;

namespace TaskDemo.Data.Common.Repository
{
    /// <summary>
    /// Read-only repository for accessing data (no accidental data changes)
    /// </summary>
    public interface IReadonlyRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetQ(Expression<Func<TEntity, bool>> filter = null);

        TEntity GetByID(params object[] keyValues);
    }
}