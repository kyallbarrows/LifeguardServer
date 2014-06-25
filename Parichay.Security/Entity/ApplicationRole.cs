namespace Parichay.Security.Entity
{
    /// <summary>
    /// Plain Old CLR Object (POCO) representing the persistent attributes of a <see cref="ApplicationRole"/> object.
    /// </summary>
    public class ApplicationRole
    {
        #region Attributes
        private Application application;
        private Role role;
        #endregion Attributes

        #region Properties
        public Application Application
        {
            get { return application; }
            set { application = value; }
        }
        public Role Role
        {
            get { return role; }
            set { role = value; }
        }
        #endregion Properties

        #region Initialization
        public ApplicationRole()
        {
        }
        public ApplicationRole(Application app, Role role)
        {
            Application = app;
            Role = role;
        }
        #endregion Initialization

        #region Overrides
        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="object.GetHashCode"/> is suitable for use in hashing algorithms
        /// and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return (Application.GetHashCode() ^ Role.GetHashCode());
        }
        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
        /// </summary>
        /// <param name="obj">object instance to compare with the current instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="object"/>; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            ApplicationRole other = obj as ApplicationRole;
            return ((null != other) &&
                (other.Application.Id == Application.Id) &&
                    (other.Role.Id == Role.Id));
        }
        #endregion Overrides
    }
}