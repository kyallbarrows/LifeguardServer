using System;
using System.Collections;
using NHibernate;
using Parichay.Security.Entity;
using Parichay.Security.Properties;
using Parichay.Security.Util;

namespace Parichay.Security.Helper
{
    /// <summary>
    /// Helper class to provide persistence logic for some re-useable operations.
    /// </summary>
    internal static class NHibernateProviderEntityHelper
    {
        #region Application
        /// <summary>
        /// Returns a flag indicating whehter or not the application record already exists in the data store.
        /// </summary>
        /// <param name="appName">name of the application for which to get the details.</param>
        /// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
        public static bool ApplicationExists(string appName)
        {
            // Assume the application does not exist.
            bool exists;

            // Determine if the application record exists in the data store.
            try
            {
                exists = (0 <
                    NHibernateHelper.FindByNamedQuery("Application.ByLoweredName", appName.ToLower(), NHibernateUtil.String).Count);
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(Resources.App_UnableToCheckExists, ex);
            }

            // Return the result of the operation.
            return exists;
        }
        /// <summary>
        /// Determines whether or not the given application name has already been registered; if not; creates the corresponding
        /// application instance.
        /// </summary>
        /// <param name="appName">name of the application for which to get the details.</param>
        /// <returns><see cref="Application"/> instance representing the given application name.</returns>
        public static Application CreateOrLoadApplication(string appName)
        {
            // Prepare a place-holder for the application.
            Application app = GetApplication(appName);

            // Determine if the application record does not exists in the data store.
            if (null == app)
            {
                try
                {
                    // Create a new application instance.
                    app = new Application(appName);
                    // Update it in the data store.
                    NHibernateHelper.Save(app);
                }
                catch (Exception ex)
                {
                    throw ExceptionUtil.NewProviderException(Resources.App_UnableToCreateOrLoad, ex);
                }
            }

            // Return the resulting application instance.
            return app;
        }
        /// <summary>
        /// Gets <see cref="Application"/> instance for the given application name.
        /// </summary>
        /// <param name="appName">name of the application for whom to retrieve information.</param>
        /// <returns><see cref="Application"/> object representing the given application name; otherwise, <c>null</c>.</returns>
        public static Application GetApplication(string appName)
        {
            // Assume we were unable to find the application.
            Application app = null;

            // Get the application record from the data store.
            try
            {
                IList apps =
                    NHibernateHelper.FindByNamedQuery("Application.ByLoweredName", appName.ToLower(), NHibernateUtil.String);
                if (1 == apps.Count)
                {
                    app = apps[0] as Application;
                }
                else if (1 < apps.Count)
                {
                    throw ExceptionUtil.NewProviderException(Resources.App_TooManyMatching);
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(Resources.App_UnableToGet, ex);
            }

            // Return the resulting application.
            return app;
        }
        #endregion Application

        #region User
        /// <summary>
        /// Gets <see cref="User"/> instance for the given user name.
        /// </summary>
        /// <param name="username">name of the user for whom to retrieve information.</param>
        /// <returns><see cref="User"/> object representing the given user name; otherwise, <c>null</c>.</returns>
        public static User GetUser(string username)
        {
            // Assume we were unable to find the user.
            User user = null;

            // Get the user record from the data store.
            try
            {
                IList users = NHibernateHelper.FindByNamedQuery("User.ByLoweredName", username.ToLower(), NHibernateUtil.String);
                if (1 == users.Count)
                {
                    user = users[0] as User;
                }
                else if (1 < users.Count)
                {
                    throw ExceptionUtil.NewProviderException(Resources.User_TooManyMatching);
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(Resources.User_UnableToGet, ex);
            }

            // Return the resulting user.
            return user;
        }
        
        /// <summary>
        /// Gets <see cref="User"/> instance for the given user name.
        /// </summary>
        /// <param name="username">name of the user for whom to retrieve information.</param>
        /// <returns><see cref="User"/> object representing the given user name; otherwise, <c>null</c>.</returns>
        public static User GetUser(int id)
        {
            // Assume we were unable to find the user.
            User user = null;

            // Get the user record from the data store.
            try
            {
                IList users = NHibernateHelper.FindByNamedQuery("User.ById", id, NHibernateUtil.Int32);
                if (1 == users.Count)
                {
                    user = users[0] as User;
                }
                else if (1 < users.Count)
                {
                    throw ExceptionUtil.NewProviderException(Resources.User_TooManyMatching);
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(Resources.User_UnableToGet, ex);
            }

            // Return the resulting user.
            return user;
        }
        /// <summary>
        /// Returns a flag indicating whehter or not the user record already exists in the data store.
        /// </summary>
        /// <param name="username">name of the user to verify.</param>
        /// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
        public static bool UserExists(string username)
        {
            // Assume the user does not exist.
            bool exists;

            // Determine if the user record exists in the data store.
            try
            {
                exists = (0 <
                    NHibernateHelper.FindByNamedQuery("User.ByLoweredName", username.ToLower(), NHibernateUtil.String).Count);
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(Resources.User_UnableToCheckIfExists, ex);
            }

            // Return the result of the operation.
            return exists;
        }
        #endregion User

        #region Role
        /// <summary>
        /// Gets <see cref="Role"/> instance for the given role name.
        /// </summary>
        /// <param name="roleName">name of the role for whom to retrieve information.</param>
        /// <returns><see cref="Role"/> object representing the given role name; otherwise, <c>null</c>.</returns>
        public static Role GetRole(string roleName)
        {
            // Assume we were unable to find the role.
            Role role = null;

            // Get the role record from the data store.
            try
            {
                IList roles = NHibernateHelper.FindByNamedQuery("Role.ByLoweredName", roleName.ToLower(), NHibernateUtil.String);
                if (1 == roles.Count)
                {
                    role = roles[0] as Role;
                }
                else if (1 < roles.Count)
                {
                    throw ExceptionUtil.NewProviderException(Resources.Role_TooManyMatching);
                }
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(Resources.Role_UnableToGet, ex);
            }

            // Return the resulting role.
            return role;
        }
        #endregion Role

        #region Profile
        /// <summary>
        /// Gets <see cref="Profile"/> instance for the given user name.
        /// </summary>
        /// <param name="username">name of the user for whom to retrieve information.</param>
        /// <returns><see cref="Profile"/> object representing the given user name; otherwise, <c>null</c>.</returns>
        public static Profile GetProfile(string username)
        {
            Profile p = null;
            if (string.IsNullOrEmpty(username)) return null;

            try
            {
                User user = NHibernateProviderEntityHelper.GetUser(username);

                IList ps = NHibernateHelper.FindByNamedQuery("Profile.ByUserId", user.Id, NHibernateUtil.Int32);
                if(ps.Count>0)
                    p = (Profile)ps[0]; 
            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException("GetProfile", ex);
            }

            // Return the resulting user.
            return p;
        }
        #endregion

        #region Permissions
        public static System.Collections.Generic.IList<T> ConvertToListOf<T>(IList iList)
        {
            System.Collections.Generic.IList<T> result = new System.Collections.Generic.List<T>();
            foreach (T value in iList)
                result.Add(value);

            return result;
        }
        #endregion
    }
}