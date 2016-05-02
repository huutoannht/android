using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfOrigin
{
    [DataContract]
    public class User
    {
        public User()
        {

        }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string UserPassword { get; set; }
    }
}