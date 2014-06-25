using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parichay.Data.Entity;

namespace Parichay.MVC.Controllers
{
    public class FriendController : BaseController
    {
        public ActionResult Index()
        {
            //MemberDetails me = m_PersistenceManager.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);
            IList<MemberFriends> lstFrnds = Data.Helper.NHibernateHelper.ConvertToListOf<MemberFriends>(Data.Helper.NHibernateHelper.Find("from MemberFriends a where a.MemberDetails.Id=? or a.Friendid.Id=?", new object[] { LoggedInUserKey, LoggedInUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 },false)); //m_PersistenceManager.RetrieveEquals<MemberFriends>("MemberDetails", me);

            foreach (var itm in lstFrnds)
            {
                //If this relationship the first user is current user, swap the users
                if (itm.MemberDetails.Id != LoggedInUserKey)
                {
                    MemberDetails tmp = itm.MemberDetails;

                    itm.MemberDetails = itm.Friendid;
                    itm.Friendid = tmp;
                }
            }

            return View(lstFrnds);
        }

        [HttpPost]
        public ActionResult RemoveFriend(int? userId, int friendId)
        {
            try
            {
                int uId = userId.HasValue ? userId.Value : LoggedInUserKey;
                //itm.MemberDetails = m_PersistenceManager.UniqueResult<MemberDetails>(null, "Id", userId);
                //itm.Friendid = m_PersistenceManager.UniqueResult<MemberDetails>(null, "Id",  friendId);

                //itm= m_PersistenceManager.UniqueResult<MemberFriends>(null,new string[] {"MemberDetails","Friendid" }, new object[]{ itm.MemberDetails, itm.Friendid});


                //m_PersistenceManager.Delete<MemberFriends>(itm);

                object obj = Data.Helper.NHibernateHelper.Delete("from MemberFriends a where a.MemberDetails.Id=? and a.Friendid.Id=?", new object[] { uId, friendId }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 });
                obj = Data.Helper.NHibernateHelper.Delete("from MemberFriends a where a.MemberDetails.Id=? and a.Friendid.Id=?", new object[] { friendId, uId }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 });

                TempData["message"] = "Requested User is now removed from your friends list.";
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message + ex.StackTrace;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Find()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Find(string givenEmail)
        {
            //string srchQuery = "from MemberDetails p where p.Givennm like ?";
            //System.Collections.IList rtrn = Data.Helper.NHibernateHelper.Find(srchQuery, "%"+givenEmail+"%", NHibernate.NHibernateUtil.String,false);
            IList<MemberDetails> rtrn = Data.Helper.NHibernateHelper.FetchProjection<MemberDetails>(new string[] { "Givennm", "Id", "PicId", "Institute", "Addr" }, "Givennm", "%" + givenEmail + "%", 0, 10, true, null, false);
            ViewBag.LoggedInUserKey = LoggedInUserKey;
            ViewBag.searchResults = rtrn;
            return View();
        }
        //
        // GET: /Friend/Details/5

        public ActionResult VRequest(int id, int userId, int type)
        { return RedirectToAction("VRequest", "Connect", new {id=id,userId=userId,type=type }); }

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Friend/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Friend/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Friend/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Friend/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Friend/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Friend/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

namespace Parichay.MVC.Models
{
    public class usrSrch
    {
        public int Id { get; set; }
        public string Givennm { get; set; }
        public string Institute { get; set; }
        public int PicId { get; set; }

    }

    public class IntPair
    {
        public int Id1 { get; set; }
        public int Id2 { get; set; }
    }
}