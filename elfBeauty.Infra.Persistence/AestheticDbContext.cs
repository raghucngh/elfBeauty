using elfBeauty.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace elfBeauty.Infra.Persistence
{
    public class AestheticDbContext : DbContext
    {
        public AestheticDbContext(DbContextOptions<AestheticDbContext> options) : base(options) { }

        public DbSet<Aesthetic> Aesthetics { get; set; }

    }
}
