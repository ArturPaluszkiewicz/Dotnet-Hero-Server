using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HeroApi.Entities
{
    public class HeroContext : DbContext
    {
        public HeroContext(DbContextOptions<HeroContext> options)
            : base(options)
        {
        }

        public DbSet<Hero> Heroes { get; set; } = null!;
    }
}