using System;

using System.Text;
using System.ComponentModel.DataAnnotations;
using Iesi.Collections.Generic;

namespace Parichay.Data.Entity
{
    /// <summary>
    /// An object representation of the Member_Groupmessages table
    /// </summary>
    [Serializable]
    public class MemberGroupmessages
    {
        protected System.Int32 _Id;

        private System.String _Imagefilename;
        private System.DateTime _Createdon;
        private MemberGroupmessages _Parent;
        private System.String _Source;
        private System.DateTime _Modifiedon;
        private System.Int32 _Type;
        private System.Int32 _Isprivate;
        private System.String _Text;
        private MemberGroupmessages _Message;
        private MemberGroups _Group;
        private System.String _Thumbnailfilename;
        private MemberDetails _Sender;
        private MemberDetails _Recipient;
        private ISet<MemberGroupmessages> _Children;

        public virtual ISet<MemberGroupmessages> Children
        {
            get { return _Children; }
            set { _Children = value; }
        }
        public virtual System.String Imagefilename
        {
            get
            {
                return _Imagefilename;
            }
            set
            {
                _Imagefilename = value;
            }
        }

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

        public virtual MemberGroupmessages Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;
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

        public virtual System.Int32 Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }

        public virtual System.Int32 Isprivate
        {
            get
            {
                return _Isprivate;
            }
            set
            {
                _Isprivate = value;
            }
        }

        [Required]
        public virtual System.String Text
        {
            get
            {
                return _Text;
            }
            set
            {
                if (value == null)
                {
                    throw new BusinessException("TextRequired", "Text must not be null.");
                }
                _Text = value;
            }
        }

        public virtual MemberGroupmessages Message
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

        public virtual System.String Thumbnailfilename
        {
            get
            {
                return _Thumbnailfilename;
            }
            set
            {
                _Thumbnailfilename = value;
            }
        }

        [Required]
        public virtual MemberDetails Sender
        {
            get
            {
                return _Sender;
            }
            set
            {
                _Sender = value;
            }
        }
        public virtual MemberDetails Recipient
        {
            get
            {
                return _Recipient;
            }
            set
            {
                _Recipient = value;
            }
        }

        public virtual System.String Source
        {
            get
            {
                return _Source;
            }
            set
            {
                if (value == null)
                {
                    throw new BusinessException("SourceRequired", "Source must not be null.");
                }
                _Source = value;
            }
        }

        public bool bIsprivate
        { get { return (_Isprivate != 0); } set { _Isprivate = (value) ? 1 : 0; } }

        public string CreatedAgo
        { get { return (_Createdon.Ago()); } }
    }
}
