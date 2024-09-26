export const environment = {
    production: true,
    msalConfig: {
      auth: {
        clientId: '16deee61-e9e6-4493-b1a3-57ed14e1c33b',
        authority: 'https://login.microsoftonline.com/885ba0b1-68d5-40e2-adaa-238cb699dbdc',
      },
    },
    apiConfig: {
      scopes: ['user.read'],
      uri: 'https://graph.microsoft.com/v1.0/me',
    },
    copilotApiConfig: {
      scopes: ['api://eaa59094-d365-45df-a7c5-aae17459e3b9/access_as_user'],
      uri: 'https://aca-chat-coplit-api.redground-a7872c78.australiaeast.azurecontainerapps.io/api',
    }
  };