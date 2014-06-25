using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using System.Web;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Parichay.MVC.Models
{
    public class EditUserModel
    {
        public UsersModel userInfo { get; set; }
        public string submitButton { get; set; }
        public string[] allRoles { get; set; } 
    }

    public class UsersModel
    {
        #region Fields
        public int id;
        public string name;
        public string loweredName;
        public string description { get; set; }
        public string password { get; set; }
        public int passwordFormat { get; set; }
        public string passwordSalt { get; set; }
        public string email;
        public string loweredEmail { get; set; }
        public string passwordQuestion { get; set; }
        public string passwordAnswer { get; set; }
        public string comments { get; set; }
        public bool isApproved { get; set; }
        public bool isLockedOut { get; set; }
        public DateTime creationDate;
        public DateTime lastActivityDate { get; set; }
        public DateTime lastLoginDate { get; set; }
        public DateTime lastLockedOutDate { get; set; }
        public DateTime lastPasswordChangeDate { get; set; }
        public int failedPasswordAttemptCount;
        public DateTime failedPasswordAttemptWindowStart;
        public int failedPasswordAnswerAttemptCount;
        public DateTime failedPasswordAnswerAttemptWindowStart;
        public IList applications { get; set; }
        public IList roles { get; set; }
        #endregion Fields



        #region Operations
        public MembershipUser ToMembershipUser(string providerName)
        {
            return (new MembershipUser(providerName, name, id, email, passwordQuestion, comments, isApproved,
                                       isLockedOut, creationDate, lastLoginDate, lastActivityDate, lastPasswordChangeDate,
                                       lastLockedOutDate));
        }
        public UsersModel FromMembershipUser(MembershipUser mu)
        {
            id =  (int)mu.ProviderUserKey;
            name = mu.UserName;
            email = mu.Email;
            passwordQuestion = mu.PasswordQuestion;
            comments = mu.Comment;
            isApproved = mu.IsApproved;
            isLockedOut = mu.IsLockedOut;
            creationDate = mu.CreationDate;
            lastActivityDate = mu.LastActivityDate;
            lastLoginDate = mu.LastLoginDate;
            lastPasswordChangeDate = mu.LastPasswordChangedDate;
            lastLockedOutDate = mu.LastLockoutDate;
            return this;
        }
        #endregion Operations
    }

    public class Permissions
    {

        public int? selectedActionId { get; set; }

        public int? selectedActionIdUser { get; set; }


        public int selectedUserKey { get; set; }
        public string selectedUserId { get; set; }
        public string selectedRoleName { get; set; }

        public IList<Parichay.Security.ViewModel.Permission> permissionsList { get; set; }
        public List<SelectListItem> rolesList { get; set; }
        public IList<Parichay.Security.ViewModel.Activities> actionsList { get; set; }
        public List<SelectListItem> PermTypes { get; set; }

        public int? PTypeR { get; set; }
        public int? PTypeU { get; set; }

        public Permissions()
        {
            rolesList = new List<SelectListItem>();
            PermTypes = new List<SelectListItem>();
            PermTypes.Add(new SelectListItem { Text = "Read", Value = "0" });
            PermTypes.Add(new SelectListItem { Text = "Write", Value = "1" });
            
        }

        public enum permissionTypes
        {
            ReadOnly = 0, Writable
        };
    }
}