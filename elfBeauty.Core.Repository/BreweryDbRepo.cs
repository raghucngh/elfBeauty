using elfBeauty.App.Dto;
using elfBeauty.Core.Entities;
using elfBeauty.Core.IRepository;
using elfBeauty.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace elfBeauty.Core.Repository
{
    public class BreweryDbRepo : IBreweryDbRepo
    {
        private readonly AestheticDbContext _ctx;

        public BreweryDbRepo(AestheticDbContext ctx)
        {
            _ctx = ctx;
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
            var breweriesQuery = _ctx.Aesthetics.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                breweriesQuery = breweriesQuery.Where(b => b.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                           b.City.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var query1 = (await breweriesQuery.ToListAsync())
                         .Select(br =>
                          {
                              double.TryParse(br.Latitude?.ToString(), out var lat1);
                              double.TryParse(br.Longitude?.ToString(), out var long1);
                              double? dist = (userLat.HasValue && userLong.HasValue) ? (new Utility().Haversine(userLat.Value, userLong.Value, lat1, long1)) : null;
                              return (Brewery: br, Distance: dist);
                          });
            var breweries = from a in query1
                            select new AestheticDto { Name = a.Brewery.Name, City = a.Brewery.City, Phone = a.Brewery.Phone, Distance = a.Distance };

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
