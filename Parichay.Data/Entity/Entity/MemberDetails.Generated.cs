using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Iesi.Collections;
using Iesi.Collections.Generic;



using Parichay.Data.Entity;

namespace Parichay.Data.Entity 
{    
	/// <summary>
	/// An object representation of the Member_Details table
	/// </summary>
	//[Serializable]
	public class MemberDetails
	{
		protected System.Int32 _Id;

        private System.String _PEmail;
		private System.String _SEmail;
		private System.String _Surnm;
		private System.String _PostalC;
		private System.String _HphoneN;
		private System.String _Institute;
		private System.String _TelN;
		private System.String _Nicknm;
        private System.String _GenderC;
		private System.String _Addr;
		private System.String _Dept;
		private System.String _CtryC;
		private System.String _Givennm;
		private System.String _TitleC;
        private System.DateTime _Version;


        private Int32 _ShowPrvInfo;

        public virtual Int32 ShowPrvInfo
        {
            get { return _ShowPrvInfo; }
            set { _ShowPrvInfo = value; }
        }


        private Nullable<System.Int32> _PicId;

        private MemberAbout _Biodata;

        public virtual MemberAbout Biodata
        {
            get { return _Biodata; }
            set { _Biodata = value; }
        }

        public Dictionary<string, string> lstGender { get { return new Dictionary<string, string>() { { "M", "Male" }, { "F", "Female" }}; } }
        public Dictionary<string, string> lstTitles { get { return new Dictionary<string, string>() { { "Mr.", "Mr" }, { "Mrs.", "Mrs." }, { "Prof.", "Prof." }, { "Dr.", "Dr." }, { "Ms.", "Ms." }}; } }
        public IList<Country> lstCountries { get; set; }

        public Nullable<System.Int32> PicId
        {
            get { return _PicId; }
            set { _PicId = value; }
        }

        [Display(Name = "Primary Email")]
        [Required]
        public virtual System.String PEmail
        {
            get
            {
                return _PEmail;
            }
            set
            {
                _PEmail = value;
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


		public virtual System.String SEmail
		{
			get
			{
				return _SEmail;
			}
			set
			{
				_SEmail = value;
			}
		}

        [Display(Name = "Sur Name")]
        [Required]
		public virtual System.String Surnm
		{
			get
			{
				return _Surnm;
			}
			set
			{
				_Surnm = value;
			}
		}

		public virtual System.String PostalC
		{
			get
			{
				return _PostalC;
			}
			set
			{
				_PostalC = value;
			}
		}

		public virtual System.String HphoneN
		{
			get
			{
				return _HphoneN;
			}
			set
			{
				_HphoneN = value;
			}
		}

		public virtual System.String Institute
		{
			get
			{
				return _Institute;
			}
			set
			{
				_Institute = value;
			}
		}

		public virtual System.String TelN
		{
			get
			{
				return _TelN;
			}
			set
			{
				_TelN = value;
			}
		}
        [Display(Name = "Nick Name")]
		public virtual System.String Nicknm
		{
			get
			{
				return _Nicknm;
			}
			set
			{
				_Nicknm = value;
			}
		}

        [Required]
        public virtual System.String GenderC
		{
			get
			{
                return _GenderC;
			}
			set
			{
                _GenderC = value;
			}
		}


		public virtual System.String Addr
		{
			get
			{
				return _Addr;
			}
			set
			{
				_Addr = value;
			}
		}

		public virtual System.String Dept
		{
			get
			{
				return _Dept;
			}
			set
			{
				_Dept = value;
			}
		}

        [Required]
		public virtual System.String CtryC
		{
			get
			{
				return _CtryC;
			}
			set
			{
				_CtryC = value;
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

        [Display(Name = "Given Name")]
        [Required]
        [StringLength(60,MinimumLength=3)]
		public virtual System.String Givennm
		{
			get
			{
				return _Givennm;
			}
			set
			{
				_Givennm = value;
			}
		}

        [Display(Name = "Title")]
        [Required]
		public virtual System.String TitleC
		{
			get
			{
				return _TitleC;
			}
			set
			{
				_TitleC = value;
			}
		}

        [Display(Name="Show Contact Info. to Public?")]
        [Required]
        public bool bShowPrvInfo { get { return (_ShowPrvInfo != 0); } set { _ShowPrvInfo = (value) ? 1 : 0; } }

		protected bool Equals(MemberDetails entity)
		{
			if (entity == null) return false;
			if (!base.Equals(entity)) return false;
			if (!Equals(_Id, entity._Id)) return false;
			return true;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			return Equals(obj as MemberDetails);
		}

		public override int GetHashCode()
		{
			int result = base.GetHashCode();
			result = 29*result + _Id.GetHashCode();
			return result;
		}

	}
}
