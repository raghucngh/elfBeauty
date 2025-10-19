using elfBeauty.App.Dto;
using elfBeauty.Core.Entities;
using elfBeauty.Core.IRepository;
using elfBeauty.Infra.Persistence;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace elfBeauty.Core.Repository
{
    public class BreweryCacheRepo : IBreweryCacheRepo
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;

        public BreweryCacheRepo(HttpClient httpClient, IConfiguration config, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _config = config;
            _cache = cache;
        }

        public async Task<IEnumerable<string?>> Autocomplete(string searchByName)
        {
            var breweries = await GetBreweriesAsync();
            if (!string.IsNullOrWhiteSpace(searchByName))
            {
                breweries = breweries.Where(b => b.Name.StartsWith(searchByName, StringComparison.OrdinalIgnoreCase));
            }

            return breweries.Select(b => b.Name).Distinct().Take(10);
        }

        public async Task<IEnumerable<AestheticDto>> GetBreweriesAsync(string? search = null, string? sortBy = null,
                                                                 double? userLat = null, double? userLong = null)
        {
            string? breweryUrl = _config.GetSection(Const.BreweryDSKey).Value;
            var query1 = (await _httpClient.GetFromJsonAsync<List<AestheticDb>>(breweryUrl))
                            .Select(br =>
                            {
                                double.TryParse(br.Latitude?.ToString(), out var lat1);
                                double.TryParse(br.Longitude?.ToString(), out var long1);
                                double? dist = (userLat.HasValue && userLong.HasValue) ? (new Utility().Haversine(userLat.Value, userLong.Value, lat1, long1)) : null;
                                return (Brewery: br, Distance: dist);
                            });
            var breweries = from a in query1
                            select new AestheticDto { Name = a.Brewery.Name, City = a.Brewery.City, Phone = a.Brewery.Phone, Distance = a.Distance };

            // Sets In-Memory cache with a ten minutes update
            new MemoryCache<AestheticDto>().SetBrewery(_cache);

            if (!string.IsNullOrWhiteSpace(search))
            {
                breweries = breweries.Where(br => br.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                                                  || br.City.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                breweries = sortBy.ToLower() switch
                {
                    "name" => breweries.OrderBy(br => br.Name).ToList()
                    ,
                    "city" => breweries.OrderBy(br => br.City).ToList()
                    ,
                    "distance" => breweries.OrderBy(o => o.Distance)
                    ,
                    _ => breweries
                };
            }

            return breweries;
        }
    }
}
