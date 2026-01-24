import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44314/',
  redirectUri: baseUrl,
  clientId: 'SerieDB_App',
  responseType: 'code',
  scope: 'offline_access SerieDB',
  requireHttps: true,
};

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'SerieDB',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44314',
      rootNamespace: 'SerieDB',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge'
  }
} as Environment;
