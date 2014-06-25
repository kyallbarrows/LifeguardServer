using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Parichay.MVC.Models;
using Parichay.Data.Entity;
using Parichay.Data.Helper;

namespace Parichay.MVC.Controllers
{

    [HandleError]
    public class AccountController : BaseController
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    MembershipUser currentUser = Membership.GetUser(model.UserName, true);
                    if ((currentUser != null) && (currentUser.IsLockedOut))
                    {
                        TempData["message"] = "This user Id has been locked due to exceeding the number of invalid password attempts. Please visit <a href='RequestUnlock/" + model.UserName + "'>Request Unlock</a> to place a request to unlock your account.";
                    }
                    else if ((currentUser != null) && (!currentUser.IsApproved))
                    {
                        TempData["message"] = "This user Id has been temporarily suspended at admin end. Please visit <a href='RequestUnlock/" + model.UserName + "'>Request Unlock</a> to place a request to unlock your account.";
                    }
                    else if (currentUser != null)
                    {
                        TempData["message"] = "Forgot your password? Please click <a href='RecoverPass'>this link</a> to go to password recovery.";
                    }
                    else
                    {
                        TempData["message"] = "No such user with provided User ID exist. Please click Sign-Up below to create an account.";
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email,model.PasswordQuestion,model.PasswordAnswer);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    try
                    {
                        MembershipUser thisUsr = Membership.GetUser(model.UserName,true);
                        MemberDetails toCreate = new MemberDetails();
                        toCreate.Id = Int32.Parse(thisUsr.ProviderUserKey.ToString());
                        toCreate.PEmail = thisUsr.Email;
                        toCreate.Givennm = model.Givennm;
                        toCreate.bShowPrvInfo = false;
                        Data.Helper.NHibernateHelper.Save<MemberDetails>(toCreate);
                    }
                    catch (Exception ex1)
                    {
                        Data.Helper.NHibernateHelper.Log(new Exception("Problem creating default user profile==>", ex1));
                    }

                    //****Remove this code block if you already have another "admin".*****//
                    //To delete this "sysadmin" => 1. Create another account>> 2. Assign admin role to new account >> 3. Delete the default "sysadmin" account
                    if ((string.Equals(model.UserName,"sysadmin",StringComparison.InvariantCultureIgnoreCase))&&(Roles.GetAllRoles().Length == 0))
                    {
                        Roles.CreateRole("Admin");
                        Roles.AddUserToRole(model.UserName, "Admin");
                    }
                    //****Remove this code block if you already have a "admin".*****//

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }
            else
            {
                ModelState.AddModelError("", "Please fill in all required information.");
            }


            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult Index(int? id)
        {
            int uId = id.HasValue ? id.Value : LoggedInUserKey;

            UserHomeModel model = new UserHomeModel();

            //This is the current logged in user viewing his own profile
            if (uId == LoggedInUserKey)
            {
                model.IsCurrentUser = true;
                model.myInfo = Parichay.Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);

                if ((string.IsNullOrEmpty(model.myInfo.Surnm) || (string.IsNullOrEmpty(model.myInfo.TitleC)) || (string.IsNullOrEmpty(model.myInfo.CtryC))))
                {
                    TempData["message"]="<b>Please fill in your basic information. Click on the 'Edit Personal Info' link on the home page to proceed.</b>";
                }

                model.myMessages = getMessagesByUId(LoggedInUserKey);
                model.myFriendMsg = getFriendMessagesByUId(LoggedInUserKey);
                model.myAlerts = getAlertsByUId(LoggedInUserKey);
                model.myRequests = getRequestsByUId(LoggedInUserKey);
            }
            else
            {
                model.myInfo = Parichay.Data.Helper.NHibernateHelper.FetchProjection<MemberDetails>(new string[] { "Id", "Nicknm", "Surnm", "Givennm", "TitleC", "GenderC", "Institute", "CtryC", "ShowPrvInfo" }, "Id", uId, 0, 1, false, null, false)[0];
                
                //This user really wants to show his entire personal info to all.
                //To-do => add logic to to control the view of personal info on the basis of ShowPrivInfo which should be set up from among the access types => Public, Friends, Self etc.
                if ((model.myInfo!=null)&&(model.myInfo.bShowPrvInfo))
                {
                    model.myInfo = Parichay.Data.Helper.NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", uId);
                }
            }
            
            return View(model);
        }

        public ActionResult ChangePassQnA()
        {
            ChangePassQnA model = new ChangePassQnA();
            MembershipUser usr = Membership.GetUser(LoggedInUserKey);

            model.PasswordQuestion = usr.PasswordQuestion;

            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassQnA(ChangePassQnA model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MembershipUser usr = Membership.GetUser(LoggedInUserKey);

                    usr.ChangePasswordQuestionAndAnswer(model.AccPassword, model.PasswordQuestion, model.PasswordAnswer);
                    TempData["message"] = "Password Question and Answer successfully changed.";
                    return RedirectToAction("Index");
                }
                catch (Exception exc1)
                {
                    TempData["message"] = "Unable to change your Password question and answer. Error: " + exc1.Message;
                    //return RedirectToAction("Info");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Please fill in the required information.");
                return View(model);
            }
        }

        public ActionResult Edit()
        {
            MemberDetails model = NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MemberDetails model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MemberDetails usrPartics = NHibernateHelper.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);

                    if (usrPartics != null)
                    {
                        NHibernateHelper.Update<MemberDetails>(model);
                    }
                    else
                    {
                        NHibernateHelper.Save<MemberDetails>(model);
                    }
                    TempData["message"] = "Profile updated successfully please click <a href='Index'>here</a> to go to home page.";

                }
                catch (Exception ex1)
                {
                    TempData["message"] = "Error saving profile info=>" + ex1.Message;
                    NHibernateHelper.Log(new Exception("Error saving profile info=>", ex1));
                }
            }
            else
            {
                ModelState.AddModelError("", "Please fill the basic required information.");
            }
            return View(model);
        }
        public ActionResult RequestUnlock(string id)
        {
            SendMail(AppConstants.adminEmail,"Parichay: Passw rd Unlock Request","Hello Admin \n\n User "+id+" has requested a password unlock. Kindly assist. \n\n Thanks\nAdmin");
            //SendMail("", "", "");
            TempData["message"] = "<b>A request to unlock your account has been placed to the admin. You will receive a notification on your primary e-mail address once the account is unlocked.</b>";

            return RedirectToAction("LogOn");
        }

        public ActionResult RecoverPass()
        {
            return View(new PassRecovery1Model() { stepN = 0 });
        }

        [HttpPost]
        public ActionResult RecoverPass(PassRecovery1Model model)
        {

            MembershipUser currentUsr;

            switch (model.submitButton)
            {
                case ("Find"):
                default:
                    if (string.IsNullOrEmpty(model.UserName) && string.IsNullOrEmpty(model.Email))
                    {
                        ModelState.AddModelError("", "Please fill in the required information.");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.UserName))
                            currentUsr = Membership.GetUser(model.UserName.Trim());
                        else
                            currentUsr = Membership.GetUser(Membership.GetUserNameByEmail(model.Email.Trim()));

                        if (currentUsr == null)
                        { TempData["message"] = "No such user exists. Please Sign-Up for the new account <a href='Register'>here.</a>"; }
                        else
                        {

                            model.Email = currentUsr.Email;
                            model.PasswordQuestion = currentUsr.PasswordQuestion;
                            model.UserName = currentUsr.UserName;
                            model.stepN = 1;
                        }
                    }
                    return View("RecoverPass", model);


                case ("Start Reset"):
                    //if (model.stepN != 1)
                    //{ TempData["message"] = "Please Enter the Username or Email to search."; return View(model); }
                    //else
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(model.UserName))
                                currentUsr = Membership.GetUser(model.UserName.Trim());
                            else
                                currentUsr = Membership.GetUser(Membership.GetUserNameByEmail(model.Email.Trim()));

                            try
                            {
                                bool tmp1 = currentUsr.UnlockUser();
                                string newPass = currentUsr.ResetPassword(model.PasswordAnswer);
                                SendMail(currentUsr.Email,"Passw rd reset requested for Parichay", string.Format("Hello {0},\n\n Your new temporary Password: \n {1} \n\nThanks,\nAdmin",LoggedInUserName,newPass));
                                 Data.Helper.NHibernateHelper.Log("Successfull Password Reset Attempt. User: " + currentUsr.UserName + ". Email:" + currentUsr.Email + " <br/>Timestamp:" + DateTime.Now.ToString() + " <br/>User Ip: " + LoggedInUserIp, Data.Helper.NHibernateHelper.LogType.Info);
                                TempData["message"] = "A new password has been created and emailed to your primary email address. Please login with your new temporary password: "+newPass;
                            }
                            catch (Exception ex1)
                            {
                                Data.Helper.NHibernateHelper.Log("Unsuccessful Password Recovery Attempt. User: " + currentUsr.UserName + ". Email:" + currentUsr.Email + " <br/>Timestamp:" + DateTime.Now.ToString() + " <br/>User Ip: " + LoggedInUserIp + " Exception:" + ex1.Message + ex1.StackTrace, Data.Helper.NHibernateHelper.LogType.Warn);
                                SendMail(Parichay.AppConstants.adminEmail, "User Unable to recover password", "Hello Admin, \n\n User: " + currentUsr.UserName + " is unable to reset his password. Please assist. \n\nAdmin.");
                                TempData["message"] = "Unable to process your password reset request. And e-mail has been forwared to admin who will assist you shortly.";
                            }
                            return RedirectToAction("LogOn");

                        }
                        catch (Exception exc)
                        {
                            Data.Helper.NHibernateHelper.Log("Failed Password Reset Attempt. User: " + model.UserName + ". Email:" + model.Email + " <br/>Timestamp:" + DateTime.Now.ToString() + " <br/>User Ip: " + LoggedInUserIp, Data.Helper.NHibernateHelper.LogType.Info);
                            ModelState.AddModelError("", "Unable to Reset your password. Error: " + exc.Message);
                            return View(model);
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Please fill in the required information.");
                        return View(model);
                    }
            }




        }
        public ActionResult BioData()
        {
            Data.Entity.MemberAbout model = new Data.Entity.MemberAbout();
            System.Collections.IList obj = Data.Helper.NHibernateHelper.Find("from MemberAbout m where m.Id=?", LoggedInUserKey, NHibernate.NHibernateUtil.Int32, false);

            if ((null != obj) && (obj.Count != 0))
            {
                //AutoMapper.Mapper.CreateMap<MemberBiodata,MemberBiodataModel>();
                //model = AutoMapper.Mapper.Map<MemberBiodata, MemberBiodataModel>(m_PersistenceManager.ConvertToListOf<MemberBiodata>(obj)[0]);
                model.AboutText = (Data.Helper.NHibernateHelper.ConvertToListOf<MemberAbout>(obj)[0]).AboutText;
            }
            else
            {
                model.Id = LoggedInUserKey;
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult BioData(Data.Entity.MemberAbout model)
        {

            System.Collections.IList obj = Data.Helper.NHibernateHelper.Find("from MemberAbout m where m.Id=?", LoggedInUserKey, NHibernate.NHibernateUtil.Int32, false);

            try
            {
                //AutoMapper.Mapper.CreateMap<MemberBiodataModel, MemberBiodata>();
                //AutoMapper.Mapper.Map<MemberBiodataModel, MemberBiodata>(model);
                MemberAbout addBio = new MemberAbout();

                if ((null != obj) && (obj.Count != 0))
                {
                    addBio = Data.Helper.NHibernateHelper.ConvertToListOf<MemberAbout>(obj)[0];
                    addBio.AboutText = model.AboutText;
                    Data.Helper.NHibernateHelper.Update<MemberAbout>(addBio);
                }
                else
                {
                    //addBio.PUser = m_PersistenceManager.UniqueResult<MemberDetails>(null, "Id", LoggedInUserKey);
                    addBio.Id = LoggedInUserKey;
                    addBio.AboutText = model.AboutText;

                    Data.Helper.NHibernateHelper.Save<MemberAbout>(addBio);
                }


                TempData["message"] = "Bio Data added";

            }
            catch (Exception ex1)
            {
                TempData["message"] = "Error adding bio-data" + ex1.Message;
            }

            return View(model);
        }

    }
}

namespace Parichay.MVC.Models
{

    public class UserHomeModel
    {
        public MemberDetails myInfo { get; set; }

        public IList<MemberMessage> myMessages { get; set; }
        public IList<MemberGroupmessages> myGroupMsg { get; set; }
        public IList<MemberMessage> myFriendMsg { get; set; }
        public IList<MemberAlert> myAlerts { get; set; }
        public IList<MemberInvitations> myInvites { get; set; }
        public IList<MemberRequests> myRequests { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}