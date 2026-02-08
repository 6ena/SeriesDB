import type { MonitoreoApiDto, MonitoreoApiStatsDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class MonitoreoApiService {
    apiName = 'Default';


    errorMonitoreoByMonitoreoAndEx = (monitoreo: MonitoreoApiDto, ex: string, config?: Partial<Rest.Config>) =>
        this.restService.request<any, MonitoreoApiDto>({
            method: 'POST',
            url: '/api/app/monitoreo-api/error-monitoreo',
            params: { ex },
            body: monitoreo,
        },
            { apiName: this.apiName, ...config });


    finalizarMonitoreoByMonitoreo = (monitoreo: MonitoreoApiDto, config?: Partial<Rest.Config>) =>
        this.restService.request<any, MonitoreoApiDto>({
            method: 'POST',
            url: '/api/app/monitoreo-api/finalizar-monitoreo',
            body: monitoreo,
        },
            { apiName: this.apiName, ...config });


    getEstadisticas = (config?: Partial<Rest.Config>) =>
        this.restService.request<any, MonitoreoApiStatsDto>({
            method: 'GET',
            url: '/api/app/monitoreo-api/estadisticas',
        },
            { apiName: this.apiName, ...config });


    iniciarMonitoreo = (config?: Partial<Rest.Config>) =>
        this.restService.request<any, MonitoreoApiDto>({
            method: 'POST',
            url: '/api/app/monitoreo-api/iniciar-monitoreo',
        },
            { apiName: this.apiName, ...config });


    mostrarMonitoreos = (config?: Partial<Rest.Config>) =>
        this.restService.request<any, MonitoreoApiDto[]>({
            method: 'POST',
            url: '/api/app/monitoreo-api/mostrar-monitoreos',
        },
            { apiName: this.apiName, ...config });


    persistirMonitoreo = (monitoreoApiDto: MonitoreoApiDto, config?: Partial<Rest.Config>) =>
        this.restService.request<any, void>({
            method: 'POST',
            url: '/api/app/monitoreo-api/persistir-monitoreo',
            body: monitoreoApiDto,
        },
            { apiName: this.apiName, ...config });

    constructor(private restService: RestService) { }
}
