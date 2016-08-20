using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyApp
{
    [DataContract]
    class DailyDetail
    {
        //次数
        [DataMember(Name = "time")]
        public int time { get; set; }
        //时间段
        [DataMember(Name = "detail")]
        public List<string> detail { get; set; }
        //评论
        [DataMember(Name = "comment")]
        public string comment { get; set; }
    }
}
