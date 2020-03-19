using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacedonianCroatianEnglishGermanTranslateV3.Helpers
{
    public class GoogleTranslateApiCredentials
    {
        public String type { get; set; }
        public String project_id { get; set; }
        public String private_key_id { get; set; }
        public String private_key { get; set; }
        public String client_email { get; set; }
        public String client_id { get; set; }
        public String auth_uri { get; set; }
        public String token_uri { get; set; }
        public String auth_provider_x509_cert_url { get; set; }
        public String client_x509_cert_url { get; set; }
    }
}
