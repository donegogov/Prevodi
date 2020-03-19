using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MacedonianCroatianEnglishGermanTranslateV3.Models
{
    public class IndexViewViewModel
    {
        [Required]
        public TranslationViewModel translationViewModel { get; set; }
        [Required]
        public SelectLanguageTranslationListViewModel selectLanguageTranslationListViewModel { get; set; }
        /*[Required]
        [BindProperty(Name = "g-recaptcha-response")]
        public String GoogleReCaptchaResponse { get; set; }*/
        public IndexViewViewModel() { }

        public IndexViewViewModel(TranslationViewModel translationViewModel, SelectLanguageTranslationListViewModel selectLanguageTranslationListViewModel) 
        {
            this.translationViewModel = translationViewModel;
            this.selectLanguageTranslationListViewModel = selectLanguageTranslationListViewModel;
        }
    }
}
