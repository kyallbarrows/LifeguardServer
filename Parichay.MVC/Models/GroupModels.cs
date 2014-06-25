using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Parichay.Data.Entity;
using System.Web;

namespace Parichay.MVC.Models
{
       public class GroupDetailsModel
       {
           public MemberGroups groupDetails { get; set; }
           public IList<MemberGroupmessages> groupMessages { get; set; }
           public IList<MemberGroupmembers> groupMembers { get; set; }
           public string submitButton { get; set; }
           public IList<MemberDetails> UserSrchResult { get; set; }
           public string txtSearchUserNm { get; set; }
           public int loggedInUserId { get; set; }
           public bool showInvite { get; set; }
       }
       public class GroupHomeModel
       {
           public IList<MemberGroups> allGroups { get; set; }
           public IList<MemberGroups> myGroups { get; set; }
       }
}