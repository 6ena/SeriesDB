using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SeriesDB.Series;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using static Volo.Abp.Identity.IdentityPermissions;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace SeriesDB.Series
{
    public class MonitoreoApiAppService : ApplicationService, IMonitoreoApiAppService
    {
        private readonly IRepository<MonitoreoApi, int> _monitoreoApiRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly ILogger<MonitoreoApiAppService> _logger;

        public MonitoreoApiAppService(
            IRepository<MonitoreoApi, int> repository,
            IObjectMapper objectMapper,
            ILogger<MonitoreoApiAppService> logger)
        {
            _monitoreoApiRepository = repository;
            _objectMapper = objectMapper;
            _logger = logger;
        }

        /// <summary>
        /// Persiste un monitoreo en el repositorio.
        /// </summary>
        /// <param name="monitoreoApiDto">El DTO que contiene los datos del monitoreo a persistir.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        public async Task PersistirMonitoreoAsync(MonitoreoApiDto monitoreoApiDto)
        {
            try
            {
                var monitoreo = _objectMapper.Map<MonitoreoApiDto, MonitoreoApi>(monitoreoApiDto);
                await _monitoreoApiRepository.InsertAsync(monitoreo);
                _logger.LogInformation("Monitoreo persistido correctamente: {MonitoreoId}", monitoreo.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al persistir el monitoreo.");
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los monitoreos almacenados en el repositorio.
        /// </summary>
        /// <returns>Un arreglo de objetos <see cref="MonitoreoApiDto"/> que representan los monitoreos almacenados.</returns>
        /// <exception cref="Exception">Lanzada cuando no hay monitoreos registrados en el repositorio.</exception>
        public async Task<MonitoreoApiDto[]> MostrarMonitoreosAsync()
        {
            try
            {
                var monitoreos = await _monitoreoApiRepository.GetListAsync();

                if (monitoreos == null)
                {
                    _logger.LogWarning("No hay monitoreos registrados en el repositorio.");
                    throw new Exception("No hay Monitoreos Registrados.");
                }

                _logger.LogInformation("Monitoreos obtenidos correctamente.");
                return _objectMapper.Map<MonitoreoApi[], MonitoreoApiDto[]>(monitoreos.ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los monitoreos.");
                throw;
            }
        }

        
        public async Task<MonitoreoApiStatsDto> GetEstadisticasAsync()
        {
            try
            {
                var monitoreosDto = await MostrarMonitoreosAsync();

                var promedioTiempoRespuesta = monitoreosDto.Average(m => m.TiempoRespuesta);
                var cantidadlErrores = monitoreosDto.Sum(m => m.Errores.Count);
                var cantidadMonitoreos = monitoreosDto.Length;

                var estadisticasDto = new MonitoreoApiStatsDto
                {
                    PromedioTiempoRespuesta = promedioTiempoRespuesta,
                    CantidadErrores = cantidadlErrores,
                    CantidadMonitoreos = cantidadMonitoreos
                };

                _logger.LogInformation("Estadísticas obtenidas correctamente.");
                return estadisticasDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las estadísticas.");
                throw;
            }
        }
        public async Task<MonitoreoApiDto> IniciarMonitoreo()
        {
            var monitoreo = new MonitoreoApiDto
            {
                HoraAcceso = DateTime.Now
            };
            return monitoreo;
        }

        public async Task<MonitoreoApiDto> FinalizarMonitoreo(MonitoreoApiDto monitoreo)
        {
            monitoreo.HoraFin = DateTime.Now;
            monitoreo.TiempoRespuesta = (float)(monitoreo.HoraFin - monitoreo.HoraAcceso).TotalSeconds;
            return monitoreo;
        }
        public async Task<MonitoreoApiDto> ErrorMonitoreo(MonitoreoApiDto monitoreo, string ex)
        {
            monitoreo.HoraFin = DateTime.Now;
            monitoreo.TiempoRespuesta = (float)(monitoreo.HoraFin - monitoreo.HoraAcceso).TotalSeconds;
            monitoreo.Errores.Add("Excepción: " + ex);
            return monitoreo;
        }
    }
}