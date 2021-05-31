using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using GlassStoreCore.BL.Models;

namespace GlassStoreCore.Data
{
    public class GlassStoreContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<WholeSaleProducts> WholeSaleProducts { get; set; }
        public DbSet<WholeSaleSellingOrder> WholeSaleSellingOrders { get; set; }
        public DbSet<WholeSaleSellingOrderDetails> WholeSaleSellingOrderDetails { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<ApplicationUserRole> UserRoles { get; set; }
        public GlassStoreContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WholeSaleProducts>().HasKey(p => new
            {
                p.Id
            });

            modelBuilder.Entity<WholeSaleProducts>()
                .Property(p => p.Price)
                .HasPrecision(9, 2);

            modelBuilder.Entity<WholeSaleSellingOrderDetails>()
                .Property(od => od.Price)
                .HasPrecision(9, 2);

            modelBuilder.Entity<WholeSaleSellingOrder>()
                .Property(o => o.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<WholeSaleSellingOrderDetails>()
                .HasKey(od =>
                            new
                            {
                                od.WholeSaleProductId,
                                od.WholeSaleSellingOrderId
                            });

            modelBuilder.Entity<WholeSaleSellingOrderDetails>()
                .HasOne(o => o.WholeSaleSellingOrder)
                .WithMany(od => od.WholeSaleSellingOrderDetails);

            modelBuilder.Entity<WholeSaleSellingOrderDetails>()
                .HasOne(p => p.WholeSaleProduct)
                .WithMany(od => od.WholeSaleSellingOrderDetails);

            modelBuilder.Entity<WholeSaleSellingOrderDetails>().HasKey(p => new
            {
                p.WholeSaleSellingOrderId,
                p.WholeSaleProductId
            });
        }
    }
}
