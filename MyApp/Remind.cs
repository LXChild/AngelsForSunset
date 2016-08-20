using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    [DataContract]
    class Remind
    {
        [DataMember(Name = "time")]
        public String time { get; set; }
        [DataMember(Name = "message")]
        public String message { get; set; }
    }
}
