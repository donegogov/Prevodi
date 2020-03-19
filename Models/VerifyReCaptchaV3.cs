using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MacedonianCroatianEnglishGermanTranslateV3.Models
{
    public class VerifyReCaptchaV3
    {
        [Required]
        public String response { get; set; }
        [Required]
        [RegularExpression("^(homepage|homepage_submit_form)")]
        public String action { get; set; }
    }
}
