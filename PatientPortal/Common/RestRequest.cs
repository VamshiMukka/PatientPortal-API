using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientPortal.Common
{
    public class RestRequest
    {
        public string RequestUri { get; set; }
        public string AuthorizationKey { get; set; }
        public string MediaType { get; set; }
        public string PostData { get; set; }
        public string HttpWebRequestMethod { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public int Timeout { get; set; }
        public Dictionary<string, string> HeaderKeyValue { get; set; }
    }
}
