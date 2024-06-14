using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.Spider.Model
{
    public class SpiderRecord
    {
        public SpiderRecord(string rawSetting)
        {
            FullParam = rawSetting;
            Url = rawSetting.Split(' ').LastOrDefault();
            PureUrl = Url?.Replace("http://", "").Replace("https://", "");
        }
        public DateTime Time { get; set; } = DateTime.MinValue;
        public string FullParam { get; set; }
        public string Url { get; set; }
        public string PureUrl { get; set; }

        public bool IsExpired(int seconds)
        {
            return Time.AddSeconds(seconds) < DateTime.Now;
        }
    }
}
