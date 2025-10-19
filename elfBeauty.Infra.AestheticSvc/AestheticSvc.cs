using elfBeauty.App.Dto;
using elfBeauty.App.Interfaces;
using elfBeauty.Core.Entities;
using elfBeauty.Core.IRepository;
using Microsoft.Extensions.Logging;

namespace elfBeauty.Infra.AestheticSvc
{
    public class AestheticSvc : IAestheticSvc
    {
        private readonly HttpClient _httpClient;
        private readonly IBreweryCacheRepo _breweryCacheRepo;
        private readonly ILogger<AestheticSvc> _logger;

        public AestheticSvc(IBreweryCacheRepo breweryCacheRepo, ILogger<AestheticSvc> logger)
        {
            _logger = logger;
            _breweryCacheRepo = breweryCacheRepo;
        }

        public async Task<IEnumerable<AestheticDto>> GetAestheticsAsync_Cache(string? search = null, string? sortBy = null,
                                                                              double? userLat = null, double? userLong = null)
        {
            try
            {
                var breweries = await _breweryCacheRepo.GetBreweriesAsync(search, sortBy, userLat, userLong);

                return breweries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Const.FetchErr);
                throw;
            }
        }

        public async Task<IEnumerable<string?>> Autocomplete_Cache(string term)
        {
            try
            {
                var breweries = await _breweryCacheRepo.Autocomplete(searchByName: term);

                return breweries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Const.FetchErr);
                throw;
            }
        }

    }
}
