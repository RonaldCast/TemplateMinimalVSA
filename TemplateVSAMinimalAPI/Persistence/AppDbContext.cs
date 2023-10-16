using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection.Emit;
using TemplateVSAMinimalAPI.Domain.Entities;
using TemplateVSAMinimalAPI.Domain.Mapping;

namespace TemplateVSAMinimalAPI.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        private IDbContextTransaction? _currentTransaction;

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProductMapping());
            builder.ApplyConfiguration(new CategoryMapping());
            base.OnModelCreating(builder);

        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction is not null)
            {
                return;
            }
            _currentTransaction = await Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction is null)
            {
                return;
            }

            await _currentTransaction.CommitAsync();

            _currentTransaction.Dispose();
            _currentTransaction = null;
        }

        public async Task RollbackTransaction()
        {
            if (_currentTransaction is null)
            {
                return;
            }

            await _currentTransaction.RollbackAsync();

            _currentTransaction.Dispose();
            _currentTransaction = null;
        }

    }
}
