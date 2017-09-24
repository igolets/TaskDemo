using System.Data.Entity;

namespace TaskDemo.Data.Common.EntityFramework
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation for EntityFramework
    /// </summary>
    public sealed class EfSessionProvider<T> : ISessionProvider
        where T : DbContext, new()
    {
        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EfSessionProvider&lt;T&gt;"/> class.
        /// </summary>
        public EfSessionProvider()
        {
            SetInitializer();
        }

        #endregion

        #region public methods

        /// <inheritdoc />
        /// <summary>
        /// Creates the session.
        /// </summary>
        /// <returns></returns>
        public void InitSession(IDataSession session)
        {
            T context = new T();
            ((EfDataSession)session).ContextInternal = context;
        }

        /// <inheritdoc />
        /// <summary>
        /// Closes the session.
        /// </summary>
        /// <param name="session"></param>
        public void CloseSession(IDataSession session)
        {
            session.Dispose();
        }

        #endregion

        #region protected methods

        /// <summary>
        /// Sets the initializer.
        /// </summary>
        private void SetInitializer()
        {
        }

        #endregion
    }
}
