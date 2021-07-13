import { Config } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'DevExtremeAngular',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:5001',
    redirectUri: baseUrl,
    clientId: 'Idea_App',
    responseType: 'code',
    scope: 'openid offline_access Idea',
  },
  apis: {
    default: {
      url: 'https://localhost:5001',
      rootNamespace: 'Idea',
    },
  },
} as Config.Environment;
