namespace elfBeauty.Core.Entities
{
    public static class Const
    {
        // Limits
        public const int AutocompleteLimit = 10;

        // Config Key
        public const string CacheKey = "AestheticCache",
                            BreweryDSKey = "BreweryDatasource";

        // Background Job
        public static readonly TimeSpan BrewerySyncInterval = TimeSpan.FromMinutes(AutocompleteLimit);

        // Sorting
        public const string SortByName = "name",
                            SortByCity = "city",
                            SortByDist = "distance";

        // Logging
        public const string SyncStartLog = "Starting background sync with Open Brewery API...",
                            SyncSuccessLog = "Successfully synced breweries from Open Brewery API.",
                            SyncFailureLog = "Error occurred while syncing brewery data.",
                            ErrorLog_Unexpected = "An unexpected error occurred. Please try again later.",
                            ErrorLog_Simple = "An error occurred",
                            FetchErr = "Error fetching breweries";

        // Error messages
        public const string InternalServerErrorMessage = "An unexpected error occurred.",
                            EmptySearchQueryMessage = "Search query cannot be empty.";

        // Content type
        public const string ContentType_Json = "application/json";

    }
}
