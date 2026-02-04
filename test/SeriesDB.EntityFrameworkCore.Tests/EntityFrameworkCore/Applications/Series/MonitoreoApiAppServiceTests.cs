using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;
using SeriesDB.EntityFrameworkCore;
using SeriesDB.Series;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace SeriesDB.Tests.MonitoreoApi
{
    public abstract class MonitoreoApiAppServiceTest<TStartupModule> : SeriesDBTestBase<TStartupModule>
    where TStartupModule : IAbpModule
    {
        private readonly IMonitoreoApiAppService _monitoreoApiAppService;
        private readonly SeriesDBDbContext _dbContext;

        protected MonitoreoApiAppServiceTest()
        {
            _monitoreoApiAppService = GetRequiredService<IMonitoreoApiAppService>();
            _dbContext = GetRequiredService<SeriesDBDbContext>();
        }

        /// <summary>
        /// Prueba unitaria para verificar que el método PersistirMonitoreoAsync de MonitoreoApiAppService
        /// llama al método InsertAsync del repositorio y persiste correctamente un objeto MonitoreoApi en la base de datos.
        /// </summary>
        /// <remarks>
        /// Esta prueba crea una instancia de MonitoreoApiDto con datos de prueba, llama al método PersistirMonitoreoAsync
        /// y luego verifica que el objeto MonitoreoApi correspondiente ha sido guardado en la base de datos.
        /// </remarks>
        [Fact]
        public async Task PersistirMonitoreoAsync_Should_Call_InsertAsync()
        {
            // Arrange
            var monitoreoDto = new MonitoreoApiDto
            {
                HoraAcceso = DateTime.Now,
                HoraFin = DateTime.Now.AddMinutes(1),
                TiempoRespuesta = 60,
                Errores = new List<string> { "Error de prueba" }
            };

            // Act
            await _monitoreoApiAppService.PersistirMonitoreoAsync(monitoreoDto);

            // Assert
            var monitoreoEnDb = await _dbContext.MonitoreosApi
                .FirstOrDefaultAsync(m => m.TiempoRespuesta == 60);

            monitoreoEnDb.ShouldNotBeNull(); // Verifica que el monitoreo fue guardado
            monitoreoEnDb.TiempoRespuesta.ShouldBe(60); // Verifica que los datos coinciden

        }

        /// <summary>
        /// Prueba unitaria para verificar que el método MostrarMonitoreosAsync de MonitoreoApiAppService
        /// retorna una lista de monitoreos almacenados en la base de datos.
        /// </summary>
        /// <remarks>
        /// Esta prueba llama al método MostrarMonitoreosAsync y verifica que la lista de monitoreos
        /// retornada no esté vacía.
        /// </remarks>
        [Fact]
        public async Task MostrarMonitoreosAsync_Should_Show_Monitoreos()
        {
            // Arrange - Create test data
            var monitoreoDto = new MonitoreoApiDto
            {
                HoraAcceso = DateTime.Now,
                HoraFin = DateTime.Now.AddMinutes(1),
                TiempoRespuesta = 30,
                Errores = new List<string> { "Error de prueba" }
            };
            await _monitoreoApiAppService.PersistirMonitoreoAsync(monitoreoDto);

            //Act
            var monitoreoApiDto = await _monitoreoApiAppService.MostrarMonitoreosAsync();

            //Assert
            Assert.NotEmpty(monitoreoApiDto);
            monitoreoApiDto.ShouldContain(m => m.TiempoRespuesta == 30);
        }

        /// <summary>
        /// Prueba unitaria para verificar que el método ObtenerEstadisticasAsync de MonitoreoApiAppService
        /// calcula correctamente las estadísticas de los monitoreos almacenados en la base de datos.
        /// </summary>
        /// <remarks>
        /// Esta prueba llama al método ObtenerEstadisticasAsync y verifica que las estadísticas calculadas
        /// (total de monitoreos, promedio de duración y total de errores) sean correctas según los datos
        /// de prueba proporcionados.
        /// </remarks>
        [Fact]
        public async Task GetEstadisticasAsync_Should_Calculate_Statistics_Correctly()
        {
            // Arrange - Create test data
            var monitoreo1 = new MonitoreoApiDto
            {
                HoraAcceso = DateTime.Now,
                HoraFin = DateTime.Now.AddMinutes(1),
                TiempoRespuesta = 20,
                Errores = new List<string> { "Error 1" }
            };
            var monitoreo2 = new MonitoreoApiDto
            {
                HoraAcceso = DateTime.Now,
                HoraFin = DateTime.Now.AddMinutes(1),
                TiempoRespuesta = 30,
                Errores = new List<string> { "Error 2", "Error 3" }
            };
            var monitoreo3 = new MonitoreoApiDto
            {
                HoraAcceso = DateTime.Now,
                HoraFin = DateTime.Now.AddMinutes(1),
                TiempoRespuesta = 45,
                Errores = new List<string> { "Error 4" }
            };

            await _monitoreoApiAppService.PersistirMonitoreoAsync(monitoreo1);
            await _monitoreoApiAppService.PersistirMonitoreoAsync(monitoreo2);
            await _monitoreoApiAppService.PersistirMonitoreoAsync(monitoreo3);

            // Act
            var estadisticas = await _monitoreoApiAppService.GetEstadisticasAsync();

            // Assert
            estadisticas.ShouldNotBeNull();
            estadisticas.CantidadMonitoreos.ShouldBe(3);
            estadisticas.PromedioTiempoRespuesta.ShouldBe(31.666666f, 0.01f); // (20+30+45)/3 = 31.666...
            estadisticas.CantidadErrores.ShouldBe(4); // 1 + 2 + 1 = 4
        }
    }

    // Concrete implementation for Entity Framework Core tests
    public class MonitoreoApiAppServiceTests : MonitoreoApiAppServiceTest<SeriesDBEntityFrameworkCoreTestModule>
    {
    }
}