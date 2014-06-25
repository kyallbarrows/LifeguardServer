using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace Parichay.MVC.Controllers
{
    [Authorize(Roles="Super Admin,Admin")]
    public class AdminController : BaseController
    {


        public ActionResult Users(int? p)
        {
            int totalRecords = 0;
            int pageIndex = 0;

            if (p.HasValue)
                pageIndex = p.Value;

            MembershipUserCollection uList = Membership.GetAllUsers(pageIndex, 10, out totalRecords);

            List<Models.UsersModel> dispList = new List<Models.UsersModel>();

            foreach (MembershipUser usr in uList)
            {
                dispList.Add(new Models.UsersModel().FromMembershipUser(usr));
            }

            

            return View(dispList);
        }

        

        #region Users & Roles
        ////[Authorize(Roles = "Administrator")]
        //public ActionResult Users()
        //{
        //    int totalRecords = 0;
        //    //ViewBag["totalRecords"] = totalRecords;
        //    MembershipUserCollection uList = Membership.GetAllUsers(0, 10, out totalRecords);

        //    List<Models.UsersModel> dispList = new List<Models.UsersModel>();

        //    foreach (MembershipUser usr in uList)
        //    {
        //        dispList.Add(new Models.UsersModel().FromMembershipUser(usr));
        //    }

        //    return View(dispList);
        //}

        public ActionResult UserDetails(int id)
        {
            Models.EditUserModel model=new Models.EditUserModel();

                model.userInfo = (new Models.UsersModel()).FromMembershipUser(Membership.GetUser(id));
                model.userInfo.roles = Roles.GetRolesForUser(model.userInfo.name);

            model.allRoles = Roles.GetAllRoles();
            
            return View(model);
        }

        [HttpPost]
        public ActionResult UserDetails(int id, Models.EditUserModel model)
        {
            MembershipUser usr = Membership.GetUser(id);

            switch (model.submitButton)
            {
                case ("UnlockUser"):
                    {
                        bool flag = usr.UnlockUser();
                        TempData["message"] = "The requested user is unlocked now.";
                        return RedirectToAction("UserDetails", new { id = id });
                        break;
                    }

                case ("AdminReset"):
                    {
                        string newPass = "";
                        usr.UnlockUser();
                        Parichay.Security.NHibernateMembershipProvider nhm = (Parichay.Security.NHibernateMembershipProvider)Membership.Providers[((System.Web.Configuration.MembershipSection)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/membership")).DefaultProvider];
                        //bool admValid = nhm.ValidateUser(model.admUsrName, model.admPass);
                        newPass = nhm.AdminResetPassword(usr.UserName);

                        TempData["message"] = "This User's password has been resetted now. The new password is : " + newPass + " .You can use this new password to update user's information.";
                        return RedirectToAction("UserDetails", new { id = id });
                        break;
                    }
                case ("Update"):
                    {
                        //usr.PasswordQuestion = model.passwordQuestion;
                        usr.IsApproved = model.userInfo.isApproved;
                        //usr.IsLockedOut = model.isLockedOut;
                        usr.Comment = model.userInfo.comments;
                        Membership.UpdateUser(usr);

                        TempData["message"] = "User's information is updated now. The new information is: <br/>Comments:" + model.userInfo.comments + "<br/>Approval Status:" + usr.IsApproved.ToString();

                        return RedirectToAction("UserDetails", new { id = id });
                        break;
                    }
                case ("ChangeUsersQuestionAndAnswer"):
                    {
                        if ((string.IsNullOrEmpty(model.userInfo.passwordQuestion)) && (!Membership.ValidateUser(usr.UserName, model.userInfo.passwordAnswer)))
                        {
                            TempData["message"] = "<b><span style='font-size:14px'>Blank or Incorrect User's Password was provided.</span><br/>Correct User's Password is required to update user's information. <b> Please use the Admin Reset User's Password button below to reset user's password.</b>";
                        }
                        else
                        {
                            bool flgPassQChange = usr.ChangePasswordQuestionAndAnswer(model.userInfo.password, model.userInfo.passwordQuestion, model.userInfo.passwordAnswer);
                            if (flgPassQChange)
                            {
                                TempData["message"] = "User's password question and answer is updated now. The new information is:<br/>Password Question:" + model.userInfo.passwordQuestion + "<br/>Password Answer:" + model.userInfo.passwordAnswer;
                            }
                            else
                            {
                                TempData["message"] = "Some Error has occured. Unable to update password question and answer.";
                            }
                        }


                        return RedirectToAction("UserDetails", new { id = id });
                        break;
                    }
                case ("Destroy"):
                    {
                        try
                        {
                            DestroyUser(usr);
                            string logMsg = string.Format("Super Admin UserId : {0} , IP Address : {1}  permanently deleted the complete user information for UserId: {2}.", LoggedInUserName, LoggedInUserIp, usr.UserName);
                            Data.Helper.NHibernateHelper.Log(logMsg, Data.Helper.NHibernateHelper.LogType.Warn);
                            TempData["message"] = "All the information related to the requested user has been deleted.";
                            return RedirectToAction("Users", "Admin");
                        }
                        catch (Exception exc)
                        {
                            TempData["message"] = "Unable to delete User. Error:" + exc.Message;
                        }


                        break;
                    }
                default:
                    {
                        TempData["message"] = "Unable to process your request.";
                        break;
                    }
            }




            model = new Models.EditUserModel() { userInfo = (new Models.UsersModel()).FromMembershipUser(usr) };
            model.userInfo.roles = Roles.GetRolesForUser(model.userInfo.name);
            model.allRoles = Roles.GetAllRoles();

            return View(model);
        }

        private void DestroyUser(MembershipUser usr)
        {
            if (Int32.Parse(usr.ProviderUserKey.ToString()) == LoggedInUserKey)
            {
                throw new Exception("You cannot delete yourself.");
            }
            object obj7 = Data.Helper.NHibernateHelper.Delete("from MemberAlert a where a.PUser.Id=?", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);
            object obj8 = Data.Helper.NHibernateHelper.Delete("from MemberFriends a where a.MemberDetails.Id=? or a.Friendid.Id=?", new object[] { usr.ProviderUserKey, usr.ProviderUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 });
            //object obj2 = Data.Helper.NHibernateHelper.Delete("from MemberGroupmessages a where a.Sender.Id=? and a.Parent is null", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);
            //object obj2 = Data.Helper.NHibernateHelper.Delete("from MemberGroupmessages a where a.Sender.Id=?", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);
            object obj3 = Data.Helper.NHibernateHelper.Delete("from MemberGroupmembers a where a.MemberDetails.Id=?", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);
            //object obj4 = Data.Helper.NHibernateHelper.Delete("from MemberMessage a where a.Sender.Id=? or a.Recipient.Id=?", new object[] { usr.ProviderUserKey, usr.ProviderUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 });
            object obj5 = Data.Helper.NHibernateHelper.Delete("from MemberRequests a where a.Senderid.Id = ? or a.Recipientid.Id = ?", new object[] { usr.ProviderUserKey, usr.ProviderUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 });
            object obj6 = Data.Helper.NHibernateHelper.Delete("from MemberInvitations a where a.Senderid.Id = ? or a.Email = ?", new object[] { usr.ProviderUserKey, usr.Email }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.String });
            object obj11 = Data.Helper.NHibernateHelper.Delete("from MemberAbout m where m.Id=?", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);
            object obj12 = Data.Helper.NHibernateHelper.Delete("from MemberUploads m where m.Owner.Id=?", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);
            object obj17 = Data.Helper.NHibernateHelper.Delete("from MemberBlog m where m.Id=?", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);

            System.Collections.IList lstGrpChildMsg = Data.Helper.NHibernateHelper.Find("from MemberGroupmessages a where ((a.Sender.Id=? or a.Recipient.Id=?) and (a.Parent.Id is not null))", new object[] { usr.ProviderUserKey, usr.ProviderUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 },false);

            foreach (Parichay.Data.Entity.MemberGroupmessages itm in lstGrpChildMsg)
            {
                itm.Parent = null;
                itm.Recipient = null;
                Parichay.Data.Helper.NHibernateHelper.Update(itm);
                Parichay.Data.Helper.NHibernateHelper.Delete(itm);
            }

            System.Collections.IList lstChildMsg = Data.Helper.NHibernateHelper.Find("from MemberMessage a where ((a.Sender.Id=? or a.Recipient.Id=?) and (a.ParentId.Id is not null))", new object[] { usr.ProviderUserKey, usr.ProviderUserKey }, new NHibernate.Type.IType[] { NHibernate.NHibernateUtil.Int32, NHibernate.NHibernateUtil.Int32 },false);

            foreach (Parichay.Data.Entity.MemberMessage itm in lstChildMsg)
            {
                itm.ParentId = null;
                itm.Recipient = null;
                Parichay.Data.Helper.NHibernateHelper.Update(itm);
                Parichay.Data.Helper.NHibernateHelper.Delete(itm);
            }

            object obj2 = Data.Helper.NHibernateHelper.Delete("from MemberGroupmessages a where a.Sender.Id=?", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);
            obj2 = Data.Helper.NHibernateHelper.Delete("from MemberGroupmessages a where a.Recipient.Id=?", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);

            object obj16 = Data.Helper.NHibernateHelper.Delete("from MemberMessage a where a.Sender.Id=?", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);
            obj16 = Data.Helper.NHibernateHelper.Delete("from MemberMessage a where a.Recipient.Id=?", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);
            
            object obj14 = Data.Helper.NHibernateHelper.Delete("from MemberDetails a where a.Id=?", usr.ProviderUserKey, NHibernate.NHibernateUtil.Int32);

            Membership.DeleteUser(usr.UserName, true);
        }

        [HttpPost]
        public ActionResult ResetUserPass(FormCollection collection)
        {
            if (string.IsNullOrEmpty(collection["txtAddRoleName"]))
            { TempData["message"] = "Role Name cannot be null."; return RedirectToAction("EditRoles"); }

            try
            {
                // TODO: Add insert logic here
                TempData["message"] = "Required Role Name is added now";
                //Parichay.Security.ADHSecurityHelper.CreateAction(collection["ControllerName"], collection["ActionName"]);
                Roles.CreateRole(collection["txtAddRoleName"]);
                return RedirectToAction("EditRoles");
            }
            catch
            {
                return RedirectToAction("EditRoles");
            }
        }

        //[Authorize(Roles = "Administrator")]
        public ActionResult EditRoles()
        {
            ViewData["roleList"] = Roles.GetAllRoles();

            return View();
        }


        public ActionResult DeleteRole(string roleName)
        {
            try
            {
                if (roleName != "Super Admin")
                {
                    Roles.DeleteRole(roleName);
                    TempData["message"] = "Requested Role is deleted now";
                }
                else { TempData["message"] = "Super Admin role cannot be deleted."; }
            }
            catch
            {
                throw;
            }

            return (RedirectToAction("EditRoles"));
        }

        [HttpPost]
        public ActionResult AddRole(FormCollection collection)
        {
            if (string.IsNullOrEmpty(collection["txtAddRoleName"]))
            { TempData["message"] = "Role Name cannot be null."; return RedirectToAction("EditRoles"); }

            try
            {
                // TODO: Add insert logic here
                TempData["message"] = "Required Role Name is added now";
                //Parichay.Security.ADHSecurityHelper.CreateAction(collection["ControllerName"], collection["ActionName"]);
                Roles.CreateRole(collection["txtAddRoleName"]);
                return RedirectToAction("EditRoles");
            }
            catch
            {
                return RedirectToAction("EditRoles");
            }
        }

        [HttpPost]
        public ActionResult RemoveUserRole(FormCollection collection)
        {
            string UsrKey = collection["hdnUserKy"];
            string UsrName = collection["hdnUsrName"];
            string RoleName = collection["hdnRoleNm"];

            if (string.Equals(UsrKey, LoggedInUserKey.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                TempData["message"] = "You cannot remove yourself from a role.";
                return RedirectToAction("UserDetails", new { id = UsrKey });
            }

            if (string.Equals(RoleName,"Admin",StringComparison.InvariantCultureIgnoreCase))
            {
                TempData["message"] = "User: " + UsrName + " cannot be removed from the role: " + RoleName+". Currently Admins cannot be removed.";
                return RedirectToAction("UserDetails", new { id = UsrKey });
            }

            
            Roles.RemoveUserFromRole(UsrName, RoleName);

            TempData["message"] = "User: " + UsrName + " is now removed from the role: " + RoleName;
            return RedirectToAction("UserDetails", new { id = UsrKey });
        }

        [HttpPost]
        public ActionResult AddUserRole(FormCollection collection)
        {
            string UsrKey = collection["hdnUserKy"];
            string UsrName = collection["hdnUsrName"];
            string RoleName = collection["hdnRoleNm"];

            Roles.AddUserToRole(UsrName, RoleName);

            TempData["message"] = "User: " + UsrName + " has been assigned to the role: " + RoleName;
            return RedirectToAction("UserDetails", new { id = UsrKey });
        }
        //
        // GET: /Admin/Details/5

        #endregion Users & Roles

        #region Permission Rules

        public ActionResult Permissions()
        {
            Models.Permissions perm = new Models.Permissions();




            //  IEnumerable<Parichay.Security.Entity.RoleActions> rslt;
            try
            {
                string[] roles = Roles.GetAllRoles();

                for (int count1 = 0; count1 <= roles.Length - 1; count1++)
                {
                    perm.rolesList.Add(new SelectListItem { Text = roles[count1], Value = roles[count1] });
                }

                perm.actionsList = Parichay.Security.ADHSecurityHelper.getAllActions();
                perm.permissionsList = Parichay.Security.ADHSecurityHelper.getAllPermissions();

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            if (TempData["message"] != null)
            {
                ModelState.AddModelError("", TempData["message"].ToString());
            }

            return View(perm);
        }

        public ActionResult UserLogs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Permissions(Models.Permissions model)
        {
            // Models.Permissions model = new Models.Permissions();

            Parichay.Security.ViewModel.Permission toCreate = new Parichay.Security.ViewModel.Permission();






            //  IEnumerable<Parichay.Security.Entity.RoleActions> rslt;

            try
            {

                model.actionsList = Parichay.Security.ADHSecurityHelper.getAllActions();

                //if (string.IsNullOrEmpty(collection["selectedRoleName"]))
                //{
                //    throw new Exception("Please Select a Role First.");
                //}
                //else
                //{
                //    toCreate.Roles = collection["selectedRoleName"];
                //}

                //if (collection["selectedActionId"] == "-1")
                //{
                //    throw new Exception("Please Select An Action - Controller.");
                //}

                if (string.IsNullOrEmpty(model.selectedRoleName))
                {
                    throw new Exception("Please Select a Role First.");
                }
                else
                {
                    toCreate.Role = model.selectedRoleName;
                }

                if (model.selectedActionId == -1)
                {
                    throw new Exception("Please Select An Action - Controller.");
                }
                else
                {

                    foreach (Parichay.Security.ViewModel.Activities item in model.actionsList)
                    {
                        if (item.Id == model.selectedActionId)
                        {
                            toCreate.Controller = item.ControllerName;
                            toCreate.Action = item.ActionName;
                            toCreate.ControllerId = item.Id;
                        }
                    }
                }



                toCreate.PermissionType = 0;

                //Creating permission Now
                Parichay.Security.ADHSecurityHelper.CreatePermission(toCreate);
                TempData["message"] = "The Requested Role Based Permission Rule is Created now";

            }
            catch (Exception ex)
            {
                TempData["message"] = "Oops. We got and Error:-" + ex.Message + ex.StackTrace;
            }
            finally
            {
                string[] roles = Roles.GetAllRoles();

                for (int count1 = 0; count1 <= roles.Length - 1; count1++)
                {
                    model.rolesList.Add(new SelectListItem { Text = roles[count1], Value = roles[count1] });
                }

                model.permissionsList = Parichay.Security.ADHSecurityHelper.getAllPermissions();

            }

            
            return RedirectToAction("Permissions");

        }

       
        [HttpPost]
        public ActionResult UserPermission(Models.Permissions model)
        {
            // Models.Permissions model = new Models.Permissions();

            Parichay.Security.ViewModel.Permission toCreate = new Parichay.Security.ViewModel.Permission();







            //  IEnumerable<Parichay.Security.Entity.RoleActions> rslt;

            try
            {

                model.actionsList = Parichay.Security.ADHSecurityHelper.getAllActions();

                //if (string.IsNullOrEmpty(collection["selectedRoleName"]))
                //{
                //    throw new Exception("Please Select a Role First.");
                //}
                //else
                //{
                //    toCreate.Roles = collection["selectedRoleName"];
                //}

                //if (collection["selectedActionId"] == "-1")
                //{
                //    throw new Exception("Please Select An Action - Controller.");
                //}

                if (string.IsNullOrEmpty(model.selectedUserId))
                {
                    throw new Exception("Please Enter a User Id First.");
                }
                else if (((MembershipUser)Membership.GetUser(model.selectedUserId)) == null)
                {
                    throw new Exception("User Not Found. Please select another User.");
                }
                else
                {
                    toCreate.Users = model.selectedUserId;
                }

                if (model.selectedActionIdUser == null)
                {
                    throw new Exception("Please Select An Action - Controller.");
                }
                else
                {

                    foreach (Parichay.Security.ViewModel.Activities item in model.actionsList)
                    {
                        if (item.Id == model.selectedActionIdUser)
                        {
                            toCreate.Controller = item.ControllerName;
                            toCreate.Action = item.ActionName;
                            toCreate.ControllerId = item.Id;
                        }
                    }
                }



                toCreate.PermissionType = 0;

                //Creating permission Now
                Parichay.Security.ADHSecurityHelper.CreatePermission(toCreate);
                TempData["message"] = "The Requested User Based Permission Rule is Created now";
            }
            catch (Exception ex)
            {
                TempData["message"] = "Oops. We got and Error:-" + ex.Message + ex.StackTrace;
            }
            finally
            {
                string[] roles = Roles.GetAllRoles();

                for (int count1 = 0; count1 <= roles.Length - 1; count1++)
                {
                    model.rolesList.Add(new SelectListItem { Text = roles[count1], Value = roles[count1] });
                }

                model.permissionsList = Parichay.Security.ADHSecurityHelper.getAllPermissions();

            }

            return RedirectToAction("Permissions");

        }

        [HttpPost]
        public ActionResult DeletePermission(FormCollection collection)
        {

            try
            {
                int id = int.Parse(collection["hdnPermId"]);
                Parichay.Security.ADHSecurityHelper.DeletePermission(id);
                TempData["message"] = "Requested permission is deleted now";
            }
            catch
            {
                throw;
            }

            return (RedirectToAction("Permissions"));
        }

        [HttpPost]
        public ActionResult AddAction(FormCollection collection)
        {
            if ((string.IsNullOrEmpty(collection["ControllerName"])) || (string.IsNullOrEmpty(collection["ActionName"])))
            { TempData["message"] = "ControllerName and ActionName cannot be null."; return RedirectToAction("Permissions"); }

            try
            {
                // TODO: Add insert logic here
                TempData["message"] = "Required Controller Action is added now";
                Parichay.Security.ADHSecurityHelper.CreateAction(collection["ControllerName"], collection["ActionName"]);

            }
            catch
            {
                throw;
            }
            return RedirectToAction("Permissions");
        }

        [HttpPost]
        public ActionResult DeleteAction(FormCollection collection)
        {
            try
            {
                int id = int.Parse(collection["hdnActionId"]);
                Parichay.Security.ADHSecurityHelper.DeleteAction(id);
                TempData["message"] = "Requested Action is deleted now";
            }
            catch
            {
                throw;
            }

            return (RedirectToAction("Permissions"));
        }

        #endregion Permission Rules

        #region Basic CRUD

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Admin/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Admin/Create

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
        // GET: /Admin/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Edit/5

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
        // GET: /Admin/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Delete/5

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
        #endregion Basic CRUD


        public ActionResult SystemLogs(int? p)
        {
            int pageIndex = 0;
            if (p.HasValue)
                pageIndex = p.Value;

            IList<Data.Entity.Log> lstLogs = Data.Helper.NHibernateHelper.ConvertToListOf<Data.Entity.Log>(Parichay.Data.Helper.NHibernateHelper.Find("select l from Log l where l.Logger=? order by l.Date desc", "parichay", NHibernate.NHibernateUtil.String, pageIndex, 10, false));
            return View(lstLogs);
        }

        [HttpPost]
        public ActionResult DeleteLogEntry(int deleteLogId)
        {
            try
            {
                Parichay.Data.Helper.NHibernateHelper.Delete<Parichay.Data.Entity.Log>(Parichay.Data.Helper.NHibernateHelper.UniqueResult<Parichay.Data.Entity.Log>(null, "Id", deleteLogId));
                TempData["message"] = "The Requested Entry is DELETED Now.";
            }
            catch (Exception excp)
            {
                TempData["message"] = "Unable to delete the requested log entry. Error: " + excp.Message;

            }
            return RedirectToAction("SystemLogs");
        }

    }
}
