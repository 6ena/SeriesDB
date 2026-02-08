import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { SerieService } from '../../proxy/series/serie.service';
import { SerieDto } from '../../proxy/series/models';
import { ConfigStateService } from '@abp/ng.core';

@Component({
  selector: 'app-series',
  templateUrl: './series.component.html',
  styleUrl: './series.component.scss'
})
export class SeriesComponent implements OnInit {
  series: any[] = [];
  serieTitle: string = "";
  persisting: { [key: string]: boolean } = {};
  persisted: { [key: string]: boolean } = {};
  currentUserId: string | undefined;

  constructor(
    private serieService: SerieService,
    private configState: ConfigStateService
  ) { }

  ngOnInit(): void {
    const currentUser = this.configState.getOne('currentUser');
    this.currentUserId = currentUser?.id;
  }

  public searchSeries() {
    if (this.serieTitle.trim()) {
      this.serieService.buscarSerie(this.serieTitle.trim(), "").subscribe(
        response => {
          this.series = response || [];
          console.log('Series encontradas:', this.series);
          this.series.forEach(serie => {
            console.log('Serie DTO:', serie);
          });
        },
        error => console.error('Error al buscar series:', error)
      );
    }
  }

  public persistSerie(serie: any) {
    console.log('Persistiendo serie con ImdbId:', serie.imdbId);

    // Asegurarse de que los campos requeridos estén presentes
    if (!serie.temporadas) {
      serie.temporadas = []; // Inicializar como un array vacío si no está presente
    }
    if (!serie.calificaciones) {
      serie.calificaciones = []; // Inicializar como un array vacío si no está presente
    }
    if (!serie.imdbId) {
      console.error('Error: ImdbId es requerido');
      return;
    }

    console.log('Datos de la serie a persistir:', serie);

    this.persisting[serie.imdbId] = true;
    this.serieService.persistirSerie(serie).subscribe(
      () => {
        console.log('Serie persistida con éxito:', serie.imdbId);
        this.persisting[serie.imdbId] = false;
        this.persisted[serie.imdbId] = true;
      },
      (error: HttpErrorResponse) => {
        console.error('Error al persistir serie:', error.message);
        console.error('Detalles del error:', error.error);
        this.persisting[serie.imdbId] = false;
      }
    );
  }
}