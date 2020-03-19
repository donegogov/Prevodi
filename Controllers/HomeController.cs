using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MacedonianCroatianEnglishGermanTranslateV3.Models;
using MacedonianCroatianEnglishGermanTranslateV3.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Google.Cloud.Diagnostics.Common;
using Google.Cloud.Diagnostics.AspNetCore;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.AspNetCore.Http;
using MacedonianCroatianEnglishGermanTranslateV3.Extensions;

namespace MacedonianCroatianEnglishGermanTranslateV3.Controllers
{
    public class HomeController : Controller
    {
        private const string _SessionSenses = "_Senses";
        private const string _SessionTranslatedSenses = "_TranslatedSenses";
        private const string _SessionWord = "_Word";
        private const string _SessionTranslatedSensesSumary = "_SessionTranslatedSensesSumary";

        private readonly IOptions<GoogleCloudPlatformProjectName> _googleCloudPlatformProjectName;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        private readonly IOptions<GoogleTranslateApiCredentials> _googleTranslateApiCredentials;
        //private readonly IOptions<GoogleReCaptcha> _googleReCaptcha;
        private readonly IOptions<GoogleReCaptchaV3> _googleReCaptchaV3;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly JavaScriptEncoder _javaScriptEncoder;
        private readonly UrlEncoder _urlEncoder;
        private readonly IOptions<OxfordDictionariesApi> _oxfordDictionariesApi;

        public HomeController(IOptions<GoogleCloudPlatformProjectName> googleCloudPlatformProjectName,
            ILoggerFactory loggerFactory,
            IOptions<GoogleTranslateApiCredentials> googleTranslateApiCredentials,
            //IOptions<GoogleReCaptcha> googleReCaptcha,
            IOptions<GoogleReCaptchaV3> googleReCaptchaV3,
            IOptions<OxfordDictionariesApi> oxfordDictionariesApi,
            HtmlEncoder htmlEncoder,
            JavaScriptEncoder javascriptEncoder,
            UrlEncoder urlEncoder)
        {
            _googleCloudPlatformProjectName = googleCloudPlatformProjectName;
            _loggerFactory = loggerFactory;
            _googleTranslateApiCredentials = googleTranslateApiCredentials;
            //_googleReCaptcha = googleReCaptcha;
            _googleReCaptchaV3 = googleReCaptchaV3;
            _htmlEncoder = htmlEncoder;
            _javaScriptEncoder = javascriptEncoder;
            _urlEncoder = urlEncoder;
            _oxfordDictionariesApi = oxfordDictionariesApi;

            if (loggerFactory != null)
                _logger = loggerFactory.CreateLogger("home_controller");
            else
            {
                using (var loggerFactoryTemp = new LoggerFactory())
                {
                    _logger = loggerFactoryTemp.CreateLogger("home_controller");
                }
            }
            // Write the log entry.
            _logger.LogInformation("Constructor Home Controller");

        }

        public IActionResult Index()
        {
            // Write the log entry.
            //_logger.LogInformation("Get Index Method");

            IndexViewViewModel indexViewViewModel = new IndexViewViewModel(new TranslationViewModel(), new SelectLanguageTranslationListViewModel());

            return View("Index", indexViewViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexViewViewModel indexViewViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(new IndexViewViewModel(new TranslationViewModel(), new SelectLanguageTranslationListViewModel()));
            }
            //_logger.LogInformation("_googleReCaptcha.Value.SecretKey");
            //_logger.LogInformation(_googleReCaptcha.Value.SecretKey);
            //_logger.LogInformation("_googleReCaptchaV3.Value.v3_secret");_
            //_logger.LogInformation(_googleReCaptchaV3.Value.v3_secret);

            /*

            var data = await verifyReCaptcha("v2", indexViewViewModel.GoogleReCaptchaResponse);
            //_logger.LogInformation("data.IsHttpResponseOk" + data.IsHttpResponseOk.ToString());
            //_logger.LogInformation("indexViewViewModel.GoogleReCaptchaResponse" + indexViewViewModel.GoogleReCaptchaResponse);
            //_logger.LogInformation("data.reCaptchaVerifyResponse[\"success\"].ToString().ToLower()" + data.reCaptchaVerifyResponse["success"].ToString().ToLower());
            //_logger.LogInformation("data.reCaptchaVerifyResponse[\"hostname\"].ToString()" + data.reCaptchaVerifyResponse["hostname"].ToString());

            if (data.IsHttpResponseOk && data.reCaptchaVerifyResponse["success"].ToString().ToLower().Equals("true") && (data.reCaptchaVerifyResponse["hostname"].ToString().Equals("clean-yew-270306.appspot.com")))
            //|| data.reCaptchaVerifyResponse["hostname"].ToString().Equals("localhost")))
            {*/
            //_logger.LogInformation("Verified ReCaptcha");
            // Write the log entry.
            //_logger.LogInformation("Post Index Method");
            // Add a handler to trace outgoing requests and to propagate the trace header.
            //indexViewViewModel.translationViewModel.TekstZaPreveduvanje = HttpUtility.HtmlEncode(indexViewViewModel.translationViewModel.TekstZaPreveduvanje);

            if (indexViewViewModel.selectLanguageTranslationListViewModel.SelectedPrevod.Equals("MkDe") && !String.IsNullOrEmpty(indexViewViewModel.translationViewModel.TekstZaPreveduvanje))
            {
                //_logger.LogInformation("if MkDe");
                //_logger.LogInformation("if MkDe : indexViewViewModel.translationViewModel.TekstZaPreveduvanje = " + indexViewViewModel.translationViewModel.TekstZaPreveduvanje);

                indexViewViewModel.translationViewModel.Makedonski = indexViewViewModel.translationViewModel.TekstZaPreveduvanje;

                TranslationViewModel result = await MacedonianCroatianEnglishGermanTranslation.MkCroEnDe(indexViewViewModel.translationViewModel.Makedonski, _googleCloudPlatformProjectName.Value.ProjectName, _logger, _googleTranslateApiCredentials);
                indexViewViewModel.translationViewModel = result;

                //indexViewViewModel.translationViewModel.Makedonski = HttpUtility.HtmlDecode(indexViewViewModel.translationViewModel.Makedonski);

                return View("Index", indexViewViewModel);
            }
            else if (indexViewViewModel.selectLanguageTranslationListViewModel.SelectedPrevod.Equals("DeMk") && !String.IsNullOrEmpty(indexViewViewModel.translationViewModel.TekstZaPreveduvanje))
            {
                //_logger.LogInformation("else if DeMk");
                //_logger.LogInformation("else if DeMk : indexViewViewModel.translationViewModel.TekstZaPreveduvanje = " + indexViewViewModel.translationViewModel.TekstZaPreveduvanje);

                indexViewViewModel.translationViewModel.Germanski = indexViewViewModel.translationViewModel.TekstZaPreveduvanje;
                TranslationViewModel result = await MacedonianCroatianEnglishGermanTranslation.DeEnCroMk(indexViewViewModel.translationViewModel.Germanski, _googleCloudPlatformProjectName.Value.ProjectName, _logger, _googleTranslateApiCredentials);
                indexViewViewModel.translationViewModel = result;

                //indexViewViewModel.translationViewModel.Germanski = HttpUtility.HtmlDecode(indexViewViewModel.translationViewModel.Germanski);

                return View("Index", indexViewViewModel);
            }
            /*}
            else
            {
                ViewData["error"] = "Invalid ReCaptcha";
            }*/
            return View(new IndexViewViewModel(new TranslationViewModel(), new SelectLanguageTranslationListViewModel()));
        }

        [HttpPost]
        public async Task<bool> VerifyReCaptchaV3(VerifyReCaptchaV3 verifyReCaptchaV3)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }
            var data = await verifyReCaptcha("v3", verifyReCaptchaV3.response);

            if (data != null && data.IsHttpResponseOk && data.reCaptchaVerifyResponse["success"].ToString().ToLower().Equals("true") &&
                data.reCaptchaVerifyResponse["action"].ToString().Equals(HttpUtility.UrlEncode(verifyReCaptchaV3.action))
                && data.reCaptchaVerifyResponse["hostname"].ToString().Equals("clean-yew-270306.appspot.com"))
            {
                if ((float)data.reCaptchaVerifyResponse["score"] < 0.5)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<JObject> getRequestJson(HttpResponseMessage httpResponse)
        {
            var json = (dynamic)null;
            using (StreamReader sr = new StreamReader(await httpResponse.Content.ReadAsStreamAsync()))
            {
                json = await sr.ReadToEndAsync();
            }
            JObject data = JsonConvert.DeserializeObject(json);

            return data;
        }

        private async Task<ReCaptchaV3Response> verifyReCaptcha(String version, String googleReCaptchaResponse)
        {
            //_logger.LogInformation("googleReCaptchaResponse");
            //_logger.LogInformation(googleReCaptchaResponse);

            if (String.IsNullOrEmpty(HttpUtility.UrlEncode(version)) || String.IsNullOrEmpty(googleReCaptchaResponse))
            {
                return null;
            }

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify");

            Dictionary<string, string> queryString = new Dictionary<string, string>();
            /*if (HttpUtility.UrlEncode(version).Equals("v2"))
            {
                queryString = new Dictionary<string, string>()
                {
                    { "secret", _googleReCaptcha.Value.SecretKey },
                    { "response", googleReCaptchaResponse }
                };
            }*/
            //else 
            if (HttpUtility.UrlEncode(version).Equals("v3"))
            {
                queryString = new Dictionary<string, string>()
                {
                    { "secret", _googleReCaptchaV3.Value.v3_secret},
                    { "response", googleReCaptchaResponse }
                };
            }
            //_logger.LogInformation(_googleReCaptcha.Value.SecretKey);
            //_logger.LogInformation(_googleReCaptchaV3.Value.v3_secret);

            var requestQueryString = QueryHelpers.AddQueryString("", queryString);
            var request = new HttpRequestMessage(HttpMethod.Post, requestQueryString);
            var httpResponse = await httpClient.SendAsync(request);

            ReCaptchaV3Response reCaptchaV3Response = new ReCaptchaV3Response();

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                reCaptchaV3Response.IsHttpResponseOk = true;
            }

            reCaptchaV3Response.reCaptchaVerifyResponse = await getRequestJson(httpResponse);

            return reCaptchaV3Response;
        }

        [HttpGet]
        public async Task<IActionResult> WordExamples()
        {
            WordExamplesViewModel wordExamplesViewModel = new WordExamplesViewModel();
            wordExamplesViewModel.Senses = new List<TranslationViewModel>();
            wordExamplesViewModel.TranslaatedTextToReturn = PaginatedList<TranslationViewModel>.Create(
                wordExamplesViewModel.Senses,
                0, 1);

            return View(wordExamplesViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WordExamples(WordExamplesViewModel wordExamplesViewModel,
            int? pageNumber)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            int pageSize = 5;
            String Word = wordExamplesViewModel.Word;
            var SessionWord = HttpContext.Session.GetString(_SessionWord);

            if (!String.IsNullOrEmpty(SessionWord))
            {
                if (SessionWord.Equals(wordExamplesViewModel.Word))
                {
                    //Вчитај примери текст поле за вчитување преводи
                    //Ako requestot e od istiot korisni i veke e prevedeno zborceto
                    var SessionSenses = HttpContext.Session.GetObjectFromJson<List<TranslationViewModel>>(_SessionSenses);
                    var SessionTranslatedSenses = HttpContext.Session.GetObjectFromJson<List<TranslationViewModel>>(_SessionTranslatedSenses);
                    var SessionTranslatedSensesSumary = HttpContext.Session.GetObjectFromJson<List<TranslationViewModel>>(_SessionTranslatedSensesSumary);
                    PaginatedList<TranslationViewModel> sessionPaginatedList = PaginatedList<TranslationViewModel>.Create(
                        SessionTranslatedSensesSumary,
                        pageNumber ?? 1, pageSize);
                    //if (pageNumber != null && wordExamplesViewModel.Senses != null && wordExamplesViewModel.Senses.Count() > 0)
                    //if(wordExamplesViewModel.PageIndex == 0)
                    if (SessionSenses != null && 
                        SessionSenses.Count() > 0 &&
                        sessionPaginatedList.Any(m => m.TekstZaPreveduvanje == wordExamplesViewModel.Word && m.Hrvatski == null && m.Angliski == null))
                    {
                        wordExamplesViewModel.TranslaatedTextToReturn = PaginatedList<TranslationViewModel>.Create(
                           SessionSenses,
                           pageNumber ?? 1, 
                           pageSize);
                        List<TranslationViewModel> Result = new List<TranslationViewModel>();
                        TranslationViewModel result = new TranslationViewModel();
                        foreach (var tekst in wordExamplesViewModel.TranslaatedTextToReturn)
                        {
                            if (wordExamplesViewModel.selectLanguageTranslationListViewModel.SelectedPrevod.Equals("MkDe") && tekst.Hrvatski == null && tekst.Angliski == null)
                            {
                                result = await MacedonianCroatianEnglishGermanTranslation.MkCroEnDe(tekst.Makedonski, _googleCloudPlatformProjectName.Value.ProjectName, _logger, _googleTranslateApiCredentials);
                                result.TekstZaPreveduvanje = Word;
                                SessionTranslatedSenses.Add(tekst);
                                Result.Add(result);
                            }
                            else if (wordExamplesViewModel.selectLanguageTranslationListViewModel.SelectedPrevod.Equals("DeMk") && tekst.Hrvatski == null && tekst.Angliski == null)
                            {
                                result = await MacedonianCroatianEnglishGermanTranslation.DeEnCroMk(tekst.Germanski, _googleCloudPlatformProjectName.Value.ProjectName, _logger, _googleTranslateApiCredentials);
                                result.TekstZaPreveduvanje = Word;
                                SessionTranslatedSenses.Add(result);
                                Result.Add(result);
                            }
                        }
                        //wordExamplesViewModel.TranslaatedTextToReturn.Add(result);
                        for(int i = 0; i < Result.Count; i++)
                        {
                            for(int j = 0; j < SessionSenses.Count; j++)
                            {
                                if(SessionSenses[j].Germanski == Result[i].Germanski)
                                {
                                    SessionTranslatedSensesSumary[j].TekstZaPreveduvanje = Result[i].TekstZaPreveduvanje;
                                    SessionTranslatedSensesSumary[j].Makedonski = Result[i].Makedonski;
                                    SessionTranslatedSensesSumary[j].Hrvatski = Result[i].Hrvatski;
                                    SessionTranslatedSensesSumary[j].Angliski = Result[i].Angliski;
                                    SessionTranslatedSensesSumary[j].Germanski = Result[i].Germanski;

                                    SessionSenses[j].TekstZaPreveduvanje = Result[i].TekstZaPreveduvanje;
                                    SessionSenses[j].Makedonski = Result[i].Makedonski;
                                    SessionSenses[j].Hrvatski = Result[i].Hrvatski;
                                    SessionSenses[j].Angliski = Result[i].Angliski;
                                    SessionSenses[j].Germanski = Result[i].Germanski;
                                }
                            }
                        }

                        wordExamplesViewModel.TranslaatedTextToReturn = PaginatedList<TranslationViewModel>.Create(
                           SessionTranslatedSensesSumary,
                           pageNumber ?? 1,
                           pageSize);
                        //WordExamplesPartialViewResult(wordExamplesViewModel.TranslaatedTextToReturn);
                        //return wordExamplesViewModel;
                        //HttpContext.Session.GetObjectFromJson<PaginatedList<TranslationViewModel>>(_SessionTranslatedSenses).Union(wordExamplesViewModel.TranslaatedTextToReturn);

                        //PaginatedList<TranslationViewModel> tempPaginatedListTranslatedViewModel = HttpContext.Session.GetObjectFromJson<PaginatedList<TranslationViewModel>>(_SessionTranslatedSenses);
                        //tempPaginatedListTranslatedViewModel.AddRange(wordExamplesViewModel.TranslaatedTextToReturn);
                        //tempPaginatedListTranslatedViewModel.Insert(pageNumber ?? 1, result);
                        /*for (int i = 0; i < wordExamplesViewModel.TranslaatedTextToReturn.Count; i++)
                        {
                            for (int j = 0; j < tempPaginatedListTranslatedViewModel.Count; j++)
                            {
                                if (wordExamplesViewModel.TranslaatedTextToReturn[i].Germanski == tempPaginatedListTranslatedViewModel[j].Germanski)
                                {
                                    tempPaginatedListTranslatedViewModel[j].Makedonski = wordExamplesViewModel.TranslaatedTextToReturn[i].Makedonski;
                                    tempPaginatedListTranslatedViewModel[j].Hrvatski = wordExamplesViewModel.TranslaatedTextToReturn[i].Hrvatski;
                                    tempPaginatedListTranslatedViewModel[j].Angliski = wordExamplesViewModel.TranslaatedTextToReturn[i].Angliski;
                                    tempPaginatedListTranslatedViewModel[j].Germanski = wordExamplesViewModel.TranslaatedTextToReturn[i].Germanski;
                                }
                            }
                        }*/
                        HttpContext.Session.SetObjectAsJson(_SessionTranslatedSenses, SessionTranslatedSenses);
                        HttpContext.Session.SetObjectAsJson(_SessionTranslatedSensesSumary, SessionTranslatedSensesSumary);

                        return PartialView("_sensesPartialView", wordExamplesViewModel);
                    }
                    else
                    {
                        /*for (int i = 0; i < SessionTranslatedSenses.Count; i++)
                        {
                            for (int j = 0; j < SessionSenses.Count; j++)
                            {
                                if (SessionSenses[j].Germanski == SessionTranslatedSenses[i].Germanski)
                                {
                                    SessionSenses[j].TekstZaPreveduvanje = SessionTranslatedSenses[j].TekstZaPreveduvanje;
                                    SessionSenses[j].Makedonski = SessionTranslatedSenses[j].Makedonski;
                                    SessionSenses[j].Hrvatski = SessionTranslatedSenses[j].Hrvatski;
                                    SessionSenses[j].Angliski = SessionTranslatedSenses[j].Angliski;
                                    SessionSenses[j].Germanski = SessionTranslatedSenses[j].Germanski;
                                }
                            }
                        }*/

                        wordExamplesViewModel.TranslaatedTextToReturn = PaginatedList<TranslationViewModel>.Create(
                            SessionTranslatedSensesSumary,
                            pageNumber ?? 1,
                            pageSize);

                        return PartialView("_sensesPartialView", wordExamplesViewModel);
                    }
                }
            }

            if (String.IsNullOrEmpty(wordExamplesViewModel.Word))
            {
                return null;
            }
            String UserWordToTranslate = wordExamplesViewModel.Word;
            if (wordExamplesViewModel.selectLanguageTranslationListViewModel.SelectedPrevod.Equals("MkDe"))
            {
                TranslationViewModel result = await MacedonianCroatianEnglishGermanTranslation.MkCroEnDe(wordExamplesViewModel.Word, _googleCloudPlatformProjectName.Value.ProjectName, _logger, _googleTranslateApiCredentials);
                wordExamplesViewModel.Word = result.Germanski;
            }

            wordExamplesViewModel.Word = await lemmas(wordExamplesViewModel.Word.ToLower());


            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://od-api.oxforddictionaries.com/api/v2/translations/de/en/" + HttpUtility.UrlEncode(wordExamplesViewModel.Word));

            Dictionary<string, string> queryString = new Dictionary<string, string>();

            queryString = new Dictionary<string, string>()
                {
                    { "strictMatch", "true"},
                    { "fields", "examples" }
                };

            var requestQueryString = QueryHelpers.AddQueryString("", queryString);
            var request = new HttpRequestMessage(HttpMethod.Get, requestQueryString);

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("app_id", _oxfordDictionariesApi.Value.app_id);
            request.Headers.Add("app_key", _oxfordDictionariesApi.Value.app_key);

            var httpResponse = await httpClient.SendAsync(request);

            WordExamplesViewModel wordExamplesViewModelToReturn = wordExamplesViewModel;
            wordExamplesViewModelToReturn.Senses = new List<TranslationViewModel>();         

            JObject data = new JObject();

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                data = await getRequestJson(httpResponse);
                /*data = (JObject)data["results"];
                data = (JObject)data["lexicalEntries"];
                data = (JObject)data["entries"];*/
            }
            else if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            //return new JsonResult(data["results"].Children()["lexicalEntries"].Children()["entries"].Children());

            foreach (var results in data["results"])
            {
                foreach (var lexicalEntries in results.Value<JToken>("lexicalEntries"))
                {
                    foreach (var entries in lexicalEntries.Value<JToken>("entries"))
                    {
                        foreach (var senses in entries.Value<JToken>("senses"))
                        {
                            foreach (var text in senses.Value<JToken>("examples"))
                            {
                                /*TranslationViewModel translatedText = await MacedonianCroatianEnglishGermanTranslation.DeEnCroMk(text.Value<String>("text"), _googleCloudPlatformProjectName.Value.ProjectName, _logger, _googleTranslateApiCredentials);
                                translatedText.Makedonski = translatedText.Makedonski.Replace("/", "\\");
                                translatedText.Hrvatski = translatedText.Hrvatski.Replace("/", "\\");
                                translatedText.Angliski = translatedText.Angliski.Replace("/", "\\");
                                translatedText.Germanski = translatedText.Germanski.Replace("/", "\\");*/
                                TranslationViewModel translationViewModel = new TranslationViewModel();
                                translationViewModel.Germanski = text.Value<String>("text");
                                translationViewModel.TekstZaPreveduvanje = Word;
                                wordExamplesViewModelToReturn.Senses.Add(translationViewModel);
                            }
                        }
                    }
                }
            }
            //return "Hpw are you doing";
            /*return new PartialViewResult
            {
                ViewName = "_partialAjaxResponseDiv",
                ViewData = this.ViewData
            };*/
            PaginatedList<TranslationViewModel> tempPaginatedList = PaginatedList<TranslationViewModel>.Create(
                wordExamplesViewModelToReturn.Senses,
                pageNumber ?? 1, pageSize);
            List<TranslationViewModel> tempTranslationViewModel = new List<TranslationViewModel>();
            //prv pat zbor
                foreach(var tekst in tempPaginatedList)
                {
                    TranslationViewModel result = await MacedonianCroatianEnglishGermanTranslation.DeEnCroMk(tekst.Germanski, _googleCloudPlatformProjectName.Value.ProjectName, _logger, _googleTranslateApiCredentials);
                    result.TekstZaPreveduvanje = Word;
                    tempTranslationViewModel.Add(result);
                }

            for (int i = 0; i < tempPaginatedList.Count; i++)
            {
                for (int j = 0; j < tempTranslationViewModel.Count; j++)
                {
                    if (tempPaginatedList[i].Germanski == tempTranslationViewModel[j].Germanski)
                    {
                        tempPaginatedList[j].Makedonski = tempTranslationViewModel[i].Makedonski;
                        tempPaginatedList[j].Hrvatski = tempTranslationViewModel[i].Hrvatski;
                        tempPaginatedList[j].Angliski = tempTranslationViewModel[i].Angliski;
                        tempPaginatedList[j].Germanski = tempTranslationViewModel[i].Germanski;
                    }
                }
            }
            /*wordExamplesViewModelToReturn.TranslaatedTextToReturn = PaginatedList<TranslationViewModel>.Create(
                wordExamplesViewModelToReturn.Senses.Union(tempTranslationViewModel, new TranslationViewModelEqualityComparer()).ToList(),
                pageNumber ?? 1, pageSize);*/

            //wordExamplesViewModelToReturn.TranslaatedTextToReturn.AddRange(tempTranslationViewModel);
            wordExamplesViewModelToReturn.TranslaatedTextToReturn = tempPaginatedList;

            /*wordExamplesViewModelToReturn.TranslaatedTextToReturn = PaginatedList<TranslationViewModel>.Create(
                tempPaginatedList,
                pageNumber ?? 1, pageSize);*/

            //wordExamplesViewModelToReturn.TranslaatedTextToReturn = tempPaginatedList;

            List<TranslationViewModel> SessinTranslatedSensesSumary = wordExamplesViewModelToReturn.Senses;

            for (int i = 0; i < SessinTranslatedSensesSumary.Count; i++)
            {
                for (int j = 0; j < wordExamplesViewModelToReturn.TranslaatedTextToReturn.Count; j++)
                {
                    if (wordExamplesViewModelToReturn.TranslaatedTextToReturn[j].Germanski == SessinTranslatedSensesSumary[i].Germanski &&
                        wordExamplesViewModelToReturn.TranslaatedTextToReturn[j].Hrvatski != null && wordExamplesViewModelToReturn.TranslaatedTextToReturn[j].Angliski != null)
                    {
                        SessinTranslatedSensesSumary[i].Makedonski = wordExamplesViewModelToReturn.TranslaatedTextToReturn[j].Makedonski;
                        SessinTranslatedSensesSumary[i].Hrvatski = wordExamplesViewModelToReturn.TranslaatedTextToReturn[j].Hrvatski;
                        SessinTranslatedSensesSumary[i].Angliski = wordExamplesViewModelToReturn.TranslaatedTextToReturn[j].Angliski;
                        SessinTranslatedSensesSumary[i].Germanski = wordExamplesViewModelToReturn.TranslaatedTextToReturn[j].Germanski;
                    }
                }
            }

            HttpContext.Session.SetObjectAsJson(_SessionSenses, wordExamplesViewModelToReturn.Senses);
            HttpContext.Session.SetObjectAsJson(_SessionTranslatedSenses, wordExamplesViewModelToReturn.TranslaatedTextToReturn);
            HttpContext.Session.SetObjectAsJson(_SessionTranslatedSensesSumary, SessinTranslatedSensesSumary);
            HttpContext.Session.SetString(_SessionWord, UserWordToTranslate);


            //return wordExamplesViewModelToReturn;
            return PartialView("_sensesPartialView", wordExamplesViewModelToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> WordExamplesPartialViewResult(PaginatedList<TranslationViewModel> wordExamplesViewModel)
        {
            return PartialView("_sensesPartialView", wordExamplesViewModel);
        }

        public async Task<TranslationViewModel> WordExamplesTextTranslate(String text)
        {
            TranslationViewModel translatedText = await MacedonianCroatianEnglishGermanTranslation.DeEnCroMk(text, _googleCloudPlatformProjectName.Value.ProjectName, _logger, _googleTranslateApiCredentials);
            /*translatedText.Makedonski = translatedText.Makedonski.Replace("/", "\\");
            translatedText.Hrvatski = translatedText.Hrvatski.Replace("/", "\\");
            translatedText.Angliski = translatedText.Angliski.Replace("/", "\\");
            translatedText.Germanski = translatedText.Germanski.Replace("/", "\\");*/
            //wordExamplesViewModelToReturn.Sentences.Add(translatedText);
            return translatedText;
        }

        private async Task<String> lemmas(String word)
        {
            if (String.IsNullOrEmpty(word))
            {
                return "";
            }

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://od-api.oxforddictionaries.com/api/v2/lemmas/de/" + word);

            Dictionary<string, string> queryString = new Dictionary<string, string>();

            queryString = new Dictionary<string, string>()
                {
                    { "strictMatch", "true"},
                    { "fields", "examples" }
                };

            var requestQueryString = QueryHelpers.AddQueryString("", queryString);
            var request = new HttpRequestMessage(HttpMethod.Get, requestQueryString);

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("app_id", _oxfordDictionariesApi.Value.app_id);
            request.Headers.Add("app_key", _oxfordDictionariesApi.Value.app_key);

            var httpResponse = await httpClient.SendAsync(request);

            JObject data = new JObject();

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                data = await getRequestJson(httpResponse);
                /*data = (JObject)data["results"];
                data = (JObject)data["lexicalEntries"];
                data = (JObject)data["entries"];*/
            }
            else if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return data["results"].Children()["lexicalEntries"].Children()["inflectionOf"].Children()["text"].FirstOrDefault().ToString();

        }

        [HttpPost]
        public async Task<TranslationViewModel> TranslatePartial(String tekstZaPreveduvanje)
        {
            //Da se napravi detectLanguage(tekstZaPreveduvanje)
            TranslationViewModel result = await MacedonianCroatianEnglishGermanTranslation.DeEnCroMk(tekstZaPreveduvanje, _googleCloudPlatformProjectName.Value.ProjectName, _logger, _googleTranslateApiCredentials);

            return result;
        }
    }
}
