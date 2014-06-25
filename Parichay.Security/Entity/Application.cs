namespace Parichay.Security.Entity
{
    /// <summary>
    /// Plain Old CLR Object (POCO) representing the persistent attributes of a <see cref="Application"/> object.
    /// </summary>
    public class Application : AbstractProviderEntity
    {
        #region Initialization
        public Application() : base()
        {
        }
        public Application(string name)
            : base(name)
        {
        }
        #endregion Initialization
    }
}