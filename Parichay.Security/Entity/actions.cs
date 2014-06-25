using System;
using System.Collections.Generic;
using System.ComponentModel;
using Iesi.Collections;
//////using Iesi.Collections.Generic;
using Parichay.Security.Entity.Components;
using Parichay.Security.Entity;

namespace Parichay.Security.Entity 
{    
	/// <summary>
	/// An object representation of the actions table
	/// </summary>
	[Serializable]
	public class actions
	{
		protected System.Int32 _Id;

		private readonly ISet _FKroleactionsactions = new HashedSet();
		private System.String _ControllerName;
		private System.String _ActionName;

		public virtual ISet FKroleactionsactions
		{
			get
			{
				return _FKroleactionsactions;
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

		public virtual System.String ControllerName
		{
			get
			{
				return _ControllerName;
			}
			set
			{
				if (value == null)
				{
					throw new BusinessException("ControllerNameRequired", "ControllerName must not be null.");
				}
				_ControllerName = value;
			}
		}

		public virtual System.String ActionName
		{
			get
			{
				return _ActionName;
			}
			set
			{
				if (value == null)
				{
					throw new BusinessException("ActionNameRequired", "ActionName must not be null.");
				}
				_ActionName = value;
			}
		}


	}
}
