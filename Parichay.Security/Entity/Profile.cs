using System;
using System.Collections.Generic;
using System.Text;

namespace Parichay.Security.Entity
{
    public class Profile 
    {
        public byte[] PropertyValuesBinary { get; set; }
        public string PropertyNames { get; set; }
        public string PropertyValuesString { get; set; }
        public DateTime LastActivityDate { get; set; }
        public int UserId { get; set; }
        //User User { get; set; }
        //public int Id { get; set; }
        public Profile()
            : base()
        {
        }

    }
}
