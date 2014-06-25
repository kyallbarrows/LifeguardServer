using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parichay.Data.Entity;
using Parichay.Data.Helper;

namespace Parichay.MVC.Controllers
{
    public class MessageController : BaseController
    {
        //
        // GET: /Message/

        public ActionResult Index()
        {
            Models.UserHomeModel model= new Models.UserHomeModel();
            model.myMessages = MyMessages();
            model.myFriendMsg = MyFriendMsg();


            return View(model);
        }

        public ActionResult MsgDtl(int id)
        {
            MemberMessage model = Data.Helper.NHibernateHelper.UniqueResult<MemberMessage>(null, "Id", id);
            return View(model);
        }

        private IList<Parichay.Data.Entity.MemberMessage> MyMessages()
        {
            IList<Parichay.Data.Entity.MemberMessage> lst = NHibernateHelper.ConvertToListOf<Parichay.Data.Entity.MemberMessage>(NHibernateHelper.Find("select distinct m from MemberMessage as m where m.Sender.Id=? and m.Recipient.Id=? and (m.ParentId=0 or m.ParentId is null) order by m.Createdon desc", new object[] { LoggedInUserKey, LoggedInUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 },false));
           
            //foreach (var msg in lst)
            //{
            //    (msgs.FirstOrDefault(m => m.Id == msg.Id)).Children = msg.Children;
            //}
            //msgs.SortDescending(m => m.Createdon);
            return (lst);
        }
        private IList<MemberMessage> MyFriendMsg()
        {
            IList<MemberMessage> lst = null;

            int[] intIds = getFriendIds(LoggedInUserKey);

            if (intIds != null && intIds.Length != 0)
            {
                object[] friendIds = new object[intIds.Length];

                for (int temp = 0; temp < intIds.Length; temp++)
                {
                    friendIds[temp] = intIds[temp];
                }


                lst = Data.Helper.NHibernateHelper.ConvertToListOf<MemberMessage>(Data.Helper.NHibernateHelper.FindByListParameter("from MemberMessage as m  where (m.Sender.Id = m.Recipient.Id) and m.Sender.Id in (:friendIds) and (m.ParentId=0 or m.ParentId is null) order by m.Createdon desc", "friendIds", friendIds, NHibernate.NHibernateUtil.Int32, false));

            }

            //msgs.SortDescending(m => m.Createdon);
            return (lst);
        }

        public ActionResult Reply(int id, int rId, int rUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReplyMessage(int id, int rId, int rUrl, MemberMessage model)
        {

                model.ParentId = new MemberMessage() { Id = id };
                model.Recipient = new MemberDetails() { Id = rId };
                model.rUrl = rUrl;

            return AddMessage(model);
        }
        [HttpPost]
        public ActionResult AddMessage(MemberMessage model)
        {
            var result = new JsonResponse();
            if (string.IsNullOrEmpty(model.Text))
            {
                result.isSuccessful = false;
                TempData["message"] = result.errorMessage = "Message cannot be blank.";
            }
            else
            {
                //update the database
                if (IsUserAuthenticated)
                {
                    try
                    {
                        
                        MemberDetails thisUsr = NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);

                        var msg = new MemberMessage
                        {
                            Text = model.Text.Replace("\r\n", " "),
                            Modifiedon = DateTime.Now.ToUniversalTime(),
                            Createdon = DateTime.Now.ToUniversalTime(),
                            Sender = thisUsr,
                            Type = ((int)model.Type),
                            Isprivate = 0,
                            Source = "parichay",
                            Recipient = thisUsr
                        };


                        if (model.ParentId != null)
                        {
                            msg.ParentId = NHibernateHelper.UniqueResult<MemberMessage>(null, "Id", model.ParentId.Id);
                        }
                        if (model.Recipient != null)
                        {
                            MemberDetails recUsr = NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", model.Recipient.Id);
                            if (recUsr == null)
                            {
                                throw new Exception("This user do not have profile information. Message cannot be sent.");
                            }
                            else
                            {
                                msg.Recipient = recUsr;
                            }
                            
                        }

                        
                        NHibernateHelper.Save<MemberMessage>(msg);

                        //Sending Notify Now
                        try
                        {
                            if (msg.Sender.Id != msg.Recipient.Id)
                            {
                                string msgUrl = "<a href='" + Url.Action("MsgDtl", "Message", new { id = msg.Id }) + "' >reply</a>";
                                Alert(msg.Recipient.Id, string.Format("You have received a ({0}) to from: {1}", msgUrl, msg.Sender.Givennm));
                            }
                        }
                        catch (Exception ex1)
                        {

                            Data.Helper.NHibernateHelper.Log(new Exception("Error Sending Message Alert=>", ex1));
                        }

                        result.Id = msg.Id.ToString();

                        

                        result.isSuccessful = true;
                        TempData["message"] = result.responseText = "Message Sent Successfully";
                    }
                    catch (Exception excp1)
                    {
                        Data.Helper.NHibernateHelper.Log(new Exception("Error Sending Message=>", excp1));
                        result.isSuccessful = false;
                        TempData["message"] = result.errorMessage = "Failed to save message. Error: " + excp1.Message;
                    }
                }
                else
                {
                    result.isSuccessful = false;
                    TempData["message"] = result.errorMessage = "You must be logged in to post a message";
                }
            }
            if (IsJsonRequest())
            { return Json(result); }
            else
            {
                    return RedirectToAction("Index",((Parichay.AppConstants.ReturnContollerHomes)model.rUrl).ToString());
            }
        }
        [HttpPost]
        public ActionResult Delete(int id,int? rUrl)
        {
            var result = new JsonResponse();

            try
            {
                //delete message from database
                //make sure the user has the right to delete

                //get the message
                MemberMessage lstResults = Data.Helper.NHibernateHelper.UniqueResult<MemberMessage>(null, new string[] { "Id", "Sender.Id" }, new object[] { id, LoggedInUserKey });
                //MemberMessage lstResults = m_PersistenceManager.UniqueResult<MemberMessage>(null,  "Id",id );

                if (lstResults != null)
                {
                    MemberMessage message = lstResults;

                    int childCount = Data.Helper.NHibernateHelper.Count("select m.Id from MemberMessage m where m.ParentId.Id=?",id,NHibernate.NHibernateUtil.Int32,false);

                    if (childCount > 0)
                    {
                        result.isSuccessful = false;
                        TempData["message"] = result.errorMessage = "You cannot delete messages that have been replied to.";
                    }
                    else
                    {
                        if (message.Type == 2)
                        {
                            ////delete the files
                            //try
                            //{
                            //    AmazonHelper.Delete(message.ImageFilename); //image
                            //    AmazonHelper.Delete(message.ThumbnailFilename); //thumbnail
                            //}
                            //catch (Exception ex)
                            //{
                            //    Utils.Log(ex);
                            //}
                        }

                        Data.Helper.NHibernateHelper.Delete<MemberMessage>(message);
                        result.isSuccessful = true;
                        TempData["message"] = result.errorMessage = "Message deleted.";
                    }
                }
                else
                {
                    result.isSuccessful = false;
                    TempData["message"] = result.errorMessage = "Message already deleted or you don't have permission to delete it.";
                }
            }
            catch (Exception exc)
            {
                result.isSuccessful = false;
                Data.Helper.NHibernateHelper.Log(new Exception("Error deleting message. ==> ", exc));
                TempData["message"] = result.errorMessage = "Unexpected error while deleting message.";
            }
            if (IsJsonRequest())
            {
                return Json(result);
            }
            else
            {
                int rUrl2 = rUrl.HasValue ? rUrl.Value : 0;
                return RedirectToAction("Index",((Parichay.AppConstants.ReturnContollerHomes)rUrl2).ToString());
            }
        }

        public ActionResult Del(int id,int? rUrl)
        {
            return Delete(id,rUrl);
        }
    }
}
