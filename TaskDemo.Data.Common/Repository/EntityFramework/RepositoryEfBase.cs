using System.Data.Entity;

namespace TaskDemo.Data.Common.Repository.EntityFramework
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation for EntityFramework
    /// </summary>
    public class RepositoryEfBase<TContext, TEntity> : ReadonlyRepositoryEfBase<TContext, TEntity>
        where TContext : DbContext, new()
        where TEntity : class
    {
        public RepositoryEfBase(IDataSession session) : base(session)
        {
        }

        public virtual void Insert(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Added;
        }

        public virtual void Delete(params object[] keyValues)
        {
            TEntity entityToDelete = DbSet.Find(keyValues);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }
    }
}
