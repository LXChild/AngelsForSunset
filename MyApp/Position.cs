using System.Runtime.Serialization;

namespace MyApp
{
    [DataContract]
    public class Position
    {
        [DataMember(Name = "time")]
        public string time { get; set; }
        [DataMember(Name = "longitude")]
        public double longitude { get; set; }
        [DataMember(Name = "latitude")]
        public double latitude { get; set; }
    }
}
