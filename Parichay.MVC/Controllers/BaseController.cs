using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using System.Runtime.Serialization;
using Parichay.MVC.Models;

namespace Parichay.MVC.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        

        public BaseController()
        {
            
        }
        /// <summary>
        /// Gets a value indicating whether the currently visiting user is authenticated.
        /// </summary>
        /// <value>
        /// if user authenticated returns <c>true</c>; otherwise, <c>false</c>.
        /// </value>
        protected bool IsUserAuthenticated
        {
            //[DebuggerStepThrough]
            get { return HttpContext.User.Identity.IsAuthenticated; }
        }


        /// <summary>
        /// Gets the Complete IP address of current user. If the user is not authenticated it returns "Anonymous"
        /// </summary>
        /// <value>IP address of the current user.</value>
        protected string LoggedInUserIp
        {
            get
            {
                string ipstr = "";
                for (int tmp1 = 0; tmp1 <= System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).Length - 1; tmp1++)
                {
                    ipstr += System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).GetValue(tmp1).ToString() + " | ";
                }
                return (ipstr.TrimEnd(new char[] { '|', ' ' }));
            }
        }

        /// <summary>
        /// Gets the Membership User Key for the current user. If the user is not authenticated it returns "Anonymous"
        /// </summary>
        /// <value>The Membership UserKey of the current user.</value>
        protected int LoggedInUserKey
        {
            //[DebuggerStepThrough]
            get
            {

                int rtRn = 0;

                if (IsUserAuthenticated)
                {
                    rtRn = ((CustomIdentity)HttpContext.User.Identity).UserKey;

                    if (rtRn == 0)
                    { rtRn = Int32.Parse(Membership.GetUser(HttpContext.User.Identity.Name).ProviderUserKey.ToString()); }
                }

                return rtRn;
            }

        }

        /// <summary>
        /// Gets the name of the current user. If the user is not authenticated it returns "Anonymous"
        /// </summary>
        /// <value>The name of the current user.</value>
        protected string LoggedInUserName
        {
            //[DebuggerStepThrough]
            get
            {
                string rtRn = string.Empty;

                if (IsUserAuthenticated)
                {
                    rtRn = ((CustomIdentity)HttpContext.User.Identity).Name;

                    if (string.IsNullOrEmpty(rtRn.Trim()))
                    { rtRn = Membership.GetUser(HttpContext.User.Identity.Name).UserName; }
                }

                return rtRn;
            }
        }


        /// <summary>
        /// Gets the email of the current user. If the user is not authenticated it returns empty string.
        /// </summary>
        /// <value>The email of the current user.</value>
        protected string LoggedInUserEmail
        {
            //get { return IsUserAuthenticated ? Membership.GetUser(LoggedInUserName, true).Email : string.Empty; }
            get
            {
                string rtRn = string.Empty;

                if (IsUserAuthenticated)
                {
                    rtRn = ((CustomIdentity)HttpContext.User.Identity).Email;

                    if (string.IsNullOrEmpty(rtRn.Trim()))
                    { rtRn = Membership.GetUser(HttpContext.User.Identity.Name).Email; }
                }

                return rtRn;
            }
        }


        protected bool IsJsonRequest()
        {
            string requestedWith = Request.ServerVariables["HTTP_X_REQUESTED_WITH"] ?? string.Empty;
            return string.Compare(requestedWith, "XMLHttpRequest", true) == 0
                && Request.ContentType.ToLower().Contains("application/json");
        }

        protected void SendMail(string to, string subject, string body)
        {
            try
            {
                //********Remember to comment out SpecifiedPickupDirectory settings in Helpers.Email class once you configure your mail client********//

                Helpers.Email ml = new Helpers.Email(AppConstants.adminEmail, to, subject, body, false);
                ml.send();
                Parichay.Data.Helper.NHibernateHelper.Log("Email Sent=> " + ml.ToString(), Data.Helper.NHibernateHelper.LogType.Warn);
            }
            catch (Exception ex)
            {
                //throw;
                Data.Helper.NHibernateHelper.Log(new Exception("Error Sending Mail to:" + to.ToString(), ex));
            }

        }


        protected void Alert(int to, string message)
        {
            try
            {
                Data.Entity.MemberAlert obj = new Data.Entity.MemberAlert();
                obj.Ishidden = 0;
                obj.Message = message;
                obj.PUser = Data.Helper.NHibernateHelper.UniqueResult<Data.Entity.MemberDetails>(null, "Id", to);
                //obj.AlertType = new Alerttype() {Id=0 };
                Data.Helper.NHibernateHelper.Save<Data.Entity.MemberAlert>(obj);
                TempData["message"] = "Requested User Notified.";

            }
            catch (Exception ex1)
            {
                Data.Helper.NHibernateHelper.Log(new Exception("Error Notifying UserKey:" + to.ToString(), ex1));
                TempData["message"] = "Unable to Notify. Error: " + ex1.Message;

            }
        }


        #region Messaging helpers
        protected int[] getFriendIds(int userId)
        {

            System.Collections.IList lstIds = Data.Helper.NHibernateHelper.Find("select a.MemberDetails.Id , a.Friendid.Id from MemberFriends a where a.MemberDetails.Id=? or a.Friendid.Id=?", new object[] { LoggedInUserKey, LoggedInUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 }, false);

            if ((lstIds != null) && (lstIds.Count != 0))
            {
                List<int> lst = new List<int>();
                object[] pairIds = Data.Helper.PropertyInspector.ListToTypeArray(lstIds, typeof(Models.IntPair));
                foreach (Models.IntPair item in pairIds)
                {
                    if (item.Id1 == LoggedInUserKey)
                    {
                        lst.Add(item.Id2);
                    }
                    else
                    {
                        lst.Add(item.Id1);
                    }
                }
                return ((int[])lst.ToArray<int>());
            }
            else { return null; }
        }

        protected IList<Parichay.Data.Entity.MemberMessage> getMessagesByUId(int uId)
        {
            IList<Parichay.Data.Entity.MemberMessage> lst = Data.Helper.NHibernateHelper.ConvertToListOf<Parichay.Data.Entity.MemberMessage>(Data.Helper.NHibernateHelper.Find("select distinct m from MemberMessage as m where m.Sender.Id=? and m.Recipient.Id=? and (m.ParentId=0 or m.ParentId is null) order by m.Createdon desc", new object[] { LoggedInUserKey, LoggedInUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 }, false));
            //msgs.SortDescending(m => m.Createdon);
            return (lst);
        }
        protected IList<Data.Entity.MemberMessage> getFriendMessagesByUId(int uId)
        {
            IList<Data.Entity.MemberMessage> lst = null;

            int[] intIds = getFriendIds(uId);

            if (intIds != null && intIds.Length != 0)
            {
                object[] friendIds = new object[intIds.Length];

                for (int temp = 0; temp < intIds.Length; temp++)
                {
                    friendIds[temp] = intIds[temp];
                }


                lst = Data.Helper.NHibernateHelper.ConvertToListOf<Data.Entity.MemberMessage>(Data.Helper.NHibernateHelper.FindByListParameter("from MemberMessage as m  where ((m.Sender.Id = m.Recipient.Id) and m.Sender.Id in (:friendIds)) and (m.ParentId=0 or m.ParentId is null) order by m.Createdon desc", "friendIds", friendIds, NHibernate.NHibernateUtil.Int32, false));

            }
            //msgs.SortDescending(m => m.Createdon);
            return (lst);
        }

        protected IList<Data.Entity.MemberAlert> getAlertsByUId(int uId)
        {
            IList<Data.Entity.MemberAlert> objList = Data.Helper.NHibernateHelper.ConvertToListOf<Data.Entity.MemberAlert>(Data.Helper.NHibernateHelper.Find("select distinct a from MemberAlert a where a.PUser.Id = ? and a.Ishidden = ?", new object[] { uId, 0 }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 }, false));
            return (objList);
        }

        protected IList<Data.Entity.MemberRequests> getRequestsByUId(int uId)
        {
            IList<Data.Entity.MemberRequests> myRequests = Data.Helper.NHibernateHelper.ConvertToListOf<Data.Entity.MemberRequests>(Data.Helper.NHibernateHelper.Find("select distinct a from MemberRequests a where a.Senderid = ? or a.Recipientid.Id = ?", new object[] { uId, uId }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 }, false));
            return myRequests;
        }
        protected IList<Data.Entity.MemberInvitations> getInvitesByUIdOrEmail(int uId, string usrEmail)
        {
            IList<Data.Entity.MemberInvitations> myInvites = Data.Helper.NHibernateHelper.ConvertToListOf<Data.Entity.MemberInvitations>(Data.Helper.NHibernateHelper.Find("select distinct a from MemberInvitations a where a.Senderid = ? or a.Email = ?", new object[] { uId, usrEmail }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.String }, false));
            return myInvites;
        }
        #endregion Messaging helpers
    }
}

namespace Parichay.MVC.Models
{
    public class CustomPrincipal : IPrincipal
    {
        public CustomPrincipal(CustomIdentity identity)
        {
            this.Identity = identity;
        }

        //public CustomPrincipal(IIdentity id, string[] roles)
        //    : base(id, roles)
        //{
        //    this.Identity = id;

        //}

        //#region IPrincipal Members

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return System.Web.Security.Roles.IsUserInRole(this.Identity.Name, role);
            //return true; // everyone's a winner
        }

        //#endregion
    }

    [Serializable]
    public class CustomIdentity : IIdentity, ISerializable
    {
        public CustomIdentity(string name)
        {
            this.Name = name;
        }

        public CustomIdentity(int userKey, string name, string email)
        {
            this.UserKey = userKey;
            this.Name = name;
            this.Email = email;
        }

        #region IIdentity Members

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(this.Name); }
        }

        public int UserKey { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        #endregion

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (context.State == StreamingContextStates.CrossAppDomain)
            {
                GenericIdentity gIdent = new GenericIdentity(this.Name, this.AuthenticationType);
                info.SetType(gIdent.GetType());

                System.Reflection.MemberInfo[] serializableMembers;
                object[] serializableValues;

                serializableMembers = FormatterServices.GetSerializableMembers(gIdent.GetType());
                serializableValues = FormatterServices.GetObjectData(gIdent, serializableMembers);

                for (int i = 0; i < serializableMembers.Length; i++)
                {
                    info.AddValue(serializableMembers[i].Name, serializableValues[i]);
                }
            }
            else
            {
                throw new InvalidOperationException("Serialization not supported");
            }
        }

        #endregion
    }

}
