using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MacedonianCroatianEnglishGermanTranslateV3.Models
{
    public class TranslationViewModel
    {
        [Required]
        //[RegularExpression("([a-zA-Z0-9абвгдѓежзѕијклљмнњопрстќуфхцчџшАБВГДЃЕЖЗЅИЈКЛЉМНЊОПРСТЌУФХЦЧЏШ]+)", ErrorMessage = "Enter only alphabets and numbers")]
        public string TekstZaPreveduvanje { get; set; }
        public string Makedonski { get; set; }
        public string Hrvatski { get; set; }
        public string Angliski { get; set; }
        public string Germanski { get; set; }
    }
}
