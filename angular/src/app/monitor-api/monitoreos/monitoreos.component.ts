import { Component, OnInit } from '@angular/core';
import { MonitoreoApiService } from '../../proxy/series/monitoreo-api.service';
import { ConfigStateService } from '@abp/ng.core';

@Component({
  selector: 'app-monitoreos',
  templateUrl: './monitoreos.component.html',
  styleUrls: ['./monitoreos.component.scss']
})
export class MonitoreosComponent implements OnInit {
  monitoreos: any[] = [];
  estadisticas: any | null = null;
  currentUserId: string | undefined;

  constructor(
    private monitoreoService: MonitoreoApiService,
    private configState: ConfigStateService
  ) { }

  ngOnInit(): void {
    const currentUser = this.configState.getOne('currentUser');
    this.currentUserId = currentUser?.id;
    this.mostrarMonitoreosEnBD();
    this.mostrarEstadisticasMonitoreos();
  }

  public mostrarMonitoreosEnBD(): void {
    this.monitoreoService.mostrarMonitoreos().subscribe(
      response => {
        this.monitoreos = response || [];
      },
      error => console.error('Error al cargar monitoreos:', error)
    );
  }

  public mostrarEstadisticasMonitoreos(): void {
    this.monitoreoService.getEstadisticas().subscribe(
      response => {
        this.estadisticas = response || null;
      },
      error => console.error('Error al cargar estadísticas:', error)
    );
  }
}