using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Parichay.Data.Entity
{
    /// <summary>
    /// An object representation of the Member_Groupmembers table
    /// </summary>
    [Serializable]
    public class MemberGroupmembers
    {
        protected System.Int32 _Id;

        private System.Int32 _Notifyonpending;
        private Nullable<System.Int32> _Status;
        private System.Int32 _Notifyonjoin;
        private System.Int32 _Notifyonreply;
        private System.Int32 _Notifyonmessage;
        private MemberDetails _MemberDetails;
        private MemberGroups _Group;
        private System.Int32 _Role;
        private System.Int32 _Notifyonleave;

        public virtual System.Int32 Notifyonpending
        {
            get
            {
                return _Notifyonpending;
            }
            set
            {
                _Notifyonpending = value;
            }
        }

        public virtual Nullable<System.Int32> Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }
        }

        public virtual System.Int32 Notifyonjoin
        {
            get
            {
                return _Notifyonjoin;
            }
            set
            {
                _Notifyonjoin = value;
            }
        }

        public virtual System.Int32 Notifyonreply
        {
            get
            {
                return _Notifyonreply;
            }
            set
            {
                _Notifyonreply = value;
            }
        }

        public virtual System.Int32 Notifyonmessage
        {
            get
            {
                return _Notifyonmessage;
            }
            set
            {
                _Notifyonmessage = value;
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
        public virtual MemberGroups Group
        {
            get
            {
                return _Group;
            }
            set
            {
                _Group = value;
            }
        }
        public virtual System.Int32 Role
        {
            get
            {
                return _Role;
            }
            set
            {
                _Role = value;
            }
        }

        public virtual System.Int32 Notifyonleave
        {
            get
            {
                return _Notifyonleave;
            }
            set
            {
                _Notifyonleave = value;
            }
        }

        public bool bNotifyonpending
        {
            get { return (_Notifyonpending != 0); }
            set { _Notifyonpending = value ? 1 : 0; }
        }

        public bool bStatus
        {
            get { return (_Status != 0); }
            set { _Status = value ? 1 : 0; }
        }
        public bool bNotifyonjoin
        {
            get { return (_Notifyonjoin != 0); }
            set { _Notifyonjoin = value ? 1 : 0; }
        }
        public bool bNotifyonreply
        {
            get { return (_Notifyonreply != 0); }
            set { _Notifyonreply = value ? 1 : 0; }
        }
        public bool bNotifyonmessage
        {
            get { return (_Notifyonmessage != 0); }
            set { _Notifyonmessage = value ? 1 : 0; }
        }
        public bool bNotifyonleave
        {
            get { return (_Notifyonleave != 0); }
            set { _Notifyonleave = value ? 1 : 0; }
        }
        public bool bRole
        {
            get { return (_Role != 0); }
            set { _Role = value ? 1 : 0; }
        }
    }
}
