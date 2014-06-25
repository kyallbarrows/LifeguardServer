using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parichay.Data.Entity;

namespace Parichay.MVC.Controllers
{
    public class ConnectController : BaseController
    {
        public ActionResult Index()
        {
            IList<Data.Entity.MemberInvitations> myInvites = Data.Helper.NHibernateHelper.ConvertToListOf<Data.Entity.MemberInvitations>(Data.Helper.NHibernateHelper.Find("select distinct a from MemberInvitations a where a.Senderid = ? or a.Email = ?", new object[] { LoggedInUserKey, LoggedInUserEmail }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.String },false));
            IList<Data.Entity.MemberRequests> myRequests = Data.Helper.NHibernateHelper.ConvertToListOf<Data.Entity.MemberRequests>(Data.Helper.NHibernateHelper.Find("select distinct a from MemberRequests a where a.Senderid = ? or a.Recipientid.Id = ?", new object[] { LoggedInUserKey, LoggedInUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 },false));

            ViewBag.myInvites = myInvites;
            ViewBag.myRequests = myRequests;

            return View();

        }
        #region Send Invite or Request

        public ActionResult Invite(int? id, AppConstants.InviteTypes type)
        {
            Data.Entity.MemberInvitations model = new Data.Entity.MemberInvitations() { Senderid = new Data.Entity.MemberDetails() { Id = LoggedInUserKey }, Createdate = DateTime.Now, Version = DateTime.Now, Type = (int)type };

            model.MyInvites = Data.Helper.NHibernateHelper.ConvertToListOf<Data.Entity.MemberInvitations>(Data.Helper.NHibernateHelper.Find("select distinct a from MemberInvitations a where a.Senderid = ? or a.Email = ?", new object[] { LoggedInUserKey, LoggedInUserEmail }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.String }, false));

            return (View(model));
        }

        [HttpPost]
        public ActionResult Invite(int? id, AppConstants.InviteTypes type, Data.Entity.MemberInvitations model)
        {

            try
            {
                int _senderId = LoggedInUserKey;

                if ((model.Senderid != null) && (model.Senderid.Id != 0) && (model.Senderid.Id != -1))
                    _senderId = model.Senderid.Id;

                MemberInvitations itm = new MemberInvitations();

                Guid val;

                do
                {
                    val = System.Guid.NewGuid();
                } while (Data.Helper.NHibernateHelper.UniqueResult<MemberInvitations>("Guid", "Guid", val.ToString()) != null);

                itm.Guid = val.ToString();
                itm.Senderid = Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", _senderId);
                itm.Email = model.Email;
                itm.Type = (int)type;
                itm.Createdate = DateTime.Now;
                itm.TargetPageid = id;
                itm.Status = 1;

                try
                {
                    string[] msgParam = new string[4];

                    string strMsg = "Hello,\n\n {0} has requested you to join Parichay. \n\n Please click the following link to sign up if you do not have an account: \n {1} \n\n Please click on following link to acknowledge if you already have an account:\n {2} \n\n {3} \n\n Thanks & Regards,\n Webmasters \n Parichay";

                    msgParam[0] = itm.Senderid.Givennm;
                    msgParam[1] = AppConstants.BaseSiteUrl.TrimEnd('/') + Url.Action("Register", "Account");
                    msgParam[2] = AppConstants.BaseSiteUrl.TrimEnd('/') + Url.Action("AckInvite", new { id = itm.Guid });
                    msgParam[3] = string.IsNullOrEmpty(model.UserMessage) ? "" : "\n----------\n" + model.UserMessage + "\n----------\n";


                    strMsg = string.Format(strMsg, msgParam);

                    SendMail(model.Email, itm.Senderid.Givennm + " : Request to Join Parichay", strMsg);
                    //SendMail(itm.Senderid.PEmail, itm.Senderid.Givennm + " : Request to Join Parichay", strMsg);
                }
                catch (Exception exc1)
                {
                    Data.Helper.NHibernateHelper.Log(new Exception("Error sending Connect Invitation Email=>", exc1));
                }

                Data.Helper.NHibernateHelper.Save<MemberInvitations>(itm);

                TempData["message"] = "Invitation sent successfully.";
            }
            catch (Exception exc1)
            {
                TempData["message"] = "Unable to invite friend. Error:" + exc1.Message;
            }


            model.MyInvites = Data.Helper.NHibernateHelper.ConvertToListOf<MemberInvitations>(Data.Helper.NHibernateHelper.Find("select distinct a from MemberInvitations a where a.Senderid = ? or a.Email = ?", new object[] { LoggedInUserKey, LoggedInUserEmail }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.String }, false));

            return (View(model));
        }

        public ActionResult VRequest(int id, int tId, int type)
        {

            MemberRequests model = new MemberRequests() { Senderid = new MemberDetails() { Id = LoggedInUserKey }, Recipientid = new MemberDetails() { Id = id }, TargetPageid = tId, Createdate = DateTime.Now, Version = DateTime.Now, Type = (int)type };


            model.MyRequests = Data.Helper.NHibernateHelper.ConvertToListOf<MemberRequests>(Data.Helper.NHibernateHelper.Find("select distinct a from MemberRequests a where a.Senderid = ? or a.Recipientid.Id = ?", new object[] { LoggedInUserKey, LoggedInUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 }, false));

            // model.MyInvites = Models.ModelMapper.Map(m_PersistenceManager.RetrieveEquals<MemberInvitations>("Senderid.Id", LoggedInUserKey));

            return (View(model));
        }

        [HttpPost]
        public ActionResult VRequest(int id, int tId, int type, MemberRequests model)
        {

            try
            {
                MemberDetails mmbr = Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", id);

                string recipientmail = mmbr.PEmail;

                int _senderId = LoggedInUserKey;

                if ((model.Senderid != null) && (model.Senderid.Id != 0) && (model.Senderid.Id != -1))
                    _senderId = model.Senderid.Id;

                MemberRequests itm = new MemberRequests();

                Guid val;

                do
                {
                    val = System.Guid.NewGuid();
                } while (Data.Helper.NHibernateHelper.UniqueResult<MemberRequests>("Guid", "Guid", val.ToString()) != null);

                itm.Guid = val.ToString();
                itm.Senderid = Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", _senderId);
                // itm.Email = recipientmail;
                itm.Recipientid = mmbr;
                itm.TargetPageid = model.TargetPageid;
                itm.Createdate = DateTime.Now;
                itm.Type = (int)type;
                itm.Status = 1;

                try
                {
                    //Mail sending. 
                    //<-- To Do --> Add logic to check if email notifications for target user are enabled
                    string[] msgParam = new string[4];

                    string strMsg = "Hello,\n\n {0} has sent you a {1} request at Parichay. \n\n Please click on following link to acknowledge :\n {2} \n\n {3} \n\n Thanks & Regards,\n Webmasters \n WebAdmin - Parichay";

                    msgParam[0] = itm.Senderid.Givennm;
                    msgParam[1] = ((AppConstants.RequestTypes)type).ToString();
                    msgParam[2] = AppConstants.BaseSiteUrl.TrimEnd('/') + Url.Action("AckRequest", new { id = itm.Guid });
                    msgParam[3] = string.IsNullOrEmpty(model.UserMessage) ? "" : "\n----------\n" + model.UserMessage + "\n----------\n";


                    strMsg = string.Format(strMsg, msgParam);

                    SendMail(model.Email, itm.Senderid.Givennm + " : Parichay " + type.ToString() + " request", strMsg);
                    //Mail sending
                }
                catch (Exception exc1)
                {
                    Data.Helper.NHibernateHelper.Log(new Exception("Error sending Connect request Email=>", exc1));
                }



                Data.Helper.NHibernateHelper.Save<MemberRequests>(itm);

                TempData["message"] = "Invitation sent successfully.";
                model.TargetPageid = id;
            }
            catch (Exception exc1)
            {
                TempData["message"] = "Unable to invite friend. Error:" + exc1.Message;
            }


            model.MyRequests = Data.Helper.NHibernateHelper.ConvertToListOf<MemberRequests>(Data.Helper.NHibernateHelper.Find("select distinct a from MemberRequests a where a.Senderid = ? or a.Recipientid.Id = ?", new object[] { LoggedInUserKey, LoggedInUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 }, false));


            return (View(model));
        }

        #endregion
        #region Acknowledge

        [Authorize]
        public ActionResult AckRequest(string id)
        {
            MemberRequests source = Data.Helper.NHibernateHelper.UniqueResult<MemberRequests>(null, "Guid", id);

            MemberRequests model = source;
            model.IsRecipient = (source.Senderid.Id != LoggedInUserKey);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AckRequest(string id, MemberRequests model)
        {
            MemberRequests source = Data.Helper.NHibernateHelper.UniqueResult<MemberRequests>(null, "Guid", model.Guid);

 
            switch (model.submitButton)
            {
                case ("Accept"):
                    {
                        if (source.Type == (int)AppConstants.RequestTypes.Friend)
                        {
                            return AddFriend(source);
                        }
                        else if (source.Type == (int)AppConstants.RequestTypes.Group)
                        {
                            return JoinGroup(source);
                        }
                        break;
                    }
                case ("Decline"):
                    {
                        //MemberRequests thisInvite = Data.Helper.NHibernateHelper.UniqueResult<MemberRequests>(null, "Guid", model.Guid);
                        Data.Helper.NHibernateHelper.Delete<MemberRequests>(source);
                        break;
                    }
                case ("Hide"):
                    {

                        source.Status = 0;
                        Data.Helper.NHibernateHelper.Update<MemberRequests>(source);
                        TempData["message"] = "The notification is now hidden from view. You will receive a Notification if the invitation is accepted.";
                        break;
                    }
                default:
                    {
                        TempData["message"] = "Un-Identified action.";
                        break;
                    }
            }


            return View(model);
        }

        public ActionResult AckInvite(string id)
        {
            Data.Entity.MemberInvitations source = Data.Helper.NHibernateHelper.UniqueResult<Data.Entity.MemberInvitations>(null, "Guid", id);
            Data.Entity.MemberInvitations model = source;
            model.IsRecipient = (source.Senderid.Id != LoggedInUserKey);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AckInvite(string id, Data.Entity.MemberInvitations model)
        {
            MemberInvitations source = Data.Helper.NHibernateHelper.UniqueResult<MemberInvitations>(null, "Guid", id);


            //source.Senderid=Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null,"Id",source.

            switch (model.submitButton)
            {
                case ("Confirm"):
                    {
                        Alert(source.Senderid.Id, string.Format("Hello {0} your freind {1} (e-mail: {2}) has accepted your invitation to join Parichay.", source.Senderid.Givennm, LoggedInUserName, source.Email));

                        source.BecameUserId = LoggedInUserKey;
                        source.Status = 0;
                        try
                        {
                            CreateRequest(source);
                            Data.Helper.NHibernateHelper.Delete<MemberInvitations>(source);
                            TempData["message"] = string.Format("Hello {0}, thanks for acknowledging the request. Your freind {1} has been notified of accepting the invitation. You can find your freinds in freinds section.", LoggedInUserName, source.Senderid.Givennm);

                        }
                        catch (Exception ex1) { TempData["message"] = "Error Confirming invitation:" + ex1.Message; Data.Helper.NHibernateHelper.Log(ex1); }

                        //return AddFriend(model);
                        break;
                    }
                case ("Hide"):
                    {

                        source.Status = 0;
                        Data.Helper.NHibernateHelper.Update<MemberInvitations>(source);
                        TempData["message"] = "The notification is now hidden from view. You will receive a Notification if the invitation is accepted.";
                        break;
                    }
                default:
                    {
                        TempData["message"] = "Un-Identified action.";
                        break;
                    }
            }
            model = source;
            model.IsRecipient = (source.Senderid.Id != LoggedInUserKey);

            return View(model);
        }

        private void CreateRequest(MemberInvitations model)
        {
            MemberRequests itm = new MemberRequests();

            Guid val;

            do
            {
                val = System.Guid.NewGuid();
            } while (Data.Helper.NHibernateHelper.UniqueResult<MemberRequests>("Guid", "Guid", val.ToString()) != null);

            itm.Guid = val.ToString();
            itm.Senderid = model.Senderid;
            itm.Recipientid = Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);
            itm.TargetPageid = model.TargetPageid;
            itm.Type = model.Type;
            itm.Createdate = DateTime.Now;
            itm.Status = 1;


            Data.Helper.NHibernateHelper.Save<MemberRequests>(itm);

        }

        private ActionResult AddFriend(MemberRequests thisInvite)
        {
            try
            {
                if (thisInvite.Senderid.Id == LoggedInUserKey)
                {
                    throw new Exception("A member cannot add himself as a friend.");
                }

                //MemberRequests thisInvite = Data.Helper.NHibernateHelper.UniqueResult<MemberRequests>(null, "Guid", model.Guid);

                MemberFriends itm = new MemberFriends();
                itm.Createdon = DateTime.Now;
                itm.Modifiedon = DateTime.Now;
                //itm = AutoMapper.Mapper.Map(model, itm);
                itm.MemberDetails = Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", thisInvite.Senderid.Id);
                itm.Friendid = Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);

                Data.Helper.NHibernateHelper.Save<MemberFriends>(itm);

                string strSender = "Hello " + itm.MemberDetails.Givennm + ", " + itm.Friendid.Givennm + " has accepted your friend request.";
                string strRecipient = "Hello" + itm.Friendid.Givennm + ", you are now friend with " + itm.MemberDetails.Givennm;

                Alert(itm.MemberDetails.Id, strSender);
                Alert(LoggedInUserKey, strRecipient);


                Data.Helper.NHibernateHelper.Delete<MemberRequests>(thisInvite);
                TempData["message"] = "Requested User is now your friend.";
            }
            catch (Exception ex)
            {
                Data.Helper.NHibernateHelper.Log(new Exception("Error accepting Friend Request. User Id:" + thisInvite.Recipientid.Id + ", Requester Id:" + thisInvite.Senderid.Id, ex));
                Data.Helper.NHibernateHelper.Delete<MemberRequests>(thisInvite);
                TempData["message"] = ex.Message + ex.StackTrace;
            }
            return RedirectToAction("Index", "Friend");
        }

        private ActionResult JoinGroup(MemberRequests thisReq)
        {
            try
            {
                if (thisReq.Senderid.Id == LoggedInUserKey)
                {
                    throw new Exception("You are already a member of your group.");
                }

                // = Data.Helper.NHibernateHelper.UniqueResult<MemberRequests>(null, "Guid", model.Guid);

                MemberGroupmembers itm = new MemberGroupmembers();
                //itm = AutoMapper.Mapper.Map(model, itm);
                itm.MemberDetails = Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", thisReq.Recipientid.Id);
                itm.Group = Data.Helper.NHibernateHelper.UniqueResult<MemberGroups>(null, "Id", thisReq.TargetPageid);
                itm.bRole = false;
                itm.bStatus = true;

                if (itm.Group == null)
                {
                    throw new Exception("Group No more available.");
                }

                if (Data.Helper.NHibernateHelper.UniqueResult<MemberGroupmembers>(null, new string[] { "MemberDetails.Id", "Group.Id" }, new object[] { thisReq.Recipientid.Id, thisReq.TargetPageid }) != null)
                {
                    string erText = "This user is already member of this group.";
                    throw new Exception(erText);
                }


                Data.Helper.NHibernateHelper.Save<MemberGroupmembers>(itm);



                string strSender = "Hello " + thisReq.Senderid.Givennm + ", " + thisReq.Recipientid.Givennm + " has joined your paper group.";
                string strRecipient = "Hello" + thisReq.Recipientid.Givennm + ", you are now a member of the group " + itm.Group.Name;

                Alert(thisReq.Senderid.Id, strSender);
                Alert(LoggedInUserKey, strRecipient);

                //thisInvite.BecameUserId = itm.Id;
                Data.Helper.NHibernateHelper.Delete<MemberRequests>(thisReq);

                TempData["message"] = "You are now a member of requested group";

            }
            catch (Exception ex)
            {
                Data.Helper.NHibernateHelper.Log(new Exception("Error accepting Join group Request. User Id:" + thisReq.Recipientid.Id + ", group Id:" + thisReq.TargetPageid.Value, ex));
                Data.Helper.NHibernateHelper.Delete<MemberRequests>(thisReq);
                TempData["message"] = ex.Message + ex.StackTrace;
            }
            return RedirectToAction("Index", "Group");
        }

        #endregion Acknowledge
    }
}
