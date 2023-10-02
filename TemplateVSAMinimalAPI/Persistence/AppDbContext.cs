using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TemplateVSAMinimalAPI.Domain.Entities;
using TemplateVSAMinimalAPI.Domain.Mapping;

namespace TemplateVSAMinimalAPI.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProductMapping());
            builder.ApplyConfiguration(new CategoryMapping());
            base.OnModelCreating(builder);

        }

    }
}
