using System.Runtime.Serialization;

namespace MyApp
{
    [DataContract]
    class IndoorPosition
    {
        [DataMember(Name = "time")]
        public string time { get; set; }
        [DataMember(Name = "room")]
        public string room { get; set; }
    }
}
