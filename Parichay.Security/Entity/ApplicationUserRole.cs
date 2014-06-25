namespace Parichay.Security.Entity
{
    /// <summary>
    /// Plain Old CLR Object (POCO) representing the persistent attributes of a <see cref="ApplicationUserRole"/> object.
    /// </summary>
    public class ApplicationUserRole
    {
        #region Attributes
        private Application application;
        private User user;
        private Role role;
        #endregion Attributes

        #region Properties
        public Application Application
        {
            get { return application; }
            set { application = value; }
        }
        public User User
        {
            get { return user; }
            set { user = value; }
        }
        public Role Role
        {
            get { return role; }
            set { role = value; }
        }
        #endregion Properties

        #region Initialization
        public ApplicationUserRole()
        {
        }
        public ApplicationUserRole(Application app, User user, Role role)
        {
            Application = app;
            User = user;
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
            return (Application.GetHashCode() ^ User.GetHashCode() ^ Role.GetHashCode());
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
            ApplicationUserRole other = obj as ApplicationUserRole;
            return ((null != other) &&
                (other.Application.Id == Application.Id) &&
                    (other.User.Id == User.Id) &&
                        (other.Role.Id == Role.Id));
        }
        #endregion Overrides
    }
}