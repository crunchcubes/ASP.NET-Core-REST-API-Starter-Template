using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restful.Core.Entities.Milk;

namespace Restful.Infrastructure.Database.EntityConfigurations.Milk
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(20);
            builder.Property(x => x.UnitPrice).HasColumnType("decimal(10, 2)");
        }
    }
}
