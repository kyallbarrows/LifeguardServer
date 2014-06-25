using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Parichay.Data.Entity
{
    /// <summary>
    /// An object representation of the Member_Friends table
    /// </summary>
    [Serializable]
    public class MemberFriends
    {
        protected System.Int32 _Id;

        private System.DateTime _Createdon;
        private System.Int32 _Isfavorite;
        private MemberDetails _Friendid;
        private MemberDetails _MemberDetails;
        private System.DateTime _Modifiedon;

        public virtual System.DateTime Createdon
        {
            get
            {
                return _Createdon;
            }
            set
            {
                _Createdon = value;
            }
        }

        public virtual System.Int32 Isfavorite
        {
            get
            {
                return _Isfavorite;
            }
            set
            {
                _Isfavorite = value;
            }
        }

        [Required]
        public virtual MemberDetails Friendid
        {
            get
            {
                return _Friendid;
            }
            set
            {
                _Friendid = value;
            }
        }

        public virtual System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }

        [Required]
        public virtual MemberDetails MemberDetails
        {
            get
            {
                return _MemberDetails;
            }
            set
            {
                _MemberDetails = value;
            }
        }
        public virtual System.DateTime Modifiedon
        {
            get
            {
                return _Modifiedon;
            }
            set
            {
                _Modifiedon = value;
            }
        }

        public bool bIsfavorite
        {
            get { return (_Isfavorite != 0); }
            set { _Isfavorite = value ? 1 : 0; }
        }
    }
}
