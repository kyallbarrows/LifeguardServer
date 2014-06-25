using System;
using System.Collections.Generic;
using System.ComponentModel;
using Iesi.Collections;
using Iesi.Collections.Generic;



using Parichay.Data.Entity;

namespace Parichay.Data.Entity
{
    /// <summary>
    /// An object  of the Member_Invitations table
    /// </summary>
    [Serializable]
    public partial class MemberInvitations
    {
        protected System.Int32 _Id;

        private System.DateTime _Version;
        private System.String _Guid;
        private System.String _Email;
        private MemberDetails _Senderid;
        private Nullable<System.Int32> _BecameUserId;
        private Nullable<System.DateTime> _Createdate;
        private System.Int32 _Status;

        private Nullable<System.Int32> _TargetPageid;

        //UI properties
        public IList<MemberInvitations> MyInvites { get; set; }
        public string UserMessage { get; set; }
        public bool IsRecipient { get; set; }
        public string submitButton { get; set; }
        //UI properties

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

        public virtual MemberDetails Senderid
        {
            get
            {
                return _Senderid;
            }
            set
            {
                _Senderid = value;
            }
        }

        public virtual System.String Guid
        {
            get
            {
                return _Guid;
            }
            set
            {
                _Guid = value;
            }
        }

        public virtual System.String Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
            }
        }

        public virtual Nullable<System.Int32> BecameUserId
        {
            get
            {
                return _BecameUserId;
            }
            set
            {
                _BecameUserId = value;
            }
        }


        public virtual Nullable<System.Int32> TargetPageid
        {
            get { return _TargetPageid; }
            set { _TargetPageid = value; }
        }


        public virtual System.Int32 Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public virtual Nullable<System.DateTime> Createdate
        {
            get
            {
                return _Createdate;
            }
            set
            {
                _Createdate = value;
            }
        }

        public virtual System.DateTime Version
        {
            get
            {
                return _Version;
            }
            set
            {
                _Version = value;
            }
        }

        private System.Int32 _Type;

        public System.Int32 Type
        {
            get { return _Type; }
            set { _Type = value; }
        }


        public bool bStatus
        {
            get { return (_Status != 0); }
            set { _Status = value ? 1 : 0; }
        }
    }
}
