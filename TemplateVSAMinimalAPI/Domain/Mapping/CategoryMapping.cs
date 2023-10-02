using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateVSAMinimalAPI.Domain.Entities;

namespace TemplateVSAMinimalAPI.Domain.Mapping
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category { Id = Guid.NewGuid(), Name = "Skincare", Description = "Lorem ipsum" },
                new Category { Id = Guid.NewGuid(), Name = "Jewelry", Description = "Lorem ipsum" },
                new Category { Id = Guid.NewGuid(), Name = "Home textiles", Description = "Lorem ipsum" }    
            );
        }
    }
}
