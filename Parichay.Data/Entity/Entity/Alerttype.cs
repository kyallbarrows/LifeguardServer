using System;

using System.ComponentModel;
using Iesi.Collections;
using Iesi.Collections.Generic;



using Parichay.Data.Entity;

namespace Parichay.Data.Entity
{
    /// <summary>
    /// An object representation of the Alert_type table
    /// </summary>
    [Serializable]
    public partial class Alerttype
    {
        protected System.Int32 _Id;

        private System.Int32 _Paramcount;
        private System.String _SupportEmail;
        private System.String _Template;
        private readonly ISet<MemberAlert> _FKAlertAlerttype = new HashedSet<MemberAlert>();
        private System.String _Name;

        public virtual System.Int32 Paramcount
        {
            get
            {
                return _Paramcount;
            }
            set
            {
                _Paramcount = value;
            }
        }

        public virtual System.String SupportEmail
        {
            get
            {
                return _SupportEmail;
            }
            set
            {
                _SupportEmail = value;
            }
        }

        public virtual System.String Template
        {
            get
            {
                return _Template;
            }
            set
            {
                _Template = value;
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

        public virtual ISet<MemberAlert> FKAlertAlerttype
        {
            get
            {
                return _FKAlertAlerttype;
            }
        }

        public virtual System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value == null)
                {
                    throw new BusinessException("NameRequired", "Name must not be null.");
                }
                _Name = value;
            }
        }


    }
}
