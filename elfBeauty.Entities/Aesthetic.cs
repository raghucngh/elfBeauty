namespace elfBeauty.Core.Entities
{
    public class AestheticBase
    {
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class Aesthetic : AestheticBase
    {
        // Primary Key
        public int Id { get; set; }
    }

    public class AestheticDb : AestheticBase
    {
        public string? Id { get; set; }
    }

}
