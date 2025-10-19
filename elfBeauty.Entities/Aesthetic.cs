using System.Text.Json.Serialization;

namespace elfBeauty.Core.Entities
{
    public class AestheticBase
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("city")]
        public string? City { get; set; }
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }
    }

    public class Aesthetic : AestheticBase
    {
        // Primary Key
        public int ID { get; set; }
    }

    public class AestheticDb : AestheticBase
    {
        [JsonPropertyName("id")]
        public string? ID { get; set; }
    }

}
