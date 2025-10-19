using elfBeauty.App.Dto;
using elfBeauty.Core.IRepository;
using elfBeauty.Infra.Persistence;

namespace elfBeauty.Core.Repository
{
    public class BreweryDbRepo : IBreweryDbRepo
    {
        private readonly AestheticDbContext _ctx;

        public BreweryDbRepo(AestheticDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<IEnumerable<string?>> Autocomplete(string searchByName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AestheticDto>> GetBreweriesAsync(string? search = null, string? sortBy = null, double? userLat = null, double? userLong = null)
        {
            throw new NotImplementedException();
        }
    }
}
