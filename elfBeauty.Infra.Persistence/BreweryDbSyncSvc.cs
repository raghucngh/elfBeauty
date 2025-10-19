using elfBeauty.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace elfBeauty.Infra.Persistence
{
    public class BreweryDbSyncSvc : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<BreweryDbSyncSvc> _logger;

        public BreweryDbSyncSvc(IConfiguration config, IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory, ILogger<BreweryDbSyncSvc> logger)
        {
            _serviceProvider = serviceProvider;
            _httpClientFactory = httpClientFactory;
            _config = config;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(Const.SyncStartLog);
                string? breweryUrl = _config.GetSection(Const.BreweryDSKey).Value;
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var response = await client.GetAsync(breweryUrl);

                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    var breweries = JsonSerializer.Deserialize<List<AestheticDb>>(responseBody);

                    await SeedDatabaseAsync(breweries!);

                    _logger.LogInformation(Const.SyncSuccessLog);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{Const.SyncFailureLog}: {ex.Message}");
                }

                // Syncs in every 10 mins
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }

        /// <summary>
        /// Seeds SQLite database for Aesthetics
        /// </summary>
        async Task SeedDatabaseAsync(List<AestheticDb> aesthetics)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AestheticDbContext>();
                if (!db.Aesthetics.Any())
                {
                    db.Aesthetics.AddRange(aesthetics.Select(s => new Aesthetic
                    {
                        Name = s.Name,
                        City = s.City,
                        Phone = s.Phone,
                        Latitude = s.Latitude,
                        Longitude = s.Longitude
                    }));
                    await db.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// DeleteAll from SQLite database table - Aesthetics
        /// </summary>
        async Task TruncateDatabaseAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AestheticDbContext>();

                await db.Database.ExecuteSqlRawAsync("DELETE FROM [Aesthetics]");
            }
        }

    }
}
