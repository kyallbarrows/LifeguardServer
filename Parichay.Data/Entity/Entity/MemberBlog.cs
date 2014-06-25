using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Parichay.Data.Entity
{
    /// <summary>
    /// An object representation of the member_blog table
    /// </summary>
    [Serializable]
    public partial class MemberBlog
    {
        protected System.Int32 _Id;
        private MemberDetails _PUser;
        private System.String _BlogText;
        private System.DateTime _Version;

        public virtual System.Int32 Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [Required]
        public virtual System.String BlogText
        {
            get
            {
                return _BlogText;
            }
            set
            {
                _BlogText = value;
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

        [Required]
        public virtual MemberDetails PUser
        {
            get { return _PUser; }
            set { _PUser = value; }
        }

        public string CreatedAgo
        { get { return Version.Ago(); } }

        protected bool Equals(MemberBlog entity)
        {
            if (entity == null) return false;
            if (!base.Equals(entity)) return false;
            if (!Equals(_Id, entity._Id)) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as MemberBlog);
        }

        public override int GetHashCode()
        {
            int result = base.GetHashCode();
            result = 29 * result + _Id.GetHashCode();
            return result;
        }
    }
}
