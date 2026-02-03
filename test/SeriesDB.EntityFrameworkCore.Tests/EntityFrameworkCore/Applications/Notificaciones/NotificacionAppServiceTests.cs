using Microsoft.EntityFrameworkCore;
using SeriesDB.Domain.Notificaciones;
using SeriesDB.EntityFrameworkCore;
using SeriesDB.Notificaciones;
using SeriesDB.Repositories.Notificaciones;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Xunit;

namespace SeriesDB.Tests.Notificaciones
{
    public abstract class NotificacionServiceTests<TStartupModule> : SeriesDBTestBase<TStartupModule>
    where TStartupModule : IAbpModule
    {
        protected readonly INotificacionAppService _notificacionAppService;
        private readonly SeriesDBDbContext _dbContext;
        protected readonly IConfigNotificacionRepository _configNotificacionRepository;

        protected NotificacionServiceTests()
        {
            _notificacionAppService = GetRequiredService<INotificacionAppService>();
            _dbContext = GetRequiredService<SeriesDBDbContext>();
            _configNotificacionRepository = GetRequiredService<IConfigNotificacionRepository>();
        }


        // Método helper para crear usuarios de prueba
        protected async Task<Guid> CreateTestUserAsync(Guid? userId = null)
        {
            var id = userId ?? Guid.NewGuid();
            var testUser = new IdentityUser(
                id,
                $"testuser_{id:N}",
                $"test_{id:N}@example.com"
            );
            
            await _dbContext.Users.AddAsync(testUser);
            await _dbContext.SaveChangesAsync();
            
            return id;
        }


        // Verifica que el método <c>MostrarNotificacionesPantalla</c> retorne una lista de notificaciones no vacía
        // cuando se llama con un usuario que tiene notificaciones no leídas.
        [Fact]
        public async Task MostrarNotificacionesPantalla_Should_Return_Unread_Notifications()
        {
            // Arrange
            var idUsuario = await CreateTestUserAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
            
            // Seed test notifications
            await _dbContext.Notificaciones.AddRangeAsync(
                new Notificacion
                {
                    IdUsuario = idUsuario,
                    Titulo = "Test Notification 1",
                    Mensaje = "Test Message 1",
                    Leida = false,
                    Tipo = TipoNotificacion.Pantalla,
                    FechaCreacion = DateTime.Now
                },
                new Notificacion
                {
                    IdUsuario = idUsuario,
                    Titulo = "Test Notification 2",
                    Mensaje = "Test Message 2",
                    Leida = false,
                    Tipo = TipoNotificacion.Email,
                    FechaCreacion = DateTime.Now.AddMinutes(-5)
                }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var notificacionesDto = _notificacionAppService.MostrarNotificacionesPantalla(idUsuario);

            // Assert
            notificacionesDto.ShouldNotBeEmpty();
            notificacionesDto.First().IdUsuario.ShouldBe(idUsuario);
            notificacionesDto.Count.ShouldBeGreaterThanOrEqualTo(2);
        }


        // Verifica que el método <c>CrearYEnviarNotificacionAsync</c> cree y envíe una notificación correctamente.
        [Fact]
        public async Task CrearYEnviarNotificacionAsync_Should_Create_And_Send_Notification()
        {
            // Arrange
            var idUsuario = await CreateTestUserAsync(Guid.Parse("00000000-0000-0000-0000-000000000002"));
            var titulo = "Nuevo Titulo";
            var mensaje = "Nuevo Mensaje";
            var tipo = TipoNotificacion.Email;

            // Act
            await _notificacionAppService.CrearYEnviarNotificacionAsync(idUsuario, titulo, mensaje, tipo);

            // Assert: verifica en la base de datos
            var notificacionEnDb = await _dbContext.Notificaciones
                .FirstOrDefaultAsync(n => n.IdUsuario == idUsuario && n.Titulo == titulo && n.Mensaje == mensaje);

            notificacionEnDb.ShouldNotBeNull();
            notificacionEnDb.Titulo.ShouldBe(titulo);
            notificacionEnDb.Mensaje.ShouldBe(mensaje);
            notificacionEnDb.Tipo.ShouldBe(tipo);
            notificacionEnDb.Leida.ShouldBe(false);
        }
    }

    // Concrete implementation that Test Explorer can discover
    public class NotificacionAppServiceTests : NotificacionServiceTests<SeriesDBEntityFrameworkCoreTestModule>
    {
        [Fact]
        public async Task Should_Update_Notification_Configuration()
        {
            // Arrange
            var usuarioId = await CreateTestUserAsync();
            ConfigNotificacion config = null;

            await WithUnitOfWorkAsync(async () =>
            {
                // Crear configuración inicial
                config = new ConfigNotificacion(usuarioId, notificacionPantalla: true, notificacionEmail: false);
                await _configNotificacionRepository.InsertAsync(config);
            });

            // Act
            await WithUnitOfWorkAsync(async () =>
            {
                await _notificacionAppService.ModificarConfiguracionNotificacionAsync(
                    usuarioId,
                    notificacionPantalla: false,
                    notificacionEmail: true
                );
            });

            // Assert
            await WithUnitOfWorkAsync(async () =>
            {
                var configActualizada = await _configNotificacionRepository.GetAsync(x => x.IdUsuario == usuarioId);
                configActualizada.ShouldNotBeNull();
                configActualizada.NotificacionPantalla.ShouldBeFalse();
                configActualizada.NotificacionEmail.ShouldBeTrue();
            });
        }

        [Fact]
        public async Task Should_Enable_Both_Notifications()
        {
            // Arrange
            var usuarioId = await CreateTestUserAsync();

            await WithUnitOfWorkAsync(async () =>
            {
                var config = new ConfigNotificacion(usuarioId, notificacionPantalla: false, notificacionEmail: false);
                await _configNotificacionRepository.InsertAsync(config);
            });

            // Act
            await WithUnitOfWorkAsync(async () =>
            {
                await _notificacionAppService.ModificarConfiguracionNotificacionAsync(
                    usuarioId,
                    notificacionPantalla: true,
                    notificacionEmail: true
                );
            });

            // Assert
            await WithUnitOfWorkAsync(async () =>
            {
                var configActualizada = await _configNotificacionRepository.GetAsync(x => x.IdUsuario == usuarioId);
                configActualizada.NotificacionPantalla.ShouldBeTrue();
                configActualizada.NotificacionEmail.ShouldBeTrue();
            });
        }
    }
}