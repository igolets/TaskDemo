using TaskDemo.Data.Common;
using TaskDemo.Data.Common.Repository.EntityFramework;
using TaskDemo.Data.EF;

namespace TaskDemo.Data.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Common implementation for TaskContext
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="TaskDemo.Data.Common.Repository.EntityFramework.ReadonlyRepositoryEfBase{TaskDemo.Data.EF.TaskContext, TEntity}" />
    /// <remarks>
    ///     Override if need to add any common logic into repository.
    /// </remarks>
    public class ReadonlyRepository<TEntity> : ReadonlyRepositoryEfBase<TaskContext, TEntity>
        where TEntity : class
    {
        public ReadonlyRepository(IDataSession session) : base(session) { }
    }
}
