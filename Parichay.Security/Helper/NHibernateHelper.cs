using System.Collections;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Type;

namespace Parichay.Security.Helper
{
    /// <summary>
    /// Helper class to provide a consistent wrapper to perform NHibernate operations.
    /// </summary>
    internal static class NHibernateHelper
    {
        #region Constants
        private const string CurrentSessionKey = "nhibernate.current_session.NHibernateHelper";
        #endregion Constants

        #region Static Fields
        private static readonly ISessionFactory sessionFactory;
        #endregion Static Fields

        #region Initialization
        static NHibernateHelper()
        {
            System.Reflection.Assembly thisAssembly = typeof(Entity.Application).Assembly;
            Configuration cfg = new Configuration();
            cfg.Configure().AddAssembly(thisAssembly);

            ////Schema Export
            //new NHibernate.Tool.hbm2ddl.SchemaExport(cfg).Execute(false, true, false, false); 
            
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
        public static IList Find(string queryString, object value, IType type)
        {
            object[] values = new object[] { value };
            IType[] types = new IType[] { type };
            return Find(queryString, values, types);
        }
        public static IList Find(string queryString, object[] values, IType[] types)
        {
            IList results;

            ISession s = GetCurrentSession();
            try
            {
                IQuery query = s.CreateQuery(queryString);
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

        public static T UniqueResult<T>(string[] fieldNames, string[] propertyNames, object[] propertyValues)
        {
            ISession session = GetCurrentSession();
            try
            {
                // Create a criteria object with the specified criteria
                ICriteria criteria = session.CreateCriteria(typeof(T));


                if ((null != propertyValues) && (null != propertyNames))
                {
                    for (int i = 0; i < propertyValues.Length; i++)
                    {
                        criteria = criteria.Add(NHibernate.Criterion.Expression.Eq(propertyNames[i], propertyValues[i]));
                    }
                }

                if (null != fieldNames)
                {
                    NHibernate.Criterion.ProjectionList proj = NHibernate.Criterion.Projections.ProjectionList();
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        proj.Add(NHibernate.Criterion.Projections.Property(fieldNames[i]), fieldNames[i]);
                    }
                    criteria = criteria.SetProjection(proj);
                }


                return (T)criteria.UniqueResult();


            }
            catch
            {
                return default(T);
            }
            finally
            {
                CloseSession(session);
            }
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
    }
}