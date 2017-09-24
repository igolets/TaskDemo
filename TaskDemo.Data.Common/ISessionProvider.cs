namespace TaskDemo.Data.Common
{
    /// <summary>
    /// Session provider
    /// </summary>
    /// <remarks>You can put any (wrappers for) data that is needed to manage session</remarks>
    public interface ISessionProvider
    {
        /// <summary>
        /// Creates the session.
        /// </summary>
        /// <returns></returns>
        void InitSession(IDataSession session);

        /// <summary>
        /// Closes the session.
        /// </summary>
        void CloseSession(IDataSession session);
    }
}
