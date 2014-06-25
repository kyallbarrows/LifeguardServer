using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parichay.Data.Entity;



namespace Parichay.MVC.Controllers
{
    
    public class GroupController : BaseController
    {

        public ActionResult Index()
        {
            Models.GroupHomeModel model = new Models.GroupHomeModel();

            //to do => add logic to show only publicly visible and my owned groups. right now showing all
            model.allGroups = Data.Helper.NHibernateHelper.ConvertToListOf<MemberGroups>(Data.Helper.NHibernateHelper.Find("select distinct g from MemberGroups g where ((g.Visibility!=0)or(g.Visibility is not null))", null, null, false));

            model.myGroups = Data.Helper.NHibernateHelper.ConvertToListOf<MemberGroups>(Data.Helper.NHibernateHelper.Find("select g from MemberGroups g join g.FKGrpMmbrsGrp gmbr where gmbr.MemberDetails.Id=? order by g.Createdon desc", LoggedInUserKey, NHibernate.NHibernateUtil.Int32,false));


            return View(model);
        }

        //
        // GET: /Group/Details/5

        public ActionResult Details(int id)
        {
            Models.GroupDetailsModel model = new Models.GroupDetailsModel();
            model.groupDetails = Data.Helper.NHibernateHelper.UniqueResult<MemberGroups>(null, "Id", id);
            model.groupMessages = Data.Helper.NHibernateHelper.ConvertToListOf<MemberGroupmessages>(Data.Helper.NHibernateHelper.Find("select m from MemberGroupmessages m where m.Group.Id=? and (m.Parent is null or m.Parent=0)", id, NHibernate.NHibernateUtil.Int32,false));
            model.loggedInUserId = LoggedInUserKey;
            return View(model);
        }

        [HttpPost]
        public ActionResult Post(int id)
        {
            return View("Post", (new MemberGroupmessages() { Group = new MemberGroups() { Id = id } }));
        }

        public ActionResult Reply(int id, int rId, int gId)
        {
            return View(new MemberGroupmessages() { Parent = new MemberGroupmessages() { Id = id }, Recipient = new MemberDetails() { Id = rId }, Group = new MemberGroups() { Id = gId } });
        }

        [HttpPost]
        public ActionResult PostMessage(MemberGroupmessages model)
        {
            try
            {

                MemberGroupmessages grpMsg = new MemberGroupmessages();

                //grpMsg = Models.ModelMapper.Map(model);

                grpMsg.Group = Data.Helper.NHibernateHelper.UniqueResult<MemberGroups>(null, "Id", model.Group.Id);
                grpMsg.Sender = Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);
                grpMsg.Recipient = grpMsg.Sender;//Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", model.Recipient.Id);
                //model.Parentid = 0;
                //model.Isprivate = 0U;
                grpMsg.bIsprivate = false;
                grpMsg.Text = model.Text;
                grpMsg.Type = 0;
                grpMsg.Createdon = DateTime.Now;
                grpMsg.Modifiedon = DateTime.Now;
                grpMsg.Source = "parichay";
                // TODO: Add insert logic here

                if (model.Parent != null)
                {
                    grpMsg.Parent = Data.Helper.NHibernateHelper.UniqueResult<MemberGroupmessages>(null, "Id", model.Parent.Id);

                }
                if (model.Recipient != null)
                {
                    MemberDetails recUsr = Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", model.Recipient.Id);
                    if (recUsr == null)
                    {
                        throw new Exception("This user do not have profile information. Message cannot be sent.");
                    }
                    else
                    {
                        grpMsg.Recipient = recUsr;
                    }

                    try
                    {
                        if (grpMsg.Sender.Id != grpMsg.Recipient.Id)
                        {
                            string msgUrl = "<a href='" + Url.Action("Details", "Group", new { id = grpMsg.Group.Id }) + "' >reply</a>";
                            Alert(model.Recipient.Id, string.Format("You have received a reply ({0}) to from: {1}", msgUrl, grpMsg.Sender.Givennm));
                        }
                    }
                    catch(Exception ex1)
                    {

                        Data.Helper.NHibernateHelper.Log(new Exception("Error Sending Group Message Alert=>", ex1));
                    }
                   
                }

                Data.Helper.NHibernateHelper.Save<MemberGroupmessages>(grpMsg);
            }
            catch (Exception ex)
            {
                Data.Helper.NHibernateHelper.Log(new Exception("Error Sending Group Message=>", ex));
                TempData["message"] = ex.Message + ex.StackTrace;
            }
            return RedirectToAction("Details", new { id = model.Group.Id });

        }

        public ActionResult RemoveMessage(int id)
        {
            MemberGroupmessages itm = Data.Helper.NHibernateHelper.UniqueResult<MemberGroupmessages>(null, "Id", id);
            try
            {
                if (itm == null)
                {
                    throw new Exception("Invalid Id");
                }

                Data.Helper.NHibernateHelper.Delete<MemberGroupmessages>(Data.Helper.NHibernateHelper.UniqueResult<MemberGroupmessages>(null, "Id", id));
            }
            catch (Exception ex1)
            {
                TempData["message"] = "Unable To Delete Message. Error: " + ex1.Message;
            }
            return RedirectToAction("Details", new { id = itm.Group.Id });
        }

        //
        // GET: /Group/Create
        public ActionResult Create()
        {
            return (View());
        }
        //
        // POST: /Group/Create

        [HttpPost]
        public ActionResult Create(MemberGroups model)
        {
            try
            {
                model.Createdon = DateTime.Now;
                model.Modifiedon = DateTime.Now;
                // TODO: Add insert logic here
                Data.Helper.NHibernateHelper.Save<MemberGroups>(model);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Update(int id)
        {
            MemberGroups itm = Data.Helper.NHibernateHelper.UniqueResult<MemberGroups>(null, "Id", id);
            if (itm.OwnerId != LoggedInUserKey)
            {
                TempData["message"] = "You do not have Edit access to the requested group. Activity Logged.";
                Data.Helper.NHibernateHelper.Log(new Exception("Hacking Attempt!!! User: "+LoggedInUserName+", Group Id: "+itm.Id));
                return RedirectToAction("Index");
            }
            return View(itm);
        }

        //
        // POST: /Group/Edit/5
        [HttpPost]
        public ActionResult Update(MemberGroups model)
        {
            try
            {
                // TODO: Add update logic here
                MemberGroups itm = Data.Helper.NHibernateHelper.UniqueResult<MemberGroups>(null, "Id", model.Id);

                if (itm.OwnerId != LoggedInUserKey)
                {
                    TempData["message"] = "You do not have Edit access to the requested group. Activity Logged.";
                    Data.Helper.NHibernateHelper.Log(new Exception("Hacking Attempt!!! User: " + LoggedInUserName + ", Group Id: " + itm.Id));
                    return RedirectToAction("Index");
                }

                itm = AutoMapper.Mapper.Map(model, itm);
                itm.OwnerId = LoggedInUserKey;

                Data.Helper.NHibernateHelper.Update<MemberGroups>(itm);
                TempData["message"] = "The requested group is updated now.";
            }
            catch (Exception excp)
            {
                TempData["message"] = "Error Updating the group: " + excp.Message + excp.StackTrace;
            }
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult vJoinGrp(int id)
        {
            return View(new MemberGroupmembers() { MemberDetails = new MemberDetails() { Id = LoggedInUserKey }, Group = new MemberGroups() { Id = id }, Role = 0 });
        }

        [HttpPost]
        public ActionResult JoinGroup(MemberGroupmembers model)
        {
            try
            {
                MemberGroupmembers itm = new MemberGroupmembers();
                itm = AutoMapper.Mapper.Map(model, itm);

                if (Data.Helper.NHibernateHelper.Count("select m.Id from MemberGroupmembers m where m.MemberDetails.Id=? and m.Group.Id=?", new object[] { itm.MemberDetails, itm.Group }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 }, false) > 0)
                    throw new Exception("The requested user is already a member of this group.");

                itm.MemberDetails = Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", model.MemberDetails.Id);
                itm.Group = Data.Helper.NHibernateHelper.UniqueResult<MemberGroups>(null, "Id", model.Group.Id);

                Data.Helper.NHibernateHelper.Save<MemberGroupmembers>(itm);
                TempData["message"] = "Requested Membership is updated now";
            }
            catch (Exception exc)
            {

                TempData["message"] = "Error Joining the requested group: " + exc.Message + exc.StackTrace;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult LeaveGroup(int? userId, int groupId)
        {
            try
            {
                MemberGroupmembers itm = Data.Helper.NHibernateHelper.UniqueResult<MemberGroupmembers>(null, new string[] { "MemberDetails.Id", "Group.Id" }, new object[] { userId.HasValue ? userId.Value : LoggedInUserKey, groupId });
                Data.Helper.NHibernateHelper.Delete<MemberGroupmembers>(itm);
            }
            catch (Exception exc)
            {

                TempData["message"] = "Error Leaving the requested group: " + exc.Message + exc.StackTrace;
            }
            return RedirectToAction("Index");
        }

        #region Admin Actions

        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult Acquire(int id)
        {
            int grpId = 0;

            try
            {
                //object obj11 = m_PersistenceManager.ExecuteUpdateNamedQuery("MemberSessions.AcquireById", new object[] { LoggedInUserKey, id }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 });
                IList<MemberGroups> usrGroups = Data.Helper.NHibernateHelper.Find<MemberGroups>("Id", id);
                foreach (var usrGrp in usrGroups)
                {
                    grpId = usrGrp.Id;
                    usrGrp.OwnerId = LoggedInUserKey;
                    Data.Helper.NHibernateHelper.Update<MemberGroups>(usrGrp);
                }

                TempData["message"] = "The group is transferred to your name now";

            }
            catch (Exception ex1)
            {

                TempData["message"] = "Cannot Acquire Session. Error: " + ex1.Message;

            }
            return RedirectToAction("Manage", new { id = grpId });
        }

        [Authorize(Roles = "Super Admin,Admin")]
        public ActionResult Manage()
        {
            IList<MemberGroups> lstAllGroups = Data.Helper.NHibernateHelper.Find<MemberGroups>();
            return View(lstAllGroups);
        }

        //
        // POST: /Group/Delete/5
        [Authorize(Roles = "Super Admin,Admin")]
        [HttpPost]
        public ActionResult Destroy(int id)
        {
            try
            {
                MemberGroups itm = Data.Helper.NHibernateHelper.UniqueResult<MemberGroups>(null, "Id", id);
                object obj1 = Data.Helper.NHibernateHelper.Delete("from MemberGroupmessages gms where gms.Group.Id=?", id, NHibernate.NHibernateUtil.Int32);// <IList<MemberGroupmessages>>(Data.Helper.NHibernateHelper.Find<MemberGroupmessages>("Group", itm));
                object obj2 = Data.Helper.NHibernateHelper.Delete("from MemberGroupmembers gmbr where gmbr.Group.Id=?", id, NHibernate.NHibernateUtil.Int32);//<IList<MemberGroupmembers>>(Data.Helper.NHibernateHelper.Find<MemberGroupmembers>("Group", itm));

                // TODO: Add update logic here
                Data.Helper.NHibernateHelper.Delete<MemberGroups>(itm);
                TempData["message"] = "The requested group is deleted now.";
            }
            catch (Exception excp)
            {
                TempData["message"] = "Error deleting the group: " + excp.Message + excp.StackTrace;
            }
            return RedirectToAction("Manage");
        }

        #endregion
    }
}
