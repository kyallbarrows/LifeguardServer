using System;
using System.Collections.Generic;
using System.ComponentModel;
using Iesi.Collections;
//using Iesi.Collections.Generic;
using Parichay.Security.Entity.Components;


using Parichay.Security.Entity;

namespace Parichay.Security.Entity 
{    
	/// <summary>
	/// An object representation of the Log table
	/// </summary>
	[Serializable]
	public class Log
	{
		protected System.Int32 _Id;

		private System.DateTime _Date;
		private System.String _Thread;
		private System.String _Level;
		private System.String _Logger;
		private System.String _Message;
		private System.String _Exception;

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

		public virtual System.DateTime Date
		{
			get
			{
				return _Date;
			}
			set
			{
				_Date = value;
			}
		}

		public virtual System.String Thread
		{
			get
			{
				return _Thread;
			}
			set
			{
				if (value == null)
				{
					throw new BusinessException("ThreadRequired", "Thread must not be null.");
				}
				_Thread = value;
			}
		}

		public virtual System.String Level
		{
			get
			{
				return _Level;
			}
			set
			{
				if (value == null)
				{
					throw new BusinessException("LevelRequired", "Level must not be null.");
				}
				_Level = value;
			}
		}

		public virtual System.String Logger
		{
			get
			{
				return _Logger;
			}
			set
			{
				if (value == null)
				{
					throw new BusinessException("LoggerRequired", "Logger must not be null.");
				}
				_Logger = value;
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
				if (value == null)
				{
					throw new BusinessException("MessageRequired", "Message must not be null.");
				}
				_Message = value;
			}
		}

		public virtual System.String Exception
		{
			get
			{
				return _Exception;
			}
			set
			{
				if (value == null)
				{
					throw new BusinessException("ExceptionRequired", "Exception must not be null.");
				}
				_Exception = value;
			}
		}


		protected bool Equals(Log entity)
		{
			if (entity == null) return false;
			if (!base.Equals(entity)) return false;
			if (!Equals(_Id, entity._Id)) return false;
			return true;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			return Equals(obj as Log);
		}

		public override int GetHashCode()
		{
			int result = base.GetHashCode();
			result = 29*result + _Id.GetHashCode();
			return result;
		}

	}
}
