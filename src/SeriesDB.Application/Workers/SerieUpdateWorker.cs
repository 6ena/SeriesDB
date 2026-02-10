using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SeriesDB.Series
{
    public class SerieUpdateWorker : IHostedService, IDisposable
    {
        private readonly ILogger<SerieUpdateWorker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;
        private bool _isRunning;

        public SerieUpdateWorker(
            ILogger<SerieUpdateWorker> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_isRunning)
            {
                return Task.CompletedTask;
            }

            _logger.LogInformation("SerieUpdateWorker starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(86400000));
            _isRunning = true;
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _ = DoWorkAsync(state);
        }

        private async Task DoWorkAsync(object state)
        {
            _logger.LogInformation("SerieUpdateWorker running verification.");
            
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var serieUpdateService = scope.ServiceProvider.GetRequiredService<ISerieUpdateService>();
                await serieUpdateService.VerificarYActualizarSeriesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SerieUpdateWorker stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            _isRunning = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}