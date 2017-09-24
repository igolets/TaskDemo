using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TaskDemo.Data.Common.EntityFramework;

namespace TaskDemo.Data.Common.Repository.EntityFramework
{
    /// <inheritdoc cref="IReadonlyRepository{TEntity}" />
    /// <summary>
    /// Implementation for EntityFramework
    /// </summary>
    public class ReadonlyRepositoryEfBase<TContext, TEntity> : IReadonlyRepository<TEntity>, IDisposable
        where TContext : DbContext, new()
        where TEntity : class
    {
        public ReadonlyRepositoryEfBase(IDataSession session)
        {
            Context = (TContext)((EfDataSession)session).ContextInternal;
            DBSet = Context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetQ(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = DBSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query;
        }

        public virtual TEntity GetByID(params object[] keyValues)
        {
            return DBSet.Find(keyValues);
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RepositoryEfBase() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

        protected readonly TContext Context;
        protected readonly DbSet<TEntity> DBSet;
    }
}
