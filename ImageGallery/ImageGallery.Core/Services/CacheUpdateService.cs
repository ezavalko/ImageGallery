using ImageGallery.Core.Api;
using ImageGallery.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ImageGallery.Core.Services
{
    public class CacheUpdateService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer _timer;

        private readonly IAPIImageClient _apiImageClient;
        private readonly ILogger<CacheUpdateService> _logger;
        private readonly IOptionsSnapshot<CacheOptions> _cacheOptions;

        public CacheUpdateService(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            _apiImageClient = scope.ServiceProvider.GetService<IAPIImageClient>();
            _logger = scope.ServiceProvider.GetService<ILogger<CacheUpdateService>>();
            _cacheOptions = scope.ServiceProvider.GetService<IOptionsSnapshot<CacheOptions>>();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(UpdateImageCache, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(_cacheOptions.Value.UpdateIntervalInMinutes));

            return Task.CompletedTask;
        }

        private void UpdateImageCache(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _apiImageClient.RefreshImagesData();

            _logger.LogInformation("Cache Update Service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Cache Update Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
