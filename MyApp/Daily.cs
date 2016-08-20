using System;
using System.Runtime.Serialization;

namespace MyApp
{
    [DataContract]
    public class Daily
    {
        [DataMember(Name = "date")]
        public string date { get; set; }

        [DataMember(Name = "dining")]
        public double dining { get; set; }
        [DataMember(Name = "detail_dining")]
        public string detail_dining { get; set; }

        [DataMember(Name = "sleep")]
        public double sleep { get; set; }
        [DataMember(Name = "detail_sleep")]
        public string detail_sleep { get; set; }

        [DataMember(Name = "toilet")]
        public double toilet { get; set; }
        [DataMember(Name = "detail_toilet")]
        public string detail_toilet { get; set; }

        [DataMember(Name = "parlour")]
        public double parlour { get; set; }
        [DataMember(Name = "detail_parlour")]
        public string detail_parlour { get; set; }

        [DataMember(Name = "outdoor")]
        public double outdoor { get; set; }
        [DataMember(Name = "detail_outdoor")]
        public string detail_outdoor { get; set; }
    }

}
