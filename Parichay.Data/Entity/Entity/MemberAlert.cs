using System;
using System.Collections.Generic;
using System.ComponentModel;
using Iesi.Collections;
using Iesi.Collections.Generic;



using Parichay.Data.Entity;

namespace Parichay.Data.Entity
{
    /// <summary>
    /// An object representation of the Member_Alert table
    /// </summary>
    [Serializable]
    public partial class MemberAlert
    {
        protected System.Int32 _Id;

        private System.DateTime _Version;
        private Nullable<System.Int32> _Ishidden;
        private System.String _Message;
        private Alerttype _AlertType;
        private MemberDetails _PUser;

        //UI properties
        public IList<MemberAlert> MyAlerts { get; set; }

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

        public virtual Nullable<System.Int32> Ishidden
        {
            get
            {
                return _Ishidden;
            }
            set
            {
                _Ishidden = value;
            }
        }

        public virtual System.String Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
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

        public virtual Alerttype AlertType
        {
            get
            {
                return _AlertType;
            }
            set
            {
                _AlertType = value;
            }
        }
        public virtual MemberDetails PUser
        {
            get
            {
                return _PUser;
            }
            set
            {
                _PUser = value;
            }
        }

        public string CreatedAgo
        { get { return Version.Ago(); } }
    }
}
