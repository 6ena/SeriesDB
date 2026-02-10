using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using SeriesDB.Series;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace SeriesDB.Tests.Series
{
    public class SerieUpdateWorkerTests
    {
        private readonly Mock<ILogger<SerieUpdateWorker>> _loggerMock;
        private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
        private readonly Mock<ISerieUpdateService> _serieUpdateServiceMock;
        private readonly SerieUpdateWorker _SerieUpdateWorker;

        public SerieUpdateWorkerTests()
        {
            _loggerMock = new Mock<ILogger<SerieUpdateWorker>>();
            _serieUpdateServiceMock = new Mock<ISerieUpdateService>();
            
            // Configura el mock para que no haga nada al invocar el método
            _serieUpdateServiceMock
                .Setup(s => s.VerificarYActualizarSeriesAsync())
                .Returns(Task.CompletedTask);

            // Mock del IServiceProvider
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(ISerieUpdateService)))
                .Returns(_serieUpdateServiceMock.Object);

            // Mock del IServiceScope
            var serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock
                .Setup(s => s.ServiceProvider)
                .Returns(serviceProviderMock.Object);

            // Mock del IServiceScopeFactory
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            _serviceScopeFactoryMock
                .Setup(f => f.CreateScope())
                .Returns(serviceScopeMock.Object);

            _SerieUpdateWorker = new SerieUpdateWorker(
                _loggerMock.Object,
                _serviceScopeFactoryMock.Object);
        }

        [Fact]
        public async Task StartAsync_Should_Start_Timer()
        {
            // Act
            await _SerieUpdateWorker.StartAsync(CancellationToken.None);

            // Assert
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == "SerieUpdateWorker starting."),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            
            // Cleanup
            await _SerieUpdateWorker.StopAsync(CancellationToken.None);
        }

        [Fact]
        public async Task StopAsync_Should_Stop_Timer()
        {
            // Arrange
            await _SerieUpdateWorker.StartAsync(CancellationToken.None);

            // Act
            await _SerieUpdateWorker.StopAsync(CancellationToken.None);

            // Assert
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == "SerieUpdateWorker stopping."),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task DoWork_Should_Call_UpdateService()
        {
            // Act - Don't start the timer, just invoke DoWork directly
            var method = typeof(SerieUpdateWorker)
                .GetMethod("DoWork", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            try
            {
                method.Invoke(_SerieUpdateWorker, new object[] { null });
                // Give async operation time to complete (DoWork calls DoWorkAsync as fire-and-forget)
                await Task.Delay(200);
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción para identificar el problema
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            // Assert
            _serieUpdateServiceMock.Verify(s => s.VerificarYActualizarSeriesAsync(), Times.Once);
        }


        [Fact]
        public void Dispose_Should_Dispose_Timer()
        {
            // Act
            _SerieUpdateWorker.Dispose();

            // Assert
            // Asegúrate de que no lanza una excepción
            _SerieUpdateWorker.Dispose();
        }
    }
}