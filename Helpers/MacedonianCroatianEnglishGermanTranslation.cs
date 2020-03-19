using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using MacedonianCroatianEnglishGermanTranslateV3.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacedonianCroatianEnglishGermanTranslateV3.Helpers
{
    public static class MacedonianCroatianEnglishGermanTranslation
    {
        public async static Task<TranslationViewModel> MkCroEnDe(String MkTekst, String projectName, ILogger logger, IOptions<GoogleTranslateApiCredentials> googleTranslateApiCredentials)
        {
            string MkCroV3 = await googleTranslateApiV3(
                MkTekst,
                "hr", projectName, logger, googleTranslateApiCredentials);

            string MkCroEnV3 = await googleTranslateApiV3(MkCroV3, "en", projectName, logger, googleTranslateApiCredentials);

            string MkCroEnDeV3 = await googleTranslateApiV3(MkCroEnV3, "de", projectName, logger, googleTranslateApiCredentials);

            return new TranslationViewModel
            {
                Makedonski = MkTekst,
                Hrvatski = MkCroV3,
                Angliski = MkCroEnV3,
                Germanski = MkCroEnDeV3
            };
        }

        public async static Task<TranslationViewModel> DeEnCroMk(String DeTekst, String projectName, ILogger logger, IOptions<GoogleTranslateApiCredentials> googleTranslateApiCredentials)
        {
            string DeEnV3 = await googleTranslateApiV3(
                DeTekst,
                "en", projectName, logger, googleTranslateApiCredentials);

            string DeEnCroV3 = await googleTranslateApiV3(DeEnV3, "hr", projectName, logger, googleTranslateApiCredentials);

            string DeEnCroMkV3 = await googleTranslateApiV3(DeEnCroV3, "mk", projectName, logger, googleTranslateApiCredentials);

            return new TranslationViewModel
            {
                Makedonski = DeEnCroMkV3,
                Hrvatski = DeEnCroV3,
                Angliski = DeEnV3,
                Germanski = DeTekst
            };
        }

        public async static Task<String> googleTranslateApiV3(String textToTranslate, String targetLanguage, String projectName, ILogger logger, IOptions<GoogleTranslateApiCredentials> googleTranslateApiCredentials)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
            {
                //var credential = GoogleCredential.GetApplicationDefaultAsync();
                //Hostirano lokalno
                TranslationServiceClientBuilder translationServiceClientBuilder = new TranslationServiceClientBuilder();
                translationServiceClientBuilder.JsonCredentials = JsonConvert.SerializeObject(googleTranslateApiCredentials.Value);
                TranslationServiceClient translationServiceClient = await translationServiceClientBuilder.BuildAsync();

                TranslateTextRequest request = new TranslateTextRequest
                {
                    Contents =
                {
                    // The content to translate.
                    textToTranslate,
                },
                    TargetLanguageCode = targetLanguage,
                    ParentAsLocationName = new LocationName(projectName, "global"),
                };
                TranslateTextResponse response = await translationServiceClient.TranslateTextAsync(request);

                return response.Translations[0].TranslatedText;
            }
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production"))
            {
                TranslationServiceClient translationServiceClient = await TranslationServiceClient.CreateAsync();

                TranslateTextRequest request = new TranslateTextRequest
                {
                    Contents =
                {
                    // The content to translate.
                    textToTranslate,
                },
                    TargetLanguageCode = targetLanguage,
                    ParentAsLocationName = new LocationName(projectName, "global"),
                };
                TranslateTextResponse response = await translationServiceClient.TranslateTextAsync(request);

                return response.Translations[0].TranslatedText;
            }

            return null;
            
        }
    }
}
