using MacedonianCroatianEnglishGermanTranslateV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacedonianCroatianEnglishGermanTranslateV3.Extensions
{
    public class TranslationViewModelEqualityComparer : IEqualityComparer<TranslationViewModel>
    {
        public bool Equals(TranslationViewModel translationViewModel1, TranslationViewModel translationViewModel2)
        {
            if (translationViewModel1.TekstZaPreveduvanje == translationViewModel2.TekstZaPreveduvanje)
                return true;

            return false;
        }

        public int GetHashCode(TranslationViewModel obj)
        {
            return obj.TekstZaPreveduvanje.GetHashCode();
        }
    }
}
