import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'WIZLOG',
    logoUrl: 'assets/images/logo/wiztechNewLogo_Black-01.png',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44337/',
    redirectUri: baseUrl,
    clientId: 'WIZLOG_App',
    responseType: 'code',
    scope: 'offline_access WIZLOG',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44337',
      rootNamespace: 'WIZLOG',
    },
  },
} as Environment;
