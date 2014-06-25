using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Parichay.MVC.Controllers
{
    public class AlertController : BaseController
    {
        //
        // GET: /Alert/

        public ActionResult Index(int? id, int? v)
        {
            Parichay.Data.Entity.MemberAlert model = new Parichay.Data.Entity.MemberAlert();
            if ((v.HasValue) && (v.Value == 1))
            {
                model.MyAlerts = Data.Helper.NHibernateHelper.ConvertToListOf<Data.Entity.MemberAlert>(Data.Helper.NHibernateHelper.Find("select distinct a from MemberAlert a where a.PUser.Id=?", LoggedInUserKey, NHibernate.NHibernateUtil.Int32,false));
            }
            else
            {
                model.MyAlerts = Data.Helper.NHibernateHelper.ConvertToListOf<Data.Entity.MemberAlert>(Data.Helper.NHibernateHelper.Find("select distinct a from MemberAlert a where a.PUser.Id = ? and a.Ishidden = ?", new object[] { LoggedInUserKey, 0 }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 },false));
            }
            //model.AlertTypes = Data.Helper.NHibernateHelper.RetrieveAll<Alerttype>();
            return View(model);
        }


        //

        public ActionResult Delete(int id, int? rUrl)
        {
            try
            {
                Data.Entity.MemberAlert model = Data.Helper.NHibernateHelper.UniqueResult<Data.Entity.MemberAlert>(null, "Id", id);
                // TODO: Add insert logic here
                Data.Helper.NHibernateHelper.Delete<Data.Entity.MemberAlert>(model);
                TempData["message"] = "Requested alert is deleted now.";

            }
            catch (Exception ex1)
            {
                TempData["message"] = "Unable to delete. Error: " + ex1.Message;

            }
            int rUrl2 = rUrl.HasValue ? rUrl.Value : 0;
            return RedirectToAction("Index", ((Parichay.AppConstants.ReturnContollerHomes)rUrl2).ToString());

        }

        //
        // GET: /MemberAlert/Delete/5

        public ActionResult Hide(int id, int? rUrl)
        {
            try
            {
                Data.Entity.MemberAlert model = Data.Helper.NHibernateHelper.UniqueResult<Data.Entity.MemberAlert>(null, "Id", id);
                model.Ishidden = (model.Ishidden == 0) ? 1 : 0;
                // TODO: Add insert logic here
                Data.Helper.NHibernateHelper.Update<Data.Entity.MemberAlert>(model);
                TempData["message"] = "Requested alert is updated now.";

            }
            catch (Exception ex1)
            {
                TempData["message"] = "Unable to toggle. Error: " + ex1.Message;

            }
            int rUrl2 = rUrl.HasValue ? rUrl.Value : 0;
            return RedirectToAction("Index", ((Parichay.AppConstants.ReturnContollerHomes)rUrl2).ToString());

        }
        //
        // POST: /Alert/Delete/5

    }
}
