using System;
using System.Collections.Generic;
using System.ComponentModel;
using Iesi.Collections;
using Iesi.Collections.Generic;
using Parichay.Data.Entity;

namespace Parichay.Data.Entity 
{    
	/// <summary>
	/// An object representation of the lookup_Country table
	/// </summary>
	[Serializable]
	public class Country
	{
		protected System.String _Id;

		private System.String _Name;

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
					throw new BusinessException("Name", "Country Name must not be null.");
				}
				_Name = value;
			}
		}

		public virtual System.String Id
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


		protected bool Equals(Country entity)
		{
			if (entity == null) return false;
			if (!base.Equals(entity)) return false;
			if (!Equals(_Id, entity._Id)) return false;
			return true;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			return Equals(obj as Country);
		}

		public override int GetHashCode()
		{
			int result = base.GetHashCode();
			result = 29*result + _Id.GetHashCode();
			return result;
		}

	}
}
