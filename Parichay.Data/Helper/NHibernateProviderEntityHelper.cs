using System;
using System.Collections;
using NHibernate;
using Parichay.Data.Entity;
using Parichay.Data.Properties;
using Parichay.Data.Util;

namespace Parichay.Data.Helper
{
    /// <summary>
    /// Helper class to provide persistence logic for some re-useable operations.
    /// This helper class has been taken from http: //www.codeproject.com/KB/dotnet/NhibernateProviders.aspx 
    /// and customized for compatibility with Parichay.Data
    /// </summary>
    internal static class EntityHelper
    {

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

        

        

        #region Permissions
        /// <summary>
        /// Utility Method for parsing of Generic IList of type "T" from IList
        /// </summary>
        /// <typeparam name="T">Type of Return object Lists expected</typeparam>
        /// <param name="iList">Input Ilist</param>
        /// <returns></returns>
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