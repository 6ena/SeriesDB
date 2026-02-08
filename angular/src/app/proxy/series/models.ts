import type { EntityDto } from '@abp/ng.core';

export interface CalificacionDto {
  nroCalificacion: number;
  comentario?: string;
  fechaCreacion?: string;
  serieID: number;
  idUsuario?: string;
}

export interface EpisodioDto extends EntityDto<number> {
  nroEpisodio: number;
  titulo?: string;
  duracion?: string;
  resumen?: string;
  fechaEstreno?: string;
  directores?: string;
  escritores?: string;
  temporadaID: number;
}

export interface SerieDto extends EntityDto<number> {
  titulo?: string;
  generos?: string;
  sinopsis?: string;
  fechaEstreno?: string;
  duracion?: string;
  clasificacion?: string;
  idiomas?: string;
  directores?: string;
  escritores?: string;
  actores?: string;
  poster?: string;
  pais?: string;
  imdbId?: string;
  imdbCalificacion?: string;
  imdbVotos: number;
  tipo?: string;
  totalTemporadas: number;
  temporadas: TemporadaDto[];
  calificaciones: CalificacionDto[];
}

export interface TemporadaDto extends EntityDto<number> {
  nroTemporada: number;
  titulo?: string;
  fechaLanzamiento?: string;
  serieID: number;
  episodios: EpisodioDto[];
}

export interface CreateUpdateSerieDto {
  titulo?: string;
  genero?: string;
}

export interface MonitoreoApiDto extends EntityDto<number> {
  horaAcceso?: string;
  horaFin?: string;
  tiempoRespuesta: number;
  errores: string[];
}

export interface MonitoreoApiStatsDto {
  promedioDuracion: number;
  totalErrores: number;
  totalMonitoreos: number;
}
