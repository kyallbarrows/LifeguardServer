using System;
using System.Collections;
using System.Collections.Specialized;
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
    /// Provides role services using NHibernate as the persistence mechanism.
    /// </summary>
    public class NHibernateRoleProvider : RoleProvider
    {
        #region Fields
        private Application application = new Application();
        #endregion Fields

        #region Properties
        /// <summary>
        /// The name of the application using the custom role provider.
        /// </summary>
        /// <returns>
        /// The name of the application using the custom role provider.
        /// </returns>
        public override string ApplicationName
        {
            get { return application.Name; }
            set { application.Name = value; }
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
                name = "NHibernateRoleProvider";
            }
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "NHibernate Role Provider");
            }

            // Call the base class implementation.
            base.Initialize(name, config);

            // Load configuration data.
            application =
                NHibernateProviderEntityHelper.CreateOrLoadApplication(
                    ConfigurationUtil.GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath));
        }
        #endregion Initialization

        #region Operations
        /// <summary>
        /// Adds a new role to the data source for the configured application name.
        /// </summary>
        /// <param name="roleName">the name of the role to create.</param>
        public override void CreateRole(string roleName)
        {
            // Make sure we are not attempting to insert an existing role.
            if (RoleExists(roleName))
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Role_AlreadyExists);
            }

            try
            {
                Role role = new Role();
                role.Name = roleName;
                role.Applications.Add(application);
                NHibernateHelper.SaveOrUpdate(role);
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Role_UnableToCreate, ex);
            }
        }
        /// <summary>
        /// Removes a role from the data source for the configured application name.
        /// </summary>
        /// <param name="roleName">the name of the role to delete.</param>
        /// <param name="throwOnPopulatedRole">if <c>true</c>, throw an exception if <c>roleName</c> has one or more
        /// members and do not delete <c>roleName</c>.</param>
        /// <returns><c>true</c> if the role was successfully deleted; otherwise, <c>false</c>.</returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            // Assume we are unable to perform the operation.
            bool result = false;

            // Check to see if we need to throw an exception if roles have been linked to other objects.
            if (throwOnPopulatedRole && (0 < GetUsersInRole(roleName).Length))
            {
                // Indicate the role is not empty and cannot be removed.
                throw ExceptionUtil.NewProviderException(this, "role is not empty.");
            }

            // Remove role information from the data store.
            try
            {
                // Get the role information.
                Role role = NHibernateProviderEntityHelper.GetRole(roleName);
                if (null != role)
                {
                    // Delete the references to applications/roles.
                    object[] values = new object[] { application.Id, role.Id };
                    IType[] types = new IType[] { NHibernateUtil.Int32, NHibernateUtil.Int32 };
                    NHibernateHelper.DeleteByNamedQuery("ApplicationUserRole.RemoveAppRoleReferences", values, types);
                    // Delete the role record.
                    NHibernateHelper.Delete(role);
                    // Indicate no errors occured.
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Role_UnableToDelete, ex);
            }

            // Return the result of the operation.
            return result;
        }
        /// <summary>
        /// Gets a value indicating whether the specified role name already exists in the data store
        /// for the configured application name.
        /// </summary>
        /// <param name="roleName">the name of the role to search for.</param>
        /// <returns>
        /// <c>true</c> if the role name already exists in the data store for the configured application name;
        /// otherwise, <c>false</c>.
        ///</returns>
        public override bool RoleExists(string roleName)
        {
            // Assume the role does not exist.
            bool exists;

            // Check against the data store if the role exists.
            try
            {
                exists = (0 < NHibernateHelper.CountByNamedQuery("Role.ByLoweredName", roleName.ToLower(), NHibernateUtil.String));
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Role_UnableToCheckIfExists, ex);
            }

            // Return the result of the operation.
            return exists;
        }
        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured application name.
        /// </summary>
        /// <param name="username">the name of the user to search for.</param>
        /// <param name="roleName">the name of the role to search in.</param>
        /// <returns>
        /// <c>true</c> if the specified user is in the specified role for the configured application name;
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            // Assume the given role is not associated to the given user.
            bool isInRole = false;

            // Check against the data store if the role has been assigned to the given user.
            try
            {
                User user = NHibernateProviderEntityHelper.GetUser(username);
                Role role = NHibernateProviderEntityHelper.GetRole(roleName);
                if ((null != user) && (null != role))
                {
                    object[] values = new object[] {application.Id, user.Id, role.Id};
                    IType[] types = new IType[] {NHibernateUtil.Int32, NHibernateUtil.Int32, NHibernateUtil.Int32};
                    isInRole = (0 < NHibernateHelper.CountByNamedQuery("ApplicationUserRole.IsUserInRole", values, types));
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Role_UnableToFindUserInRole, ex);
            }

            // Return the result of the operation.
            return isInRole;
        }
        /// <summary>
        /// Gets a list of the roles that a specified user is in for the configured application name.
        /// </summary>
        /// <param name="username">the name of the user for whom to return a list of roles.</param>
        /// <returns>
        /// A string array containing the names of all the roles that the specified user is in for
        /// the configured application name.
        /// </returns>
        public override string[] GetRolesForUser(string username)
        {
            // Prepare a placeholder for the roles.
            string[] roleNames = new string[0];

            // Load the list from the data store.
            try
            {
                User user = NHibernateProviderEntityHelper.GetUser(username);
                object[] values = new object[] {application.Id, user.Id};
                IType[] types = new IType[] {NHibernateUtil.Int32, NHibernateUtil.Int32};
                IList roles = NHibernateHelper.FindByNamedQuery("ApplicationUserRole.GetRoleNamesForUser", values, types);
                if (null != roles)
                {
                    roleNames = new string[roles.Count];
                    for (int i = 0; i < roles.Count; i++)
                    {
                        roleNames[i] = roles[i].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Role_UnableToGetRolesForUser, ex);
            }

            // Return the result of the operation.
            return roleNames;
        }
        /// <summary>
        /// Adds the specified user names to the specified roles for the configured application name.
        /// </summary>
        /// <param name="roleNames">A string array of the role names to add the specified user names to. </param>
        /// <param name="userNames">A string array of user names to be added to the specified roles. </param>
        public override void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            // Add the users to the given roles.
            try
            {
                // For every user in the given list attempt to add to the given roles.
                foreach (string userName in userNames)
                {
                    // Assume that the given user name will be found. If any is not found this call will fail.
                    User user = NHibernateProviderEntityHelper.GetUser(userName);
                    // Each role must be added from the user being currently processed. The assumption is that
                    // the same list of roles will apply to all given users.
                    foreach (string roleName in roleNames)
                    {
                        // Assume that the given user name will be found. If any is not found this call will fail.
                        Role role = NHibernateProviderEntityHelper.GetRole(roleName);
                        // NOTE: To ensure this relationship is stored we must use Save and not SaveOrUpdate.
                        ApplicationUserRole appUserRole = new ApplicationUserRole(application, user, role);
                        NHibernateHelper.Save(appUserRole);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Role_UnableToAddUsersToRoles, ex);
            }
        }
        /// <summary>
        /// Removes the specified user names from the specified roles for the configured application name.
        /// </summary>
        /// <param name="userNames">string array of user names to be removed from the specified roles.</param>
        /// <param name="roleNames">string array of role names from which to remove the specified user names.</param>
        public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            // Remove the users from the given roles.
            try
            {
                // For every user in the given list attempt to remove the given roles.
                foreach (string userName in userNames)
                {
                    // Assume that the given user name will be found. If any is not found this call will fail.
                    User user = NHibernateProviderEntityHelper.GetUser(userName);
                    // Each role must be attempted to be removed from the user being currently processed. If no
                    // association is found, ignore it.
                    foreach (string roleName in roleNames)
                    {
                        // Assume that the given user name will be found. If any is not found this call will fail.
                        Role role = NHibernateProviderEntityHelper.GetRole(roleName);
                        // Execute the delete operation.
                        object[] values = new object[] {application.Id, user.Id, role.Id};
                        IType[] types = new IType[] {NHibernateUtil.Int32, NHibernateUtil.Int32, NHibernateUtil.Int32};
                        NHibernateHelper.DeleteByNamedQuery("ApplicationUserRole.RemoveUserFromRole", values, types);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Role_UnableToRemoveUsersFromRoles, ex);
            }
        }
        /// <summary>
        /// Gets a list of users in the specified role for the configured application name.
        /// </summary>
        /// <param name="roleName">the name of the role for which to get the list of users.</param>
        /// <returns>
        /// A string array containing the names of all the users who are members of the specified role
        /// for the configured application name.
        /// </returns>
        public override string[] GetUsersInRole(string roleName)
        {
            // Prepare a placeholder for the roles.
            string[] userNames = new string[0];

            // Load the list from the data store.
            try
            {
                Role role = NHibernateProviderEntityHelper.GetRole(roleName);
                object[] values = new object[] {application.Id, role.Id};
                IType[] types = new IType[] {NHibernateUtil.Int32, NHibernateUtil.Int32};
                IList users = NHibernateHelper.FindByNamedQuery("ApplicationUserRole.GetUserNamesInRole", values, types);
                if (null != users)
                {
                    userNames = new string[users.Count];
                    for (int i = 0; i < users.Count; i++)
                    {
                        userNames[i] = users[i].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Role_UnableToGetUsersInRole, ex);
            }

            // Return the result of the operation.
            return userNames;
        }
        /// <summary>
        /// Gets a list of all the roles for the configured application name.
        /// </summary>
        /// <returns>
        /// A string array containing the names of all the roles stored in the data store for the configured application name.
        /// </returns>
        public override string[] GetAllRoles()
        {
            // Prepare a placeholder for the roles.
            string[] roleNames = new string[0];

            // Load the list of roles for the configured application name.
            try
            {
                IList list = NHibernateHelper.FindByNamedQuery("ApplicationRole.GetRoleNames", application.Id, NHibernateUtil.Int32);
                if (0 < list.Count)
                {
                    roleNames = new string[list.Count];
                    int i = 0;
                    foreach (string roleName in list)
                    {
                        roleNames[i++] = roleName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Role_UnableToGetAllRoles, ex);
            }

            // Return the result of the operation.
            return roleNames;
        }
        /// <summary>
        /// Gets an array of user names in a role where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="roleName">the name of the role to search in.</param>
        /// <param name="usernameToMatch">the user name to search for.</param>
        /// <returns>
        /// A string array containing the names of all the users where the user name matches <c>usernameToMatch</c>
        /// and the user is a member of the specified role.
        /// </returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            // Prepare a placeholder for the users.
            string[] userNames = new string[0];

            // Load the list of users for the given role name.
            try
            {
                // Replace all * and ? wildcards for % and _, respectively.
                usernameToMatch = usernameToMatch.Replace('*', '%');
                usernameToMatch = usernameToMatch.Replace('?', '_');

                // Perform the search.
                Role role = NHibernateProviderEntityHelper.GetRole(roleName);
                object[] values = new object[] {application.Id, role.Id, usernameToMatch.ToLower()};
                IType[] types = new IType[] {NHibernateUtil.Int32, NHibernateUtil.Int32, NHibernateUtil.String};
                IList users =
                    NHibernateHelper.FindByNamedQuery("ApplicationUserRole.FindUserNamesInRoleByUserLoweredName", values, types);
                if (null != users)
                {
                    userNames = new string[users.Count];
                    for (int i = 0; i < users.Count; i++)
                    {
                        userNames[i] = users[i].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, Resources.Role_UnableToFindUsersInRole, ex);
            }

            // Return the result of the operation.
            return userNames;
        }
        #endregion Operations
    }
}