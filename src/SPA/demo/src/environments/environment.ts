export const environment = {
    production: true,
    msalConfig: {
      auth: {
        clientId: '2c06ad20-d923-4ed5-84e7-4dadea8d7d92',
        authority: 'https://login.microsoftonline.com/16b3c013-d300-468d-ac64-7eda0820b6d3',
      },
    },
    apiConfig: {
      scopes: ['user.read'],
      uri: 'https://graph.microsoft.com/v1.0/me',
    },
    copilotApiConfig: {
      scopes: ['api://9d829b28-5191-4629-90ec-cbd98cda6053/access_as_user'],
      uri: 'https://localhost:7203/api',
    }
  };