namespace TaskDemo.Data.Common.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Repository for accessing data
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="T:TaskDemo.Data.Common.Repository.IReadonlyRepository`1" />
    public interface IRepository<TEntity> : IReadonlyRepository<TEntity>
        where TEntity : class
    {
        void Insert(TEntity entity);

        void Delete(params object[] keyValues);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

        void Save();
    }
}
