using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using BoilerplateProject.Authorization.Roles;
using BoilerplateProject.Authorization.Users;
using BoilerplateProject.MultiTenancy;
using BoilerplateProject.Entities.Orders;
using BoilerplateProject.Entities.Products;

namespace BoilerplateProject.EntityFrameworkCore
{
    public class BoilerplateProjectDbContext : AbpZeroDbContext<Tenant, Role, User, BoilerplateProjectDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany(o => o.Orders)
                .UsingEntity<OrderedProduct>();
                //.UsingEntity<OrderedProduct>(
                //l => l.HasOne<Product>().WithMany(e => e.OrderedProducts),
                //r => r.HasOne<Order>().WithMany(e => e.OrderedProducts));
        }
        public BoilerplateProjectDbContext(DbContextOptions<BoilerplateProjectDbContext> options)
            : base(options)
        {
        }
    }
}
