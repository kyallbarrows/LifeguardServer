using System;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using NHibernate;
using NHibernate.Type;
using Parichay.Security.Entity;
using Parichay.Security.Helper;
using Parichay.Security.Properties;
using Parichay.Security.Util;

namespace Parichay.Security
{
    /// <summary>
    /// Provides membership services using NHibernate as the persistence mechanism.
    /// </summary>
    public sealed class NHibernateMembershipProvider : MembershipProvider
    {
        #region Enumerations
        /// <summary>
        /// Types of failure that need to be tracked on a per user basis.
        /// </summary>
        private enum FailureType
        {
            Password,
            PasswordAnswer
        }
        #endregion

        #region Fields
        private Application application = new Application();
        private bool requiresQuestionAndAnswer;
        private bool requiresUniqueEmail;
        private bool enablePasswordRetrieval;
        private bool enablePasswordReset;
        private int maxInvalidPasswordAttempts;
        private int passwordAttemptWindow;
        private MembershipPasswordFormat passwordFormat;
        private int minRequiredPasswordLength;
        private int minRequiredNonAlphanumericCharacters;
        private string passwordStrengthRegularExpression;
        private MachineKeySection machineKey;
        #endregion Fields

        #region Properties
        /// <summary>
        /// The name of the application using the custom membership provider.
        /// </summary>
        /// <returns>
        /// The name of the application using the custom membership provider.
        /// </returns>
        public override string ApplicationName
        {
            get { return application.Name; }
            set { application.Name = value; }
        }
        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require
        /// the user to answer a password question for password reset and retrieval.
        /// </summary>
        /// <returns>
        /// <c>true</c> if a password answer is required for password reset and retrieval;
        /// otherwise, <c>false</c>. The default is <c>true</c>.
        /// </returns>
        public override bool RequiresQuestionAndAnswer
        {
            get { return requiresQuestionAndAnswer; }
        }
        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require
        /// a unique e-mail address for each user name.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the membership provider requires a unique e-mail address;
        /// otherwise, <c>false</c>. The default is <c>true</c>.
        /// </returns>
        public override bool RequiresUniqueEmail
        {
            get { return requiresUniqueEmail; }
        }
        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve
        /// their passwords.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the membership provider is configured to support password retrieval;
        /// otherwise, <c>false</c>. The default is <c>false</c>.
        /// </returns>
        public override bool EnablePasswordRetrieval
        {
            get { return enablePasswordRetrieval; }
        }
        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their
        /// passwords.
        /// </summary>
        ///<returns>
        ///<c>true</c> if the membership provider supports password reset; otherwise, <c>false</c>.
        /// The default is <c>true</c>.
        ///</returns>
        public override bool EnablePasswordReset
        {
            get { return enablePasswordReset; }
        }
        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the
        /// membership user is locked out.
        /// </summary>
        /// <returns>
        /// The number of invalid password or password-answer attempts allowed before the membership
        /// user is locked out.
        /// </returns>
        public override int MaxInvalidPasswordAttempts
        {
            get { return maxInvalidPasswordAttempts; }
        }
        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer
        /// attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <returns>
        /// The number of minutes in which a maximum number of invalid password or password-answer attempts
        /// are allowed before the membership user is locked out.
        /// </returns>
        public override int PasswordAttemptWindow
        {
            get { return passwordAttemptWindow; }
        }
        /// <summary>
        /// Gets a value indicating the format for storing passwords in the data store.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Web.Security.MembershipPasswordFormat"></see> values indicating
        /// the format for storing passwords in the data store.
        /// </returns>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return passwordFormat; }
        }
        /// <summary>
        /// Gets the minimum length required for a password.
        /// </summary>
        /// <returns>
        /// The minimum length required for a password. 
        /// </returns>
        public override int MinRequiredPasswordLength
        {
            get { return minRequiredPasswordLength; }
        }
        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <returns>
        /// The minimum number of special characters that must be present in a valid password.
        /// </returns>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return minRequiredNonAlphanumericCharacters; }
        }
        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <returns>
        /// A regular expression used to evaluate a password.
        /// </returns>
        public override string PasswordStrengthRegularExpression
        {
            get { return passwordStrengthRegularExpression; }
        }
        #endregion Properties

        #region Initialization
        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific
        /// attributes specified in the configuration for this provider.</param>
        /// <param name="name">The friendly name of the provider.</param>
        /// <exception cref="ArgumentNullException">The name of the provider is null.</exception>
        /// <exception cref="InvalidOperationException">An attempt is made to call <see cref="Initialize(System.String,System.Collections.Specialized.NameValueCollection)"></see> on a provider after the provider has already been initialized.</exception>
        /// <exception cref="ArgumentException">The name of the provider has a length of zero.</exception>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from Web.config.
            if (null == config)
            {
                throw (new ArgumentNullException("config"));
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "NHibernateMembershipProvider";
            }
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "NHibernate Membership Provider");
            }
            // Call the base class implementation.
            base.Initialize(name, config);

            // Load configuration data.
            application =
                NHibernateProviderEntityHelper.CreateOrLoadApplication(
                    ConfigurationUtil.GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath));
            requiresQuestionAndAnswer =
                Convert.ToBoolean(ConfigurationUtil.GetConfigValue(config["requiresQuestionAndAnswer"], "False"));
            requiresUniqueEmail = Convert.ToBoolean(ConfigurationUtil.GetConfigValue(config["requiresUniqueEmail"], "True"));
            enablePasswordRetrieval = Convert.ToBoolean(ConfigurationUtil.GetConfigValue(config["enablePasswordRetrieval"], "True"));
            enablePasswordReset = Convert.ToBoolean(ConfigurationUtil.GetConfigValue(config["enablePasswordReset"], "True"));
            maxInvalidPasswordAttempts =
                Convert.ToInt32(ConfigurationUtil.GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            passwordAttemptWindow = Convert.ToInt32(ConfigurationUtil.GetConfigValue(config["passwordAttemptWindow"], "10"));
            minRequiredPasswordLength = Convert.ToInt32(ConfigurationUtil.GetConfigValue(config["minRequiredPasswordLength"], "7"));
            minRequiredNonAlphanumericCharacters =
                Convert.ToInt32(ConfigurationUtil.GetConfigValue(config["minRequiredAlphaNumericCharacters"], "1"));
            passwordStrengthRegularExpression =
                Convert.ToString(ConfigurationUtil.GetConfigValue(config["passwordStrengthRegularExpression"], string.Empty));

            // Initialize the password format.
            switch (ConfigurationUtil.GetConfigValue(config["passwordFormat"], "Hashed"))
            {
                case "Hashed":
                    passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw ExceptionUtil.NewProviderException(this, "password format not supported");
            }

            // Get encryption and decryption key information from the configuration.
            Configuration cfg = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
            machineKey = (MachineKeySection) cfg.GetSection("system.web/machineKey");
            if ("Auto".Equals(machineKey.Decryption))
            {
                // Create our own key if one has not been specified.
                machineKey.DecryptionKey = KeyCreator.CreateKey(24);
                machineKey.ValidationKey = KeyCreator.CreateKey(64);
            }
        }
        #endregion Initialization

        #region Operations

        #region User
        /// <summary>
        /// Adds a new membership user to the data source.
        /// </summary>
        /// <param name="username">the user name for the new user.</param>
        /// <param name="password">the password for the new user.</param>
        /// <param name="email">the e-mail address for the new user.</param>
        /// <param name="passwordQuestion">the password question for the new user.</param>
        /// <param name="passwordAnswer">the password answer for the new user.</param>
        /// <param name="isApproved">whether or not the new user is approved to be validated.</param>
        /// <param name="providerUserKey">the unique identifier from the membership data source for the user.</param>
        /// <param name="status">a <see cref="MembershipCreateStatus"/> enumeration value indicating whether the user was created successfully.</param>
        /// <returns>
        /// A <see cref="MembershipUser"/> object populated with the information for the newly created user.
        /// </returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
            string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            // Raise the ValidatingPassword event in case an event handler has been defined.
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);
            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            // Validate the e-mail address has not already been specified, if required.
            if (RequiresUniqueEmail && !string.IsNullOrEmpty(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            // Attempt to get the user record associated to the given user name.
            if (NHibernateProviderEntityHelper.UserExists(username))
            {
                // Indicate we have found an existing user record.
                status = MembershipCreateStatus.DuplicateUserName;
            }
            else
            {
                // NOTE: providerUserKey is ignored on purpose. In this implementation it represents the user identifier.

                // Insert user record in the data store.
                User user = new User();
                user.Name = username;
                user.Password = EncodePassword(password, machineKey.ValidationKey);
                user.PasswordFormat = (int) PasswordFormat;
                user.PasswordSalt = machineKey.ValidationKey;
                user.Email = email;
                user.PasswordQuestion = passwordQuestion;
                user.PasswordAnswer = EncodePassword(passwordAnswer, machineKey.ValidationKey);
                user.IsApproved = isApproved;
                user.Applications.Add(application);
                try
                {
                    NHibernateHelper.SaveOrUpdate(user);
                    status = MembershipCreateStatus.Success;
                }
                catch (Exception ex)
                {
                    //status = MembershipCreateStatus.UserRejected;
                    throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToCreate, ex);
                }

                // Return the newly created user record.
                return GetUser(username, false);
            }

            // Indicate we were unable to create the user record.
            return null;
        }
        /// <summary>
        /// Removes a user from the data store.
        /// </summary>
        /// <param name="username">the name of the user to delete.</param>
        /// <param name="deleteAllRelatedData"><c>true</c> to delete data related to the user from the data store; <c>false</c> to leave related data intact.</param>
        /// <returns><c>true</c> if the user was successfully deleted; otherwise, <c>false</c>.</returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            // Assume we are unable to perform the operation.
            bool result;

            // Delete the corresponding user record from the data store.
            try
            {
                // Get the user information.
                User user = NHibernateProviderEntityHelper.GetUser(username);
                if (null != user)
                {
                    // Process commands to delete all data for the user in the database.
                    if (deleteAllRelatedData)
                    {
                        // Delete the references to applications/users.
                        object[] values = new object[] { application.Id, user.Id };
                        IType[] types = new IType[] { NHibernateUtil.Int32, NHibernateUtil.Int32 };
                        NHibernateHelper.DeleteByNamedQuery("ApplicationUserRole.RemoveAppUserReferences", values, types);
                    }
                    // Delete the user record.
                    NHibernateHelper.Delete(user);
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToDelete, ex);
            }

            // Return the result of the operation.
            return result;
        }
        /// <summary>
        /// Updates information about a user in the data source.
        /// </summary>
        /// <param name="user">a <see cref="MembershipUser"/> object that represents the user to update and the updated information for the user.</param>
        public override void UpdateUser(MembershipUser user)
        {
            // Perform the update in the data store.
            try
            {
                NHibernateHelper.SaveOrUpdate(NHibernateProviderEntityHelper.GetUser(user.UserName).FromMembershipUser(user));
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToUpdate, ex);
            }
        }
        /// <summary>
        /// Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <param name="username">the name of the user for whom to clear the lock status.</param>
        /// <returns><c>true</c> if the user was unlocked successfully; otherwise, <c>false</c>.</returns>
        public override bool UnlockUser(string username)
        {
            // Assume we are unable to perform the operation.
            bool result = false;

            // Unlock the user in the data store.
            try
            {
                // Get the user record form the data store.
                User user = NHibernateProviderEntityHelper.GetUser(username);
                if (null != user)
                {
                    // Perform the update in the data store.
                    user.IsLockedOut = false;
                    user.LastLockedOutDate = DateTime.Now;
                    user.LastActivityDate = DateTime.Now;
                    NHibernateHelper.SaveOrUpdate(user);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToUnlock, ex);
            }

            // Return the result of the operation.
            return result;
        }
        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">the name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns><c>true</c> if the specified username and password are valid; otherwise, <c>false</c>.</returns>
        public override bool ValidateUser(string username, string password)
        {
            // Assume the given user is not valid.
            bool isValid = false;

            // Get the password and the flag indicating the user is approved.
            User user = NHibernateProviderEntityHelper.GetUser(username);
            if (null != user)
            {
                // Ensure the passwords match but only if the user is not already locked out of the system.
                if (!user.IsLockedOut && CheckPassword(password, user.Password, user.PasswordSalt))
                {
                    // Ensure the user is allowed to login.
                    if (user.IsApproved)
                    {
                        // Indicate the user is valid.
                        isValid = true;
                        // Update the user's last login date.
                        UpdateLastLoginDate(username);
                    }
                }
                else
                {
                    // Update the failure count.
                    UpdateFailureCount(username, FailureType.Password);
                }
            }

            // Return the result of the operation.
            return isValid;
        }
        /// <summary>
        /// Gets information from the data source for a user based on the unique identifier for the
        /// membership user. Provides an option to update the last-activity timestamp for the user.
        /// </summary>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline"><c>true</c> to update the last-activity date/time stamp for the user; <c>false</c> to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="MembershipUser"></see> object populated with the specified user's information.
        /// </returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            // Assume we were unable to find the user.
            MembershipUser user = null;

            // Ensure the provider key is valid.
            if (null == providerUserKey)
            {
                throw (new ArgumentNullException("providerUserKey"));
            }

            // Get the user record from the data store.
            try
            {
                IList users = NHibernateHelper.FindByNamedQuery("User.ById", providerUserKey, NHibernateUtil.Int32);
                if (1 == users.Count)
                {
                    user = ((User) users[0]).ToMembershipUser(Name);
                }
                else if (1 < users.Count)
                {
                    throw ExceptionUtil.NewProviderException(this, Resources.User_TooManyMatching);
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToGet, ex);
            }

            // Determine if we need to update the activity information.
            if (userIsOnline && (user != null))
            {
                // Update the last activity timestamp (LastActivityDate).
                UpdateLastActivityDate(user.UserName);
            }

            // Return the resulting user.
            return user;
        }
        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity
        /// timestamp for the user.
        /// </summary>
        /// <param name="username">the name of the user for whom to get information.</param>
        /// <param name="userIsOnline"><c>true</c> to update the last-activity date/time stamp for the user; <c>false</c> to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>A <see cref="MembershipUser"/> object populated with the specified user's information.</returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            // Assume we were unable to find the user.
            MembershipUser user = null;

            // Don't accept empty user names.
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }

            // Get the user record from the data store.
            try
            {
                IList users = NHibernateHelper.FindByNamedQuery("User.ByLoweredName", username.ToLower(), NHibernateUtil.String);
                if (1 == users.Count)
                {
                    user = ((User) users[0]).ToMembershipUser(Name);
                }
                else if (1 < users.Count)
                {
                    throw ExceptionUtil.NewProviderException(this, Resources.User_TooManyMatching);
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToGet, ex);
            }

            // Determine if we need to update the activity information.
            if (userIsOnline && (user != null))
            {
                // Update the last activity timestamp (LastActivityDate).
                UpdateLastActivityDate(user.UserName);
            }

            // Return the resulting user.
            return user;
        }
        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email">the e-mail address to search for.</param>
        /// <returns>The user name associated with the specified e-mail address. If no match is found, return <c>null</c>.</returns>
        public override string GetUserNameByEmail(string email)
        {
            // Assume we were unable to find the corresponding user name.
            string username = null;

            // Don't accept empty emails.
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }

            // Get the user record from the data store.
            try
            {
                IList usernames =
                    NHibernateHelper.FindByNamedQuery("User.GetLoweredNameByLoweredEmail", email.ToLower(), NHibernateUtil.String);
                if (1 == usernames.Count)
                {
                    username = usernames[0].ToString();
                }
                else if (1 < usernames.Count)
                {
                    throw ExceptionUtil.NewProviderException(this, Resources.User_TooManyMatching);
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToGet, ex);
            }

            // Return the name of the user associated to the given e-mail address, if any.
            return username;
        }
        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>The number of users currently accessing the application.</returns>
        public override int GetNumberOfUsersOnline()
        {
            // Assume there are no users online.
            int numberOfUsersOnline;

            // Get a count of users whose LastActivityDate is greater than the threashold.
            try
            {
                // Determine the threashold based on the configured time window against which we'll compare.
                TimeSpan onlineSpan = new TimeSpan(0, Membership.UserIsOnlineTimeWindow, 0);
                DateTime compareTime = DateTime.Now.Subtract(onlineSpan);
                object[] values = new object[] { application.Id, compareTime };
                IType[] types = new IType[] { NHibernateUtil.Int32, NHibernateUtil.DateTime };
                numberOfUsersOnline = NHibernateHelper.CountByNamedQuery("ApplicationUser.GetUsersOnline", values, types);
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToGetOnlineNumber, ex);
            }

            // Return the result of the operation.
            return numberOfUsersOnline;
        }
        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data.
        /// </summary>
        /// <param name="pageIndex">the index of the page of results to return. <c>pageIndex</c> is zero-based.</param>
        /// <param name="pageSize">the size of the page of results to return.</param>
        /// <param name="totalRecords">the total number of matched users.</param>
        /// <returns>
        /// A <see cref="MembershipUserCollection"/> instance that contains a page of <c>pageSize</c> of
        /// <see cref="MembershipUser"/> objects beginning at the page specified by <c>pageIndex</c>.
        /// </returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            // Create a placeholder for all user accounts retrived, if any.
            MembershipUserCollection users = new MembershipUserCollection();

            // Get the user record from the data store.
            try
            {
                // Perform the search.
                IList page = NHibernateHelper.FindPageByNamedQuery("ApplicationUser.FindAll", application.Id, NHibernateUtil.Int32, pageIndex, pageSize);
                foreach (ApplicationUser appUser in page)
                {
                    users.Add(appUser.User.ToMembershipUser(Name));
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToGetAllUsers, ex);
            }

            // Prepare return parameters.
            totalRecords = NHibernateHelper.CountByNamedQuery("ApplicationUser.FindAll", application.Id, NHibernateUtil.Int32);

            // Return the result of the operation.
            return users;
        }
        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch">the user name to search for.</param>
        /// <param name="pageIndex">the index of the page of results to return. <c>pageIndex</c> is zero-based.</param>
        /// <param name="pageSize">the size of the page of results to return.</param>
        /// <param name="totalRecords">the total number of matched users.</param>
        /// <returns>
        /// A <see cref="MembershipUserCollection"/> instance that contains a page of <c>pageSize</c> of
        /// <see cref="MembershipUser"/> objects beginning at the page specified by <c>pageIndex</c>.
        /// </returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            // Create a placeholder for all user accounts retrived, if any.
            MembershipUserCollection users = new MembershipUserCollection();

            // Get the user record from the data store.
            try
            {
                // Replace all * and ? wildcards for % and _, respectively.
                usernameToMatch = usernameToMatch.Replace('*', '%');
                usernameToMatch = usernameToMatch.Replace('?', '_');

                // Perform the search.
                object[] values = new object[] { application.Id, usernameToMatch.ToLower() };
                IType[] types = new IType[] { NHibernateUtil.Int32, NHibernateUtil.String };
                IList page = NHibernateHelper.FindPageByNamedQuery("ApplicationUser.FindByLoweredName", values, types, pageIndex, pageSize);
                foreach (ApplicationUser appUser in page)
                {
                    users.Add(appUser.User.ToMembershipUser(Name));
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToGetByName, ex);
            }

            // Prepare return parameters.
            totalRecords = users.Count;

            // Return the result of the operation.
            return users;
        }
        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">the e-mail address to search for.</param>
        /// <param name="pageIndex">the index of the page of results to return. <c>pageIndex</c> is zero-based.</param>
        /// <param name="pageSize">the size of the page of results to return.</param>
        /// <param name="totalRecords">the total number of matched users.</param>
        /// <returns>
        /// A <see cref="MembershipUserCollection"/> instance that contains a page of <c>pageSize</c> of
        /// <see cref="MembershipUser"/> objects beginning at the page specified by <c>pageIndex</c>.
        /// </returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            // Create a placeholder for all user accounts retrived, if any.
            MembershipUserCollection users = new MembershipUserCollection();

            // Get the user record from the data store.
            try
            {
                // Replace all * and ? wildcards for % and _, respectively.
                emailToMatch = emailToMatch.Replace('*', '%');
                emailToMatch = emailToMatch.Replace('?', '_');

                // Perform the search.
                object[] values = new object[] { application.Id, emailToMatch.ToLower() };
                IType[] types = new IType[] { NHibernateUtil.Int32, NHibernateUtil.String };
                IList page = NHibernateHelper.FindPageByNamedQuery("ApplicationUser.FindByLoweredEmail", values, types, pageIndex, pageSize);
                foreach (ApplicationUser appUser in page)
                {
                    users.Add(appUser.User.ToMembershipUser(Name));
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToGetByEmail, ex);
            }

            // Prepare return parameters.
            totalRecords = users.Count;

            // Return the result of the operation.
            return users;
        }
        #endregion User

        #region Password
        /// <summary>
        /// Gets the password for the specified user name from the data store.
        /// </summary>
        /// <param name="username">the name of the user for whom to retrieve the password.</param>
        /// <param name="answer">the password answer for the user. </param>
        /// <returns>The password for the specified user name.</returns>
        public override string GetPassword(string username, string answer)
        {
            // Assume we are not able to fetch the user's password.
            string password = null;

            // Ensure password retrievals are allowed.
            if (!EnablePasswordRetrieval)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Pwd_RetrievalNotEnabled);
            }
            // Is the request made when the password in Hashed?
            if (MembershipPasswordFormat.Hashed == PasswordFormat)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Pwd_CannotRetrieveHashed);
            }

            // Get the user from the data store.
            User user = NHibernateProviderEntityHelper.GetUser(username);
            if (null != user)
            {
                // Determine if the user is required to answer a password question.
                if (RequiresQuestionAndAnswer && !CheckPassword(answer, user.PasswordAnswer, user.PasswordSalt))
                {
                    UpdateFailureCount(username, FailureType.PasswordAnswer);
                    throw (new MembershipPasswordException(ExceptionUtil.FormatExceptionMessage(this, Resources.Pwd_IncorrectAnswer)));
                }

                // Once the answer has been given, if required, determine if we need to unencode the password before
                // we return it. The call to UnencodePassword will just return the given password as is if the password
                // format is set to MembershipPasswordFormat.Clear.
                password = UnencodePassword(user.Password);
            }

            // Return the retrieved password.
            return password;
        }
        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <param name="newPassword">the new password for the specified user.</param>
        /// <param name="oldPassword">the current password for the specified user.</param>
        /// <param name="username">the name of the user for whom to update the password.</param>
        /// <returns><c>true</c> if the password was updated successfully; otherwise, <c>false</c>.</returns>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            // Assume we are unable to perform the operation.
            bool result = false;

            // Ensure we are dealing with a valid user.
            if (ValidateUser(username, oldPassword))
            {
                // Raise the ValidatingPassword event in case an event handler has been defined.
                ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);
                OnValidatingPassword(args);
                if (args.Cancel)
                {
                    // Check for a specific error message.
                    if (null != args.FailureInformation)
                    {
                        throw (args.FailureInformation);
                    }
                    else
                    {
                        throw ExceptionUtil.NewProviderException(this, Resources.Pwd_ChangeCancelledDueToNewPassword);
                    }
                }

                // Get the user from the data store.
                User user = NHibernateProviderEntityHelper.GetUser(username);
                if (null != user)
                {
                    try
                    {
                        // Encode the new password.
                        user.Password = EncodePassword(newPassword, user.PasswordSalt);
                        user.LastPasswordChangeDate = DateTime.Now;
                        user.LastActivityDate = DateTime.Now;
                        // Update user record with the new password.
                        NHibernateHelper.SaveOrUpdate(user);
                        // Indicate we successfully changed the password.
                        result = true;
                    }
                    catch
                    {
                        throw (new MembershipPasswordException(
                            ExceptionUtil.FormatExceptionMessage(this, Resources.Pwd_OpCancelledDueToAccountLocked)));
                    }
                }
            }

            // Return the result of the operation.
            return result;
        }
        /// <summary>
        /// Processes a request to update the password question and answer for a membership user.
        /// </summary>
        /// <param name="newPasswordQuestion">the new password question for the specified user. </param>
        /// <param name="newPasswordAnswer">the new password answer for the specified user. </param>
        /// <param name="username">the name of the user for whom to update the password question and answer.</param>
        /// <param name="password">the password for the specified user. </param>
        /// <returns><c>true</c> if the password question and answer are updated successfully; otherwise, <c>false</c>.</returns>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
            string newPasswordAnswer)
        {
            // Assume we are unable to perform the operation.
            bool result = false;

            // Ensure we are dealing with a valid user.
            if (ValidateUser(username, password))
            {
                // Get the user from the data store.
                User user = NHibernateProviderEntityHelper.GetUser(username);
                if (null != user)
                {
                    try
                    {
                        // Update the new password question and answer.
                        user.PasswordQuestion = newPasswordQuestion;
                        user.PasswordAnswer = EncodePassword(newPasswordAnswer, user.PasswordSalt);
                        user.LastActivityDate = DateTime.Now;
                        // Update user record with the new password.
                        NHibernateHelper.SaveOrUpdate(user);
                        // Indicate a successful operation.
                        result = true;
                    }
                    catch
                    {
                        throw (new MembershipPasswordException(
                            ExceptionUtil.FormatExceptionMessage(this, Resources.Pwd_UnableToChangeQandA)));
                    }
                }
            }

            // Return the result of the operation.
            return result;
        }
        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="username">the name of the user for whom to reset the password.</param>
        /// <param name="answer">the password answer for the specified user. </param>
        /// <returns>The new password for the specified user.</returns>
        public override string ResetPassword(string username, string answer)
        {
            // Prepare a placeholder for the new passowrd.
            string newPassword;

            // Ensure password retrievals are allowed.
            if (!EnablePasswordReset)
            {
                throw (new MembershipPasswordException(ExceptionUtil.FormatExceptionMessage(this, Resources.Pwd_ResetNotEnabled)));
            }
            // Determine if a valid answer has been given if question and answer is required.
            if ((null == answer) && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(username, FailureType.PasswordAnswer);
                throw (new MembershipPasswordException(ExceptionUtil.FormatExceptionMessage(this, Resources.Pwd_AnswerRequiredForReset)));
            }

            // Generate a new random password of the specified length.
            newPassword = Membership.GeneratePassword(minRequiredPasswordLength, MinRequiredNonAlphanumericCharacters);

            // Raise the ValidatingPassword event in case an event handler has been defined.
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);
            OnValidatingPassword(args);
            if (args.Cancel)
            {
                // Check for a specific error message.
                if (null != args.FailureInformation)
                {
                    throw (args.FailureInformation);
                }
                else
                {
                    throw (new MembershipPasswordException(ExceptionUtil.FormatExceptionMessage(this,
                        Resources.Pwd_ResetCancelledDueToNewPassword)));
                }
            }

            // Get the user from the data store.
            User user = NHibernateProviderEntityHelper.GetUser(username);
            if (null != user)
            {
                // Determine if the user is locked out of the system.
                if (user.IsLockedOut)
                {
                    throw (new MembershipPasswordException(ExceptionUtil.FormatExceptionMessage(this, Resources.User_IsLockedOut)));
                }

                // Determine if the user is required to answer a password question.
                if (RequiresQuestionAndAnswer && !CheckPassword(answer, user.PasswordAnswer, user.PasswordSalt))
                {
                    UpdateFailureCount(username, FailureType.PasswordAnswer);
                    throw (new MembershipPasswordException(ExceptionUtil.FormatExceptionMessage(this, Resources.Pwd_IncorrectAnswer)));
                }

                // Update user record with the new password.
                try
                {
                    user.Password = EncodePassword(newPassword, user.PasswordSalt);
                    user.LastPasswordChangeDate = DateTime.Now;
                    user.LastActivityDate = DateTime.Now;
                    NHibernateHelper.SaveOrUpdate(user);
                }
                catch
                {
                    throw (new MembershipPasswordException(
                        ExceptionUtil.FormatExceptionMessage(this, Resources.Pwd_OpCancelledDueToAccountLocked)));
                }
            }

            // Return the resulting new password.
            return newPassword;
        }


        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="username">the name of the user for whom to reset the password.</param>
        /// <param name="answer">the password answer for the specified user. </param>
        /// <returns>The new password for the specified user.</returns>
        public string AdminResetPassword(string username)
        {
            // Prepare a placeholder for the new passowrd.
            string newPassword;

            // Ensure password retrievals are allowed.
            if (!EnablePasswordReset)
            {
                throw (new MembershipPasswordException(ExceptionUtil.FormatExceptionMessage(this, Resources.Pwd_ResetNotEnabled)));
            }

            // Generate a new random password of the specified length.
            newPassword = Membership.GeneratePassword(minRequiredPasswordLength, MinRequiredNonAlphanumericCharacters);

            // Raise the ValidatingPassword event in case an event handler has been defined.
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);
            OnValidatingPassword(args);
            if (args.Cancel)
            {
                // Check for a specific error message.
                if (null != args.FailureInformation)
                {
                    throw (args.FailureInformation);
                }
                else
                {
                    throw (new MembershipPasswordException(ExceptionUtil.FormatExceptionMessage(this,
                        Resources.Pwd_ResetCancelledDueToNewPassword)));
                }
            }

            // Get the user from the data store.
            User user = NHibernateProviderEntityHelper.GetUser(username);
            if (null != user)
            {
                // Determine if the user is locked out of the system.
                if (user.IsLockedOut)
                {
                    throw (new MembershipPasswordException(ExceptionUtil.FormatExceptionMessage(this, Resources.User_IsLockedOut)));
                }

                // Update user record with the new password.
                try
                {
                    user.Password = EncodePassword(newPassword, user.PasswordSalt);
                    user.LastPasswordChangeDate = DateTime.Now;
                    user.LastActivityDate = DateTime.Now;
                    NHibernateHelper.SaveOrUpdate(user);
                }
                catch
                {
                    throw (new MembershipPasswordException(
                        ExceptionUtil.FormatExceptionMessage(this, Resources.Pwd_OpCancelledDueToAccountLocked)));
                }
            }

            // Return the resulting new password.
            return newPassword;
        }

        #endregion Password

        #endregion Operations

        #region Helpers
        /// <summary>
        /// Update the login date for the given user name.
        /// </summary>
        /// <param name="username">name of the user for whom to update the last login date.</param>
        private void UpdateLastLoginDate(string username)
        {
            // Get user record associated to the given user name.
            User user = NHibernateProviderEntityHelper.GetUser(username);
            if (null != user)
            {
                // Update the last login timestamp (LastLoginDate).
                try
                {
                    // Perform the update in the data store.
                    user.LastLoginDate = DateTime.Now;
                    NHibernateHelper.SaveOrUpdate(user);
                }
                catch (Exception ex)
                {
                    throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToUpdateLastLoginDate, ex);
                }
            }
        }
        /// <summary>
        /// Update the activity date for the given user name.
        /// </summary>
        /// <param name="username">name of the user for whom to update the activity date.</param>
        private void UpdateLastActivityDate(string username)
        {
            // Get user record associated to the given user name.
            User user = NHibernateProviderEntityHelper.GetUser(username);
            if (null != user)
            {
                // Update the activity timestamp (LastActivityDate).
                try
                {
                    // Perform the update in the data store.
                    user.LastActivityDate = DateTime.Now;
                    NHibernateHelper.SaveOrUpdate(user);
                }
                catch (Exception ex)
                {
                    throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToUpdateLastActivityDate, ex);
                }
            }
        }
        /// <summary>
        /// A helper method that performs the checks and updates associated with password failure tracking.
        /// </summary>
        /// <param name="username">name of the user that is failing to specify a valid password.</param>
        /// <param name="failureType">type of failure to record.</param>
        private void UpdateFailureCount(string username, FailureType failureType)
        {
            // Get user record associated to the given user name.
            User user = NHibernateProviderEntityHelper.GetUser(username);
            if (null != user)
            {
                // Update the failure information for the given user in the data store.
                DateTime windowStart = DateTime.Now;
                int failureCount = 0;
                try
                {
                    // First determine the type of update we need to do and get the relevant details.
                    switch (failureType)
                    {
                        case FailureType.Password:
                            windowStart = user.FailedPasswordAttemptWindowStart;
                            failureCount = user.FailedPasswordAttemptCount;
                            break;
                        case FailureType.PasswordAnswer:
                            windowStart = user.FailedPasswordAnswerAttemptWindowStart;
                            failureCount = user.FailedPasswordAnswerAttemptCount;
                            break;
                    }

                    // Then determine if the threashold has been exeeded.
                    DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);
                    if ((0 == failureCount) || DateTime.Now > windowEnd)
                    {
                        // First password failure or outside of window, start new password failure count from 1
                        // and a new window start.
                        switch (failureType)
                        {
                            case FailureType.Password:
                                user.FailedPasswordAttemptWindowStart = DateTime.Now;
                                user.FailedPasswordAttemptCount = 1;
                                break;
                            case FailureType.PasswordAnswer:
                                user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
                                user.FailedPasswordAnswerAttemptCount = 1;
                                break;
                        }
                    }
                    else
                    {
                        // Track failures.
                        failureCount++;
                        if (failureCount >= MaxInvalidPasswordAttempts)
                        {
                            // Password attempts have exceeded the failure threshold. Lock out the user.
                            user.IsLockedOut = true;
                            user.LastLockedOutDate = DateTime.Now;
                        }
                        else
                        {
                            switch (failureType)
                            {
                                case FailureType.Password:
                                    user.FailedPasswordAttemptCount = failureCount;
                                    break;
                                case FailureType.PasswordAnswer:
                                    user.FailedPasswordAnswerAttemptCount = failureCount;
                                    break;
                            }
                        }
                    }

                    // Persist the changes.
                    NHibernateHelper.SaveOrUpdate(user);
                }
                catch (Exception ex)
                {
                    throw ExceptionUtil.NewProviderException(this, Resources.User_UnableToUpdateFailureCount, ex);
                }
            }
        }
        /// <summary>
        /// Compares password values based on the <see cref="MembershipPasswordFormat"/> property value.
        /// </summary>
        /// <param name="password1">first password to compare.</param>
        /// <param name="password2">second password to compare</param>
        /// <param name="validationKey">key to use when encoding the password.</param>
        /// <returns></returns>
        private bool CheckPassword(string password1, string password2, string validationKey)
        {
            // Format the password as required for comparison.
            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Hashed:
                    password1 = EncodePassword(password1, validationKey);
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password2 = UnencodePassword(password2);
                    break;
            }

            // Return the result of the comparison.
            return (password1 == password2);
        }
        /// <summary>
        /// Encrypts, Hashes, or leaves the password clear based on the <see cref="PasswordFormat"/> property value.
        /// </summary>
        /// <param name="password">the password to encode.</param>
        /// <param name="validationKey">key to use when encoding the password.</param>
        /// <returns>
        /// The encoded password only if all parameters are specified. If <c>validationKey</c> is <c>null</c>
        /// then the given <c>password</c> is returned untouched.
        /// </returns>
        private string EncodePassword(string password, string validationKey)
        {
            // Assume no encoding is performed.
            string encodedPassword = password;

            // Only perform the encoding if all parameters are passed and valid.
            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(validationKey))
            {
                // Determine the type of encoding required.
                switch (PasswordFormat)
                {
                    case MembershipPasswordFormat.Clear:
                        // Nothing to do.
                        break;
                    case MembershipPasswordFormat.Encrypted:
                        encodedPassword = Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                        break;
                    case MembershipPasswordFormat.Hashed:
                        // If we are not password a validation key, use the default specified.
                        if (string.IsNullOrEmpty(validationKey))
                        {
                            // The machine key will either come from the Web.config file or it will be automatically generate
                            // during initialization.
                            validationKey = machineKey.ValidationKey;
                        }
                        HMACSHA1 hash = new HMACSHA1();
                        hash.Key = HexToByte(validationKey);
                        encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                        break;
                    default:
                        throw ExceptionUtil.NewProviderException(this, Resources.Pwd_UnsupportedFormat);
                }
            }

            // Return the encoded password.
            return encodedPassword;
        }
        /// <summary>
        /// Decrypts or leaves the password clear based on the <see cref="PasswordFormat"/> property value.
        /// </summary>
        /// <param name="password">password to unencode.</param>
        /// <returns>Unencoded password.</returns>
        private string UnencodePassword(string password)
        {
            // Assume no unencoding is performed.
            string unencodedPassword = password;

            // Determine the type of unencoding required.
            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    // Nothing to do.
                    break;
                case MembershipPasswordFormat.Encrypted:
                    unencodedPassword = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(unencodedPassword)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw ExceptionUtil.NewProviderException(this, Resources.Pwd_CannotUnencodeHashed);
                default:
                    throw ExceptionUtil.NewProviderException(this, Resources.Pwd_UnsupportedFormat);
            }

            // Return the unencoded password.
            return unencodedPassword;
        }
        /// <summary>
        /// Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration.
        /// </summary>
        /// <param name="hexString">hexadecimal string to conver.</param>
        /// <returns><c>byte</c> array containing the converted hexadecimal string contents.</returns>
        private static byte[] HexToByte(string hexString)
        {
            byte[] bytes = new byte[hexString.Length/2 + 1];
            for (int i = 0; i <= hexString.Length/2 - 1; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i*2, 2), 16);
            }
            return bytes;
        }
        #endregion Helpers
    }
}