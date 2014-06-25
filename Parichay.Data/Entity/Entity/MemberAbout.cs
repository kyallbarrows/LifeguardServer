using System;
using System.Collections.Generic;
using System.Text;

namespace Parichay.Data.Entity
{
        /// <summary>
    /// An object representation of the member_abour table
    /// </summary>
    [Serializable]
    public partial class MemberAbout
    {
        protected System.Int32 _Id;
        //private MemberDetails _PUser;
        private System.String _AboutText;

        public virtual System.Int32 Id
        {
            get { return _Id; }
            set { _Id = value; }
        }


        public virtual System.String AboutText
        {
            get
            {
                return _AboutText;
            }
            set
            {
                _AboutText = value;
            }
        }


        //public virtual MemberDetails PUser
        //{
        //    get { return _PUser; }
        //    set { _PUser = value; }
        //}


        protected bool Equals(MemberAbout entity)
        {
            if (entity == null) return false;
            if (!base.Equals(entity)) return false;
            if (!Equals(_Id, entity._Id)) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as MemberAbout);
        }

        public override int GetHashCode()
        {
            int result = base.GetHashCode();
            result = 29 * result + _Id.GetHashCode();
            return result;
        }
    }
}
