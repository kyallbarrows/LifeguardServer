using System;
using System.Collections.Generic;
using System.Text;

namespace Parichay.Security.ViewModel
{
   public class Permission
    {
       Entity.roleactions _base;

        #region Properties
        public int Id
        {
            get { return _base.Id; }
            set { _base.Id = value; }
        }
        public string Role
        {
            get { return (_base.Role==null||string.IsNullOrEmpty(_base.Role.Name)) ? string.Empty : _base.Role.Name; }
            set { _base.Role.Name = value; }
        }
        public string Controller
        {
            get { return _base.Action.ControllerName; }
            set { _base.Action.ControllerName = value; }
        }
        public string Action
        {
            get { return _base.Action.ActionName; }
            set { _base.Action.ActionName = value; }
        }
        public string Users
        {
            get { return (_base.User==null||string.IsNullOrEmpty(_base.User.Name))?string.Empty:_base.User.Name; }
            set { _base.User.Name = value; }
        }
        public int ControllerId
        {
            get;
            set;
        }

        public Int32 UserId
        {
            get { return _base.User.Id; }
            set { _base.User.Id = value; }
        }
        public int PermissionType
        {
            get { return _base.PermissionType; }
            set { _base.PermissionType = value; }
        }

        #endregion Properties

        internal Permission(Entity.roleactions inputbase)
        {
            _base = new Entity.roleactions();
            _base.Role = new Entity.Role();
            _base.User = new Entity.User();
            _base.Action = new Entity.actions();
            _base.Id = inputbase.Id;
            _base.Role = inputbase.Role;
            _base.Action = inputbase.Action;
            _base.User = inputbase.User;
            _base.PermissionType = inputbase.PermissionType;
        }
        public Permission()
        {
            _base = new Entity.roleactions();
            _base.Role = new Entity.Role();
            _base.User = new Entity.User();
            _base.Action = new Entity.actions();
        }

        internal Entity.roleactions toRoleAction()
        {
            return _base;
        }
    }
}
