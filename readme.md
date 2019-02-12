# Emergency Services Bot

A bot designed to allow for rapid deployment of a question and answer service with support for multiple languages and accessible over web and SMS.

## Background

Disaster response scenarios require time-critical response to provide relief to affected individuals.  Inspired by the aftermath of Hurricane Harvey, this project serves as a disaster response question and answer service to provide assistance for victims of natural disaster.  The service allows for an operator to quickly plug in question and answer responses into a knowledge base which become auto-translated to end users of the service. The project also provides out of box support for Web and SMS allowing for increased availability to individuals who may not have internet access. 

## Demo

Try a live demonstration of the Emergency Services Bot by visiting https://emergencyservicesbot-test.azurewebsites.net/ or texting "hi" to +1-484-552-4268

## Prerequisites

1.  An active [Microsoft Azure Subscription](http://azure.com)
2.  A registered bot at [botframework.com](https://dev.botframework.com/) 
3.  An active QnA Service from [qnamaker.ai](http://qnamaker.ai)
4.  An active [Translator Text API Key](https://www.microsoft.com/en-us/translator)

## QuickStart

In some steps you will be asked to obtain and save a key value pair.  Please be aware of this as you go through the steps and keep these values in a safe place.

1. Create or use an existing Azure Subscription 
    
    Free subscriptions can be obtained @ https://azure.microsoft.com/en-us/free/

2. Create an active QnA Service at [qnamaker.ai](http://qnamaker.ai)
    
    <img src="/Assets/CreateQnAService.PNG">
    
    You may now begin adding questions to your knowledge base
    
    <img src="/Assets/CreateQnAPair.PNG">

    Save / Retrain, and Publish your changes when you are ready

    <img src="/Assets/QnAPublish.PNG">

    Obtain and save the following from values from Settings => Deployment Details 
    
    (top value is for "QnAKnowledgebaseId", bottom is "QnASubscriptionKey")

    <img src="/Assets/GetQnAKeys.PNG">

3.  Create a Translator Text API Service @ http://azure.com 
    
     <img src="/Assets/TranslatorTextRegistration.PNG">-
    
    Obtain and save the value for the "TranslatorApiKey"
    
    <img src="/Assets/TranslatorTextKeys.PNG">
    
4. Create a bot channel registration within your Azure subscription 
    <img src="/Assets/CreateBotChannelRegistration.PNG"> 
    <img src="/Assets/BotChannelRegistration.PNG"> 

    Within the Bot Channel Registration, click Settings to obtain and Save the value in "Bot handle" as "BotName" and the value in "Microsoft App Id" as "MicrosoftAppId"

    <img src="/Assets/BotChannelSettings.PNG">

    Near the "Microsoft App Id" click "Manage" then "Generate New Password" to obtain and Save as "MicrosoftAppPassword"

    <img src="/Assets/BotChannelPassword.PNG">
    
    Select "Channels", then add a DirectLine Channel and obtain and save one of the secret keys as "DirectLineSecret"
    
    <img src="/Assets/BotChannelDirectLine.PNG">

5. Verify that you have a saved value for "QnAKnowledgebaseId", "QnASubscriptionKey", "BotName", "MicrosoftAppId", "MicrosoftAppPassword", and "DirectLineSecret"
    
    Once you have all of these [values](./Docs/azure-deploy-parameters.md), click the "Deploy to Azure" Button below and supply the values where indicated:
    
    [![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)
    
    <img src="/Assets/DeployToAzure.PNG">
    
    Ensure that deployment is successful
    
    <img src="/Assets/DeployToAzureSuccess.PNG">
    
    Head back to the bot channel from step 4, click Settings and update the value for Messaging Endpoint to your newly deployed url + "/api/messages"
    
    <img src="/Assets/UpdateBotChannel.PNG">
    
    Reload your deployed website and you should see the bot operating using the configured knowledgebase:
    
    <img src="/Assets/BotWebPage.PNG">
    
    <img src="/Assets/BotWebPageVerify.PNG">
    
    If you do not have success, verify that the appropriate values are being supplied for the website's AppSettings
    
    <img src="/Assets/VerifyAppSettings.PNG">

## Enable Mobile SMS Channel

To enable SMS messaging with the bot, follow the instructions @ https://docs.microsoft.com/en-us/bot-framework/bot-service-channel-connect-twilio

For other channels, see: https://docs.microsoft.com/en-us/bot-framework/bot-service-manage-channels

## Developing Locally in Visual Studio

1. Clone this repo
2. Open EmergencyServicesBot.sln in Visual Studio
3. Add appropriate keypairs from the QuickStart steps into "Web.config" (Note: Local development does not require use of "MicrosoftAppId" and "MicrosoftAppPassword")
4. Build and run the bot
5. Download and run [botframework-emulator](https://emulator.botframework.com/)
6. Connect the emulator to http://localhost:{deploymentPort}

### Publishing to Azure from Visual Studio

In Visual Studio, right click on EmergencyServicesBot and select 'Publish'

## Styling the bot

Within the include default.htm, there is an example of a landing page for the bot: 

* Custom styling of the bot's look and feel are controlled by "/Content/botchat.css"
* Custom bot behavior is controlled by "/Scripts/botchat.js"

By modifying these files, you can give the bot a custom look and feel to accommodate your use case.



