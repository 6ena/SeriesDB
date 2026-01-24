import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44345/',
  redirectUri: baseUrl,
  clientId: 'SeriesDB_App',
  responseType: 'code',
  scope: 'offline_access SeriesDB',
  requireHttps: true,
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'SeriesDB',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44345',
      rootNamespace: 'SeriesDB',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
