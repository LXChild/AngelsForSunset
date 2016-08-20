using System.Runtime.Serialization;

namespace MyApp
{
    [DataContract]
    class Heartbeat
    {
        [DataMember(Name = "time")]
        public string time { get; set; }
        [DataMember(Name = "beat")]
        public int beat { get; set; }
    }
}
