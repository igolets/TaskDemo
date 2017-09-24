using System.Data.Entity;

namespace TaskDemo.Data.Common.EntityFramework
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation for EntityFramework
    /// </summary>
    /// <seealso cref="T:TaskDemo.Data.Common.IDataSession" />
    public class EfDataSession : IDataSession
    {
        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EfDataSession"/> class.
        /// </summary>
        /// <param name="provider">Session provider.</param>
        public EfDataSession(ISessionProvider provider)
        {
            provider.InitSession(this);
        }

        #endregion

        #region public properties

        /// <summary>
        /// Gets the context internal.
        /// </summary>
        internal DbContext ContextInternal
        {
            get;
            set;
        }

        #endregion

        #region public methods

        #endregion

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    ContextInternal.Dispose();
                    ContextInternal = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EfDataSession() {
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

        #region private fields

        #endregion
    }
}
