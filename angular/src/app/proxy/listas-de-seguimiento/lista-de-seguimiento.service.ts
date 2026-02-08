import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { SerieDto } from '../series/models';

@Injectable({
  providedIn: 'root',
})
export class ListaDeSeguimientoService {
  apiName = 'Default';
  

  addSerieLista = (serieDto: SerieDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/lista-de-seguimiento/serie-lista',
      body: serieDto,
    },
    { apiName: this.apiName,...config });
  

  buscarSeriesLista = (titulo: string, genero?: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SerieDto[]>({
      method: 'POST',
      url: '/api/app/lista-de-seguimiento/buscar-series-lista',
      params: { titulo, genero },
    },
    { apiName: this.apiName,...config });
  

  getSeriesLista = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, SerieDto[]>({
      method: 'GET',
      url: '/api/app/lista-de-seguimiento/series-lista',
    },
    { apiName: this.apiName,...config });
  

  removeSerieLista = (ImdbId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/lista-de-seguimiento/serie-lista/${ImdbId}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
