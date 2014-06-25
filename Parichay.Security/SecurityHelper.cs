using System;
using System.Collections.Generic;
using System.Text;
using Parichay.Security.Entity;
using NHibernate;

namespace Parichay.Security
{
   public static class SecurityHelper
    {

       public static IList<ViewModel.DbEntity> getAllRoles()
       {
           IList<ViewModel.DbEntity> resultList = new List<ViewModel.DbEntity>();

           try
           {
               IList<Role> actList = Helper.NHibernateProviderEntityHelper.ConvertToListOf<Role>(Parichay.Security.Helper.NHibernateHelper.FindByNamedQuery("Role.FindAll"));

               foreach (Role item in actList)
               {
                   ViewModel.DbEntity tmp = new ViewModel.DbEntity();
                   tmp.Id = item.Id;
                   tmp.Name = item.Name;
                   tmp.Description = item.Description;
                   tmp.LoweredName = item.LoweredName;

                   resultList.Add(tmp);
               }
           }
           catch
           {
               throw;
           }
           return (resultList);
       }

    }
}
