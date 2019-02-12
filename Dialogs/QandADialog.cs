using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace EmergencyServicesBot.Dialogs
{
    [Serializable]
    public class QandADialog : IDialog<object>
    {
        static ResourceManager translateDialog = new ResourceManager("EmergencyServicesBot.Resources.Resources", Assembly.GetExecutingAssembly());

        public Task StartAsync(IDialogContext context)
        {
            var message = translateDialog.GetString("AskQuestion", context.UserData.GetValue<CultureInfo>("cultureInfo"));
            PromptDialog.Text(context, QuestionAsked, message);

            return Task.CompletedTask;
        }

        private async Task QuestionAsked(IDialogContext context, IAwaitable<string> result)
        {
            await QuestionAskedImpl(context, await result);
        }

        private async Task QuestionAskedviaMessageAsync(IDialogContext context, IAwaitable<object> result)
        {
            var msg = (IMessageActivity)(await result);

            await QuestionAskedImpl(context, msg.Text);
        }

        private async Task QuestionAskedImpl(IDialogContext context, string question)
        {
            if (IsDoneCommand(question))
            {
                context.Done(default(object));
            }
            else
            {
                using (var qnaClient = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings[@"QnAendpoint"] + ConfigurationManager.AppSettings[@"QnAKnowledgebaseId"] + "/generateAnswer" )}) 
                {
                    qnaClient.DefaultRequestHeaders.Add("Authorization", $"EndpointKey {ConfigurationManager.AppSettings[@"QnASubscriptionKey"]}");

                    string ApiKey = ConfigurationManager.AppSettings[@"TranslatorApiKey"];
                    string targetLang = context.UserData.GetValue<string>(@"userLanguage");
                    string knowledgeBaseLang = ConfigurationManager.AppSettings[@"QnAKnowledgebaseLanguage"];
                    var accessToken = await GetAuthenticationToken(ApiKey);

                    var httpResponse = new HttpResponseMessage();

                    if (targetLang != knowledgeBaseLang)
                    {
                        var translatedQuestion = await TranslateText(question, knowledgeBaseLang, accessToken);
                        httpResponse = await qnaClient.PostAsJsonAsync(string.Empty, new { question = translatedQuestion });
                    }
                    else
                    {
                        httpResponse = await qnaClient.PostAsJsonAsync(string.Empty, new { question = question });
                    }

                    try
                    {
                        var qnaResponse = JObject.Parse(await httpResponse.Content.ReadAsStringAsync());
                        Trace.TraceInformation($@"QnA Response to ""{question}"": {qnaResponse.ToString()}");

                        var answerThreshold = double.Parse(ConfigurationManager.AppSettings[@"QnAanswerThreshold"]);
                        Trace.TraceInformation($@"Answer threshold: {answerThreshold}");

                        var possibleAnswers = qnaResponse[@"answers"]
                            .Where(answer => answer.Value<double>(@"score") >= answerThreshold)
                            .Select(answer => answer.Value<string>(@"answer"));

                        if (possibleAnswers.Any())
                        {
                            var initialComment = translateDialog.GetString("AnswerQuestion", context.UserData.GetValue<CultureInfo>("cultureInfo"));
                            await context.PostAsync(initialComment)
                                .ContinueWith(t =>
                                {
                                    foreach (var a in possibleAnswers)
                                    {
                                        //// don't use await here because we want to block until these messages come out
                                        //context.PostAsync(a).GetAwaiter().GetResult();

                                        Task.Run(async () =>
                                        {
                                            string output;

                                            if (targetLang != knowledgeBaseLang)
                                                output = await TranslateText(a, targetLang, accessToken);
                                            else
                                                output = a;

                                            Console.WriteLine(output);

                                            context.PostAsync(output).GetAwaiter().GetResult();

                                        }).Wait();

                                    }
                                });
                        }
                        else
                        {
                            await context.PostAsync(translateDialog.GetString("NoAnswer", context.UserData.GetValue<CultureInfo>("cultureInfo")));
                        }

                        PromptDialog.Text(context, QuestionAsked, translateDialog.GetString("NewQuestion", context.UserData.GetValue<CultureInfo>("cultureInfo")));
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError($@"QnA endpoint error for question ""{question}"" : {ex.ToString()}");

                        PromptDialog.Text(context, QuestionAsked, translateDialog.GetString("ErrorQnA", context.UserData.GetValue<CultureInfo>("cultureInfo")));
                    }
                }
            }
        }

        static async Task<string> TranslateText(string inputText, string language, string accessToken)
        {
            string url = "http://api.microsofttranslator.com/v2/Http.svc/Translate";
            string query = $"?text={System.Net.WebUtility.UrlEncode(inputText)}&to={language}&contentType=text/plain";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync(url + query);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return "Hata: " + result;

                var translatedText = XElement.Parse(result).Value;
                return translatedText;
            }
        }

        static async Task<string> GetAuthenticationToken(string key)
        {
            string endpoint = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
                var response = await client.PostAsync(endpoint, null);
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
        }

        //TODO change to use resources
        private bool IsDoneCommand(string commandText) =>
            commandText.Equals(@"done", StringComparison.OrdinalIgnoreCase)
            || commandText.StartsWith(@"no", StringComparison.OrdinalIgnoreCase)
            || commandText.Equals(@"exit", StringComparison.OrdinalIgnoreCase)
            || commandText.Equals(@"quitter", StringComparison.OrdinalIgnoreCase)
            || commandText.Equals(@"quit", StringComparison.OrdinalIgnoreCase);
    }
}