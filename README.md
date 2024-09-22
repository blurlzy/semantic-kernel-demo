# Copilot Chat Demo 
A Chat bot demo application built on top of Semantic Kernel.

## Semantic Kernel

```
https://github.com/microsoft/semantic-kernel
https://github.com/microsoft/semantic-kernel/tree/main/dotnet/samples
```

## Backend (Asp.net Core Web API)
### Installation

- Install Microsoft.SemanticKernel.
- Install Azure.Identity & Azure.Security.KeyVault.Secrets (Azure KeyVault integration)
- Install SharpToken (https://github.com/dmitry-brazhenko/SharpToken) (SharpToken is a C# library that serves as a port of the Python tiktoken library. It provides functionality for encoding and decoding tokens using GPT-based encodings.)

```
dotnet add package Microsoft.SemanticKernel
dotnet add package Microsoft.Identity.Web
dotnet add package Azure.Identity
dotnet add package Azure.Security.KeyVault.Secrets
dotnet add package SharpToken
```

### Azure Entra ID Configuration
```
  "AzureEntraId": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "<azure-tenant-id>", 
    "ClientId": "<app-registration-client-id>", 
    "Audience": "<app-regisration-application-uri>" // the default value is api://[client-id]
  },

```

### Register / Configure Authentication
```
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(configuration, "AzureEntraId"); // specify the configuration section name, the default value is "AzureAd"

```

### Add auth middleware to pipeline
```
app.UseAuthentication();
app.UseAuthorization();

```

## Front-end (Angular)
### Prerequisites

Before using `@azure/msal-angular`, [register an application in Azure AD](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to get your `clientId`.

### Installation

- Install @azure/msal-angular 
- Install @angular/material 
- Install bootstrap-icons
- Install ngx-markdown - ngx-markdown is an Angular library that combines...
  - Marked to parse markdown to HTML
  - Prism.js for language syntax highlight

```
npm install @azure/msal-browser @azure/msal-angular@latest
ng add @angular/material
npm i bootstrap-icons
npm install ngx-markdown marked@^12.0.0 --save
npm install prismjs@^1.28.0 --save
```