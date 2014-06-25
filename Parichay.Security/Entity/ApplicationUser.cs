namespace Parichay.Security.Entity
{
    /// <summary>
    /// Plain Old CLR Object (POCO) representing the persistent attributes of a <see cref="ApplicationUser"/> object.
    /// </summary>
    public class ApplicationUser
    {
        #region Attributes
        private Application application;
        private User user;
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
        #endregion Properties

        #region Initialization
        public ApplicationUser()
        {
        }
        public ApplicationUser(Application app, User user)
        {
            Application = app;
            User = user;
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
            return (Application.GetHashCode() ^ User.GetHashCode());
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
            ApplicationUser other = obj as ApplicationUser;
            return ((null != other) &&
                (other.Application.Id == Application.Id) &&
                    (other.User.Id == User.Id));
        }
        #endregion Overrides
    }
}