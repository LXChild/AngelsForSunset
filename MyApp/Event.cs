using System.Runtime.Serialization;

namespace MyApp
{
    [DataContract]
    class Event
    {
        [DataMember(Name = "time")]
        public string time { get; set; }
        [DataMember(Name = "title")]
        public string title { get; set; }
        [DataMember(Name = "content")]
        public string content { get; set; }
    }
}
