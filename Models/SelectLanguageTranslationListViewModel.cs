using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MacedonianCroatianEnglishGermanTranslateV3.Models
{
    public class SelectLanguageTranslationListViewModel
    {
        //public List<SelectListItem> IzberiPrevod { get; set; }
        [Required]
        [RegularExpression("^(MkDe|DeMk)")]
        public string SelectedPrevod { get; set; }
        public SelectLanguageTranslationListViewModel()
        {
            SelectedPrevod = "Германско Македонски";
        }

        public IEnumerable<SelectListItem> GetVidoviPrevodi()
        {
            yield return new SelectListItem { Text = "Германско Македонски", Value = "DeMk" };
            yield return new SelectListItem { Text = "Македонско Германски", Value = "MkDe" };
        }
    }
}
