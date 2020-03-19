using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacedonianCroatianEnglishGermanTranslateV3.Models
{
    public class ReCaptchaV3Response
    {
        public bool IsHttpResponseOk { get; set; }
        public JObject reCaptchaVerifyResponse { get; set; }
    }
}
