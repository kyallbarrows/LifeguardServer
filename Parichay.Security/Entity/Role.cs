using System.Collections;

namespace Parichay.Security.Entity
{
    /// <summary>
    /// Plain Old CLR Object (POCO) representing the persistent attributes of a <see cref="Role"/> object.
    /// </summary>
    public class Role : AbstractProviderEntity
    {
        #region Fields
        private IList applications = new ArrayList();
        #endregion Fields

        #region Properties
        public IList Applications
        {
            get { return applications; }
            set { applications = value; }
        }
        #endregion Properties

        #region Initialization
        public Role() : base()
        {
        }
        public Role(string name)
            : base(name)
        {
        }
        #endregion Initialization
    }
}
