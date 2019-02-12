# Azure Deployment Parameters

| Parameter      | Description |           |
| -------------- | ------------| --------- |
| Directory      | Defaults to `Microsoft` | Required  |
| Subscription   | [Get an azure subscription](https://azure.microsoft.com/en-us/free/). | Required |
| Resource Group | Through `deploy to azure`, you may choose an existing resource group, or create a new one. | Required |
| Site Name | The name used to prefix the Azure DNS record *sitename*.azurewebsites.net     | Required |
| Sku | The App Service Pricing Sky - See: https://azure.microsoft.com/en-us/pricing/details/app-service/windows/ | Required |
| Worker Size | The size of the Azure Web App Instance (Small, Medium, Large) | Required |
| Microsoft App Id | The App Id assigned when creating a Bot Channel Registration | Required |
| Microsoft App Password | The password generated for the Bot Channel Registration | Required |
| Bot Name | The name assigned to your bot when creating a Bot Channel Registration | Required |
| Directline Secret | The generated secret when configuring the Directline channel | Required |
| QnA Endpoint | The url to the created QnA Maker service endpoint (provided) | Required |
| QnA Knowledgebase Id | The id of the QnA Maker knoweldge base | Required |
| QnA Subscription Key | The subscription key for accessing your QnA maker instance| Required |
| QnA Knowledgebase Language | The prefix for the language of the knowledgebase (default: 'en') | Required |
| QnA Answer Threshold | Confidence threshold to return result from the knowledgebase (default: '30')| Required |
| Translator Endpoint | The url to the Microsoft Translator service endpoint (provided) | Required |
| Translator Api Key | Api Key for accessing the Microsoft Translator service | Required |
