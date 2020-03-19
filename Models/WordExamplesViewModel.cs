using MacedonianCroatianEnglishGermanTranslateV3.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MacedonianCroatianEnglishGermanTranslateV3.Models
{
    public class WordExamplesViewModel
    {
        [Required]
        public String Word { get; set; }
        public List<TranslationViewModel> Senses { get; set; }
        public PaginatedList<TranslationViewModel> TranslaatedTextToReturn { get; set; }
        [Required]
        public SelectLanguageTranslationListViewModel selectLanguageTranslationListViewModel { get; set; }

        public WordExamplesViewModel()
        {
            selectLanguageTranslationListViewModel = new SelectLanguageTranslationListViewModel();
        }
    }
}
