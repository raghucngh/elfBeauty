using elfBeauty.App.Dto;

namespace elfBeauty.App.Interfaces
{
    public interface IAestheticSvc
    {
        Task<IEnumerable<AestheticDto>> GetAestheticsAsync_Cache(string? search = null, string? sortBy = null, double? userLat = null, double? userLong = null);
        Task<IEnumerable<string?>> Autocomplete_Cache(string searchByName);

        Task<IEnumerable<AestheticDto>> GetAestheticsAsync_DB(string? search = null, string? sortBy = null, double? userLat = null, double? userLong = null);
        Task<IEnumerable<string?>> Autocomplete_DB(string searchByName);
    }
}