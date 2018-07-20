using Microsoft.EntityFrameworkCore;
using Restful.Core.Entities;
using Restful.Core.Entities.Milk;
using Restful.Core.Entities.RestDemo;
using Restful.Infrastructure.Database.EntityConfigurations;
using Restful.Infrastructure.Database.EntityConfigurations.Milk;
using Restful.Infrastructure.Database.EntityConfigurations.RestDemo;

namespace Restful.Infrastructure.Database
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
