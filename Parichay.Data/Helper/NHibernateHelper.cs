using System.Collections;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Type;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace Parichay.Data.Helper
{
    /// <summary>
    /// Helper class to provide a consistent wrapper to perform NHibernate operations.
    /// This helper class has been taken from http: //www.codeproject.com/KB/dotnet/NhibernateProviders.aspx 
    /// and customized for compatibility with Parichay.Data
    /// </summary>
    public static class NHibernateHelper
    {
        public enum LogType { Debug, Error, Fatal, Info, Warn }

        #region Constants
        private const string CurrentSessionKey = "nhibernate.current_session.NHibernateHelper";
        #endregion Constants

        #region Static Fields
        private static readonly ISessionFactory sessionFactory;
        #endregion Static Fields

        #region Initialization
        static NHibernateHelper()
        {
            System.Reflection.Assembly thisAssembly = typeof(Entity.User).Assembly;
            Configuration cfg = new Configuration();
            cfg.Configure().AddAssembly(thisAssembly);

            ////Schema Export
            //new NHibernate.Tool.hbm2ddl.SchemaUpdate(cfg).Execute(false, true); 

            sessionFactory = cfg.BuildSessionFactory();
        }
        #endregion Initialization

        #region Session Operations
        public static ISession GetCurrentSession()
        {
            ISession currentSession = null;
            HttpContext context = HttpContext.Current;
            if (null != context)
            {
                currentSession = context.Items[CurrentSessionKey] as ISession;
            }
            if (null == currentSession)
            {
                lock (sessionFactory)
                {
                    currentSession = sessionFactory.OpenSession();
                }
            }
            if (null != context)
            {
                context.Items[CurrentSessionKey] = currentSession;
            }
            return currentSession;
        }
        public static void CloseSession(ISession currentSession)
        {
            HttpContext context = HttpContext.Current;
            if ((null == currentSession) && null != context)
            {
                currentSession = context.Items[CurrentSessionKey] as ISession;
            }
            if (null != currentSession)
            {
                currentSession.Close();
                if (null != context)
                {
                    context.Items.Remove(CurrentSessionKey);
                }
            }
        }
        public static void CloseSessionFactory()
        {
            lock (sessionFactory)
            {
                if (null != sessionFactory)
                {
                    sessionFactory.Close();
                }
            }
        }
        #endregion Session Operations

        #region Persistence Operations
        public static void Save(object obj)
        {
            ISession s = GetCurrentSession();
            ITransaction tx = null;
            try
            {
                tx = s.BeginTransaction();
                s.Save(obj);
                tx.Commit();
            }
            catch
            {
                if (null != tx) tx.Rollback();
                throw;
            }
            finally
            {
                CloseSession(s);
            }
        }
        public static void Update(object obj)
        {
            ISession s = GetCurrentSession();
            ITransaction tx = null;
            try
            {
                tx = s.BeginTransaction();
                s.Update(obj);
                tx.Commit();
            }
            catch
            {
                if (null != tx) tx.Rollback();
                throw;
            }
            finally
            {
                CloseSession(s);
            }
        }
        public static void SaveOrUpdate(object obj)
        {
            ISession s = GetCurrentSession();
            ITransaction tx = null;
            try
            {
                tx = s.BeginTransaction();
                s.SaveOrUpdate(obj);
                tx.Commit();
            }
            catch
            {
                if (null != tx) tx.Rollback();
                throw;
            }
            finally
            {
                CloseSession(s);
            }
        }
        public static void Delete(object obj)
        {
            ISession s = GetCurrentSession();
            ITransaction tx = null;
            try
            {
                tx = s.BeginTransaction();
                s.Delete(obj);
                tx.Commit();
            }
            catch
            {
                if (null != tx) tx.Rollback();
                throw;
            }
            finally
            {
                CloseSession(s);
            }
        }

        public static void DeleteById(System.Type type, object value)
        {
            ISession s = GetCurrentSession();
            ITransaction tx = null;
            try
            {
                tx = s.BeginTransaction();
                s.Delete(s.Load(type,value));
                tx.Commit();
            }
            catch
            {
                if (null != tx) tx.Rollback();
                throw;
            }
            finally
            {
                CloseSession(s);
            }
        }
        public static int Delete(string queryString, object value, IType type)
        {
            object[] values = new object[] { value };
            IType[] types = new IType[] { type };
            return Delete(queryString, values, types);
        }
        public static int Delete(string queryString, object[] values, IType[] types)
        {
            int result;

            ISession s = GetCurrentSession();
            ITransaction tx = null;
            try
            {
                tx = s.BeginTransaction();
                result = s.Delete(queryString, values, types);
                tx.Commit();
            }
            catch
            {
                if (null != tx) tx.Rollback();
                throw;
            }
            finally
            {
                CloseSession(s);
            }

            return result;
        }
        public static int DeleteByNamedQuery(string queryName, object value, IType type)
        {
            object[] values = new object[] { value };
            IType[] types = new IType[] { type };
            return DeleteByNamedQuery(queryName, values, types);
        }
        public static int DeleteByNamedQuery(string queryName, object[] values, IType[] types)
        {
            int result;

            ISession s = GetCurrentSession();
            ITransaction tx = null;
            try
            {
                tx = s.BeginTransaction();
                IQuery query = s.GetNamedQuery(queryName);
                result = s.Delete(query.QueryString, values, types);
                tx.Commit();
            }
            catch
            {
                if (null != tx) tx.Rollback();
                throw;
            }
            finally
            {
                CloseSession(s);
            }

            return result;
        }
        public static IList Find(string queryString)
        {
            IList results;

            ISession s = GetCurrentSession();
            try
            {
                results = s.CreateQuery(queryString).List();
            }
            finally
            {
                CloseSession(s);
            }

            return results;
        }
        public static IList Find(string queryString, object value, IType type, bool isNamedQuery)
        {
            object[] values = new object[] { value };
            IType[] types = new IType[] { type };
            return Find(queryString, values, types,isNamedQuery);
        }
        public static IList Find(string queryString, object[] values, IType[] types, bool isNamedQuery)
        {
            IList results;

            ISession s = GetCurrentSession();
            try
            {
                IQuery query;
                if(isNamedQuery)
                    query = s.GetNamedQuery(queryString);
                else
                    query = s.CreateQuery(queryString);

                if ((null != values) && (null != types))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        query.SetParameter(i, values[i], types[i]);
                    }
                }
                results = query.List();
            }
            finally
            {
                CloseSession(s);
            }

            return results;
        }

        /// <summary>
        /// Retrieves Paged object list of a custom type on the basis of Custom Query
        /// </summary>
        /// <param name="queryString">Custom Query</param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList Find(string queryString, object value, NHibernate.Type.IType type, int pageIndex, int pageSize,bool isNamedQuery)
        {
            object[] values = new object[] { value };
            NHibernate.Type.IType[] types = new NHibernate.Type.IType[] { type };
            return (Find(queryString, values, types, pageIndex, pageSize,isNamedQuery));
        }

        /// <summary>
        /// Paged Results by Custom Query and multiple parameters
        /// </summary>
        /// <param name="queryString">Custom Query</param>
        /// <param name="values"></param>
        /// <param name="types"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IList Find(string queryString, object[] values, NHibernate.Type.IType[] types, int pageIndex, int pageSize, bool isNamedQuery)
        {
            IList results;

            using (ISession session = sessionFactory.OpenSession())
            {
                IQuery query;
                if (isNamedQuery)
                    query = session.GetNamedQuery(queryString);
                else
                    query = session.CreateQuery(queryString);

                if ((null != values) && (null != types))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        query.SetParameter(i, values[i], types[i]);
                    }
                }
                query.SetFirstResult(pageSize * pageIndex);
                query.SetMaxResults(pageSize);
                results = query.List();
            }

            return results;
        }

        /// <summary>
        /// Performs a search through a list parameter. Basically used in Where In clause. => Where item in (:value) <=
        /// </summary>
        /// <param name="queryString">Query String or Named Query</param>
        /// <param name="value">Value of the list</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList FindByListParameter(string queryString, string parameterName, object[] values, NHibernate.Type.IType type, bool isNamedQuery)
        {
            IList results;

            using (ISession session = sessionFactory.OpenSession())
            {
                IQuery query = (isNamedQuery) ? session.GetNamedQuery(queryString) : session.CreateQuery(queryString);
                query.SetParameterList(parameterName, values, type);
                results = query.List();
            }

            return results;
        }


        public static IList FindByNamedQuery(string queryName)
        {
            IList results;

            ISession s = GetCurrentSession();
            try
            {
                IQuery query = s.GetNamedQuery(queryName);
                results = query.List();
            }
            finally
            {
                CloseSession(s);
            }

            return results;
        }
        public static IList FindByNamedQuery(string queryName, object value, IType type)
        {
            object[] values = new object[] { value };
            IType[] types = new IType[] { type };
            return FindByNamedQuery(queryName, values, types);
        }
        public static IList FindByNamedQuery(string queryName, object[] values, IType[] types)
        {
            IList results;

            ISession s = GetCurrentSession();
            try
            {
                IQuery query = s.GetNamedQuery(queryName);
                if ((null != values) && (null != types))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        query.SetParameter(i, values[i], types[i]);
                    }
                }
                results = query.List();
            }
            finally
            {
                CloseSession(s);
            }

            return results;
        }
        public static IList FindPage(string queryString, int pageIndex, int pageSize)
        {
            IList results;

            ISession s = GetCurrentSession();
            try
            {
                IQuery q = s.CreateQuery(queryString);
                q.SetFirstResult(pageSize * pageIndex);
                q.SetMaxResults(pageSize);
                results = q.List();
            }
            finally
            {
                CloseSession(s);
            }

            return results;
        }
        public static IList FindPageByNamedQuery(string queryName, int pageIndex, int pageSize)
        {
            return FindPageByNamedQuery(queryName, null, null, pageIndex, pageSize);
        }
        public static IList FindPageByNamedQuery(string queryName, object value, IType type, int pageIndex, int pageSize)
        {
            object[] values = new object[] { value };
            IType[] types = new IType[] { type };
            return FindPageByNamedQuery(queryName, values, types, pageIndex, pageSize);
        }
        public static IList FindPageByNamedQuery(string queryName, object[] values, IType[] types, int pageIndex, int pageSize)
        {
            IList results;

            ISession s = GetCurrentSession();
            try
            {
                IQuery query = s.GetNamedQuery(queryName);
                if ((null != values) && (null != types))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        query.SetParameter(i, values[i], types[i]);
                    }
                }
                query.SetFirstResult(pageSize * pageIndex);
                query.SetMaxResults(pageSize);
                results = query.List();
            }
            finally
            {
                CloseSession(s);
            }

            return results;
        }

        public static int Count(string queryOrName, object value, NHibernate.Type.IType type,bool isNamedQuery)
        {
            object[] values = new object[] { value };
            NHibernate.Type.IType[] types = new NHibernate.Type.IType[] { type };
            return Count(queryOrName, values, types,isNamedQuery);
        }
        public static int Count(string queryOrName, object[] values, NHibernate.Type.IType[] types,bool isNamedQuery)
        {
            int result = 0;

            using (ISession session = sessionFactory.OpenSession())
            {
                IQuery query =  (isNamedQuery)?session.GetNamedQuery(queryOrName):session.CreateQuery(queryOrName);
                if ((null != values) && (null != types))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        query.SetParameter(i, values[i], types[i]);
                    }
                }
                IEnumerator e = query.Enumerable().GetEnumerator();
                while (e.MoveNext())
                {
                    result++;
                }
            }


            return result;
        }
        public static object UniqueResult(string queryString)
        {
            object result;

            ISession s = GetCurrentSession();
            try
            {
                result = s.CreateQuery(queryString).UniqueResult();
            }
            finally
            {
                CloseSession(s);
            }

            return result;
        }
        public static object UniqueResultByNamedQuery(string queryName, object value, IType type)
        {
            object[] values = new object[] { value };
            IType[] types = new IType[] { type };
            return UniqueResultByNamedQuery(queryName, values, types);
        }
        public static object UniqueResultByNamedQuery(string queryName, object[] values, IType[] types)
        {
            object result;

            ISession s = GetCurrentSession();
            try
            {
                IQuery query = s.GetNamedQuery(queryName);
                if ((null != values) && (null != types))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        query.SetParameter(i, values[i], types[i]);
                    }
                }
                result = query.UniqueResult();
            }
            finally
            {
                CloseSession(s);
            }

            return result;
        }
        public static int CountByNamedQuery(string queryName, object value, IType type)
        {
            object[] values = new object[] { value };
            IType[] types = new IType[] { type };
            return CountByNamedQuery(queryName, values, types);
        }
        public static int CountByNamedQuery(string queryName, object[] values, IType[] types)
        {
            int result = 0;

            ISession s = GetCurrentSession();
            try
            {
                IQuery query = s.GetNamedQuery(queryName);
                if ((null != values) && (null != types))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        query.SetParameter(i, values[i], types[i]);
                    }
                }
                IEnumerator e = query.Enumerable().GetEnumerator();
                while (e.MoveNext())
                {
                    result++;
                }
            }
            finally
            {
                CloseSession(s);
            }

            return result;
        }
        #endregion Persistence Operations

        #region CriteriaOperations
        /// <summary>
        /// Saves an object and its persistent children.
        /// </summary>
        public static void Save<T>(T item)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.Save(item);
                    session.Transaction.Commit();
                }
            }
        }
        /// <summary>
        /// Saves an object and its persistent children.
        /// </summary>
        public static void Update<T>(T item)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.Update(item);
                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Deletes an object of a specified type.
        /// </summary>
        /// <param name="itemsToDelete">The items to delete.</param>
        /// <typeparam name="T">The type of objects to delete.</typeparam>
        public static void Delete<T>(T item)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.Delete(item);
                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Please do not use with "null" fieldNames. Use Find instead. Retrieves projections of a specified type where a specified property equals a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <param name="fieldNames">Array of Field Names to be fetched. Null to fetch all fields.</param>
        /// <param name="propertyName">property name to be tested.</param>
        /// <param name="propertyValue">property value that property Name should be assigned.</param>
        /// <param name="pageIndex">Start Page Index</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="isSoftSearch">True to perform a "Like" search, false to perform an "Equal" search.</param>
        /// <returns></returns>
        public static IList<T> FetchProjection<T>(string[] fieldNames, string propertyName, object propertyValue, int? pageIndex, int? pageSize, bool isSoftSearch, string sortField, bool isAscSort)
        {
            object[] tmp2 = (propertyValue != null) ? new object[] { propertyValue } : null;
            return (FetchProjection<T>(fieldNames, new string[] { propertyName }, tmp2, pageIndex, pageSize, isSoftSearch, sortField, isAscSort));
        }

        /// <summary>
        /// Please do not use with "null" fieldNames. Use Find instead. Retrieves projections of a specified type where a specified property equals a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <param name="fieldNames">Array of Field Names to be fetched. Null to fetch all fields.</param>
        /// <param name="propertyNames">Array of property names to be tested.</param>
        /// <param name="propertyValues">Array of property values that property Names must hold.</param>
        /// <param name="pageIndex">Start Page Index.</param>
        /// <param name="pageSize">Page Size.</param>
        /// <param name="isSoftSearch">True to perform a "Like" search, false to perform an "Equal" search.</param>
        /// <param name="sortField">Field to be sorted upon.</param>
        /// <param name="isAscSort">True for ascending sort, false for Descending sort on the basis of sortField.</param> 
        /// <returns>A list of all objects meeting the specified criteria.</returns>
        public static IList<T> FetchProjection<T>(string[] fieldNames, string[] propertyNames, object[] propertyValues, int? pageIndex, int? pageSize, bool isSoftSearch, string sortField, bool isAscSort)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                // Create a criteria object with the specified criteria
                ICriteria criteria = session.CreateCriteria(typeof(T));

                //If parameters are provided, add the parameters with values
                if ((null != propertyValues) && (null != propertyNames))
                {
                    if (isSoftSearch)
                    {
                        for (int i = 0; i < propertyValues.Length; i++)
                        {
                                criteria = criteria.Add(Expression.Like(propertyNames[i], propertyValues[i]));
                        }
                    }
                    else
                    {
                        if (propertyValues.Length > 1)
                            criteria = criteria.Add(Restrictions.Disjunction());

                        for (int i = 0; i < propertyValues.Length; i++)
                        {
                            //  criteria = criteria.Add(Expression.Eq(propertyNames[i], propertyValues[i]));
                            criteria = criteria.Add(Restrictions.Eq(propertyNames[i], propertyValues[i]));
                        }
                    }

                    
                }

                //If Projection is sought, apply filter for projection
                if(null==fieldNames)
                {
                    throw new System.Exception("fieldNames should not be null for FetchProjection. Please use Find and then ConvertToListOf<T> instead of FetchProjection.");
                }
                else
                {
                    ProjectionList proj = Projections.ProjectionList();
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        proj.Add(Projections.Property(fieldNames[i]), fieldNames[i]);
                    }
                    criteria = criteria.SetProjection(proj);
                }

                if (null != sortField)
                    if (isAscSort)
                        criteria.AddOrder(Order.Asc(sortField));
                    else
                        criteria.AddOrder(Order.Desc(sortField));

                //Set Paging Option
                if(pageIndex.HasValue&&pageSize.HasValue)
                criteria = criteria.SetFirstResult(pageSize.Value * pageIndex.Value).SetMaxResults(pageSize.Value);
                //criteria.SetMaxResults(pageSize);

                System.Collections.Generic.IList<T> matchingObjects;

                criteria = criteria.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(T)));

                if (!isSoftSearch)
                {
                    //This procedure's working with strict search
                    // Get the matching objects
                    matchingObjects = criteria.List<T>();
                }
                else
                {
                    //Working with like search
                    matchingObjects = ConvertToListOf<T>(criteria.List());
                }

                // Set return value
                return matchingObjects;
            }
        }

        public static IList<T> ConvertToListOf<T>(IList iList)
        {
            IList<T> result = new List<T>();
            foreach (T value in iList)
                result.Add(value);

            return result;
        }

        /// <summary>
        /// Retrieves all objects of a given type.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <returns>A list of all objects of the specified type.</returns>
        public static IList<T> Find<T>()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                // Retrieve all objects of the type passed in
                ICriteria targetObjects = session.CreateCriteria(typeof(T));
                IList<T> itemList = targetObjects.List<T>();
                return itemList;
            }
        }

        /// <summary>
        /// Retrieves objects of a specified type where a specified property equals a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <param name="propertyName">The name of the property to be tested.</param>
        /// <param name="propertyValue">The value that the named property must hold.</param>
        /// <returns>A list of all objects meeting the specified criteria.</returns>
        public static IList<T> Find<T>(string propertyName, object propertyValue)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                // Create a criteria object with the specified criteria
                ICriteria criteria = session.CreateCriteria(typeof(T));
                criteria.Add(Expression.Eq(propertyName, propertyValue));

                // Get the matching objects
                IList<T> matchingObjects = criteria.List<T>();

                // Set return value
                return matchingObjects;
            }
        }

        /// <summary>
        /// Use null for fieldName only.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static T UniqueResult<T>(string fieldName, string propertyName, object propertyValue)
        {
            //string[] tmp=(!string.IsNullOrEmpty(fieldName))?new string[] { fieldName}:null;
            object[] tmp2 = (propertyValue != null) ? new object[] { propertyValue } : null;

            return (UniqueResult<T>(fieldName, new string[] { propertyName }, tmp2));
        }

        /// <summary>
        /// Use null for fieldName only
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldNames"></param>
        /// <param name="propertyNames"></param>
        /// <param name="propertyValues"></param>
        /// <returns></returns>
        public static T UniqueResult<T>(string fieldName, string[] propertyNames, object[] propertyValues)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                // Create a criteria object with the specified criteria
                ICriteria criteria = session.CreateCriteria(typeof(T));


                if ((null != propertyValues) && (null != propertyNames))
                {
                    for (int i = 0; i < propertyValues.Length; i++)
                    {
                        criteria = criteria.Add(Expression.Eq(propertyNames[i], propertyValues[i]));
                    }
                }

                if (!string.IsNullOrEmpty(fieldName))
                {
                    ProjectionList proj = Projections.ProjectionList();
                    proj.Add(Projections.Property(fieldName), fieldName);
                    criteria = criteria.SetProjection(proj);
                    object rtrn = criteria.UniqueResult();
                    if (rtrn != null)
                    {
                        object obj = (T)System.Activator.CreateInstance(typeof(T));
                        Helper.PropertyInspector.SetPropertyValue(ref obj, fieldName, rtrn);
                        return ((T)obj);
                    }
                    else
                    { return default(T); }
                }
                else
                {
                    return (T)criteria.UniqueResult();
                }

            }
        }

        #endregion

        #region Logging

        public static void Log(System.Exception ex)
        {
            try
            {
                string msg = FormatException(ex);

                Save<Entity.Log>(new Entity.Log() { Date = System.DateTime.Now, Level = "ERROR", Logger = "parichay", Message = msg, Thread = "0" });
            }
            catch
            {
                throw;
            }
        }

        public static void Log(object message, LogType logType)
        {
            try
            {
                Save<Entity.Log>(new Entity.Log() { Date = System.DateTime.Now, Level = logType.ToString(), Logger = "parichay", Message = message.ToString(), Thread = "0" });
            }
            catch
            {
                throw;
            }
            //try
            //{
            //    switch (logType)
            //    {

            //        case (LogType.Info):
            //            logger.Info(message);
            //            break;
            //        case (LogType.Debug):
            //            logger.Debug(message);
            //            break;
            //        case (LogType.Fatal):
            //            logger.Fatal(message);
            //            break;
            //        case (LogType.Warn):
            //            logger.Warn(message);
            //            break;
            //        case (LogType.Error):
            //            logger.Info(message);
            //            break;
            //        default:
            //            logger.Info(message);
            //            break;
            //    }
            //}
            //catch
            //{
            //    throw;
            //}
        }

        private static string FormatException(System.Exception ex)
        {
            string message = ex.Message;



            //Add the inner exception if present (showing only the first 50 characters of the first exception)
            if (ex.InnerException != null)
            {
                if (message.Length > 100)
                    message = message.Substring(0, 100);

                message += "...->" + ex.InnerException.Message + ": " + ex.InnerException.StackTrace;
            }
            else
                message = ex.Message + ": " + ex.StackTrace;

            if (message.Length >= 4000)
                message = message.Substring(0, 3999);

            return message;
        }

        #endregion
    }
}