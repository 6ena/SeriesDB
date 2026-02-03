using SeriesDB.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SeriesDB.Series
{
    public interface IMonitoreoApiAppService : IApplicationService
    {
        Task PersistirMonitoreoAsync(MonitoreoApiDto monitoreoDto);
        Task<MonitoreoApiDto[]> MostrarMonitoreosAsync();
        Task<MonitoreoApiStatsDto> GetEstadisticasAsync();
        Task<MonitoreoApiDto> IniciarMonitoreo();
        Task<MonitoreoApiDto> FinalizarMonitoreo(MonitoreoApiDto monitoreo);
        Task<MonitoreoApiDto> ErrorMonitoreo(MonitoreoApiDto monitoreo, string ex);
    }
}