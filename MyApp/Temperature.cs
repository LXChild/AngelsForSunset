using System.Runtime.Serialization;

namespace MyApp
{
    [DataContract]
    class Temperature
    {
        [DataMember(Name = "time")]
        public string time { get; set; }
        [DataMember(Name = "temperature")]
        public double temperature { get; set; }
    }
}
