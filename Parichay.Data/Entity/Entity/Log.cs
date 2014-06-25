using System;
using System.Collections.Generic;
using System.ComponentModel;
using Iesi.Collections;
using Iesi.Collections.Generic;



using Parichay.Data.Entity;

namespace Parichay.Data.Entity
{
    /// <summary>
    /// An object representation of the system_log table
    /// </summary>
    [Serializable]
    public class Log
    {
        protected System.Int32 _Id;
        private DateTime _Date;

        private string _Level;
        private string _Logger;
        private string _Message;
        private string _Thread;

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

        public virtual DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }

        
        public virtual string Logger
        {
            get { return _Logger; }
            set
            {
                if (value == null)
                {
                    throw new BusinessException("LoggerRequired", "Logger must not be null.");
                } _Logger = value;
            }
        }


        public virtual string Level
        {
            get { return _Level; }
            set
            {
                if (value == null)
                {
                    throw new BusinessException("LevelRequired", "Level must not be null.");
                } _Level = value;
            }
        }



        public virtual string Message
        {
            get { return _Message; }
            set
            {
                if (value == null)
                {
                    throw new BusinessException("MessageRequired", "Message must not be null.");
                } _Message = value;
            }
        }



        public virtual string Thread
        {
            get { return _Thread; }
            set
            {
                if (value == null)
                {
                    throw new BusinessException("ThreadRequired", "Thread must not be null.");
                } _Thread = value;
            }
        }


        

    }
}
