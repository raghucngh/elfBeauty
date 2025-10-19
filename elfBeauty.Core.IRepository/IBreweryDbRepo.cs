using elfBeauty.App.Dto;

namespace elfBeauty.Core.IRepository
{
    public interface IBreweryDbRepo
    {
        Task<IEnumerable<string?>> Autocomplete(string searchByName);
        Task<IEnumerable<AestheticDto>> GetBreweriesAsync(string? search = null, string? sortBy = null,
                                                          double? userLat = null, double? userLong = null);
    }
}
