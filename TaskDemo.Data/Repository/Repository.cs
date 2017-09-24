using TaskDemo.Data.Common;
using TaskDemo.Data.Common.Repository;
using TaskDemo.Data.Common.Repository.EntityFramework;
using TaskDemo.Data.EF;

namespace TaskDemo.Data.Repository
{
    /// <inheritdoc cref="TaskDemo.Data.Common.Repository.EntityFramework.RepositoryEfBase" />
    /// <summary>
    /// Common implementation for TaskContext
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="TaskDemo.Data.Common.Repository.EntityFramework.RepositoryEfBase{TaskDemo.Data.EF.TaskContext, TEntity}" />
    /// <seealso cref="TaskDemo.Data.Common.Repository.IRepository`1" />
    /// <remarks>
    ///     Override if need to add any common logic into repository.
    /// </remarks>
    public class Repository<TEntity> : RepositoryEfBase<TaskContext, TEntity>, IRepository<TEntity>
        where TEntity : class
    {
        public Repository(IDataSession session) : base(session) { }
    }
}
