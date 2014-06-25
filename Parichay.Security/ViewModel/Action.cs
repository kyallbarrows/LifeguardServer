using System;
using System.Collections;

using System.Text;
using Iesi.Collections;
//using Iesi.Collections.Generic;

namespace Parichay.Security.ViewModel
{
   public class Activities
    {
       private Entity.actions _base;

        #region Properties
        public int Id
        {
            get { return _base.Id; }
            set { _base.Id = value; }
        }
        public string ControllerName
        {
            get { return _base.ControllerName; }
            set
            {
                _base.ControllerName = value;
            }
        }
        public string ActionName
        {
            get { return _base.ActionName; }
            set
            {
                _base.ActionName = value;
            }
        }

        public string FullName
        {
            get
            {
                return _base.ControllerName + " - " + _base.ActionName ;
                
            }
        }



        public ISet Roles
        {
            get { return _base.FKroleactionsactions; }
            
        }
        #endregion Properties

        internal Activities(Entity.actions inputbase)
        {
            _base = new Entity.actions();
            _base.Id = inputbase.Id;
            _base.ControllerName = inputbase.ControllerName;
            _base.ActionName = inputbase.ActionName;
           // _base.FKroleactionsactions = inputbase.FKroleactionsactions;
        }
    }
}
