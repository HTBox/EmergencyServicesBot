using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Web;

namespace EmergencyServicesBot.Dialogs
{
    [Serializable]
    public class SetLanguage : IDialog<object>
    {
        static ResourceManager translateDialog = new ResourceManager("EmergencyServicesBot.Resources.Resources", Assembly.GetExecutingAssembly());

        //Set the language to be used; you can change this on-demand to change the langauage across the app
        //You will pass this everytime you request a value from the resx file


        public async Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(
               context,
               this.onLanguageSelect,
               context.Activity.ChannelId == ChannelIds.Sms ? LanguageConst.smsLanguages : LanguageConst.languages,
               translateDialog.GetString("SetLanguage", context.UserData.GetValue<CultureInfo>("cultureInfo")));
        }

        private async Task onLanguageSelect(IDialogContext context, IAwaitable<string> result)
        {
            // set the language preference in userData
            var selectedLanguage = await result;
            var choice = await result;
            CultureInfo culture;

            if (choice.IndexOf(LanguageConst.enLanguageName, 0, StringComparison.OrdinalIgnoreCase) != -1)
            {
                choice = LanguageConst.enLanguageId;
                selectedLanguage = LanguageConst.enLanguageName;
                culture = LanguageConst.ciEnglish;
            }
            else if (choice.IndexOf(LanguageConst.esLanguageName, 0, StringComparison.OrdinalIgnoreCase) != -1)
            {
                choice = LanguageConst.esLanguageId;
                selectedLanguage = LanguageConst.esLanguageName;
                culture = LanguageConst.ciSpanish;
            }
            else if (choice.IndexOf(LanguageConst.zhLanguageName, 0, StringComparison.OrdinalIgnoreCase) != -1)
            {
                choice = LanguageConst.zhLanguageId;
                selectedLanguage = LanguageConst.zhLanguageName;
                culture = LanguageConst.ciChinese;
            }
            else if (choice.IndexOf(LanguageConst.frLanguageName, 0, StringComparison.OrdinalIgnoreCase) != -1)
            {
                choice = LanguageConst.frLanguageId;
                selectedLanguage = LanguageConst.frLanguageName;
                culture = LanguageConst.ciFrench;
            }
            else
            {
                choice = LanguageConst.enLanguageId;
                selectedLanguage = LanguageConst.enLanguageName;
                culture = LanguageConst.ciEnglish;
            }

            context.UserData.SetValue(@"userLanguage", choice);
            context.UserData.SetValue(@"cultureInfo", culture);

            var message = translateDialog.GetString("SetLanguageConfirmation", culture);

            await context.PostAsync(message + " " + selectedLanguage);

            context.Done(context);
        }
    }
}