import { Component, OnInit } from '@angular/core';
import { SerieService } from '../../proxy/series/serie.service';
import { ConfigStateService } from '@abp/ng.core';

@Component({
  selector: 'app-series-persistidas',
  templateUrl: './series-persistidas.component.html',
  styleUrls: ['./series-persistidas.component.scss']
})
export class SeriesPersistidasComponent implements OnInit {
  persistedSeries: any[] = [];
  currentUserId: string | undefined;

  constructor(
    private serieService: SerieService,
    private configState: ConfigStateService
  ) { }

  ngOnInit(): void {
    const currentUser = this.configState.getOne('currentUser');
    this.currentUserId = currentUser?.id;
    this.loadPersistedSeries();
  }

  loadPersistedSeries() {
    this.serieService.getList({ maxResultCount: 100, skipCount: 0 }).subscribe(
      response => {
        this.persistedSeries = response.items || [];
      },
      error => console.error('Error al cargar series persistidas:', error)
    );
  }
}