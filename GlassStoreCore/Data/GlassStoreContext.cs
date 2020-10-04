using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;

namespace GlassStoreCore.Data
{
    public class GlassStoreContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<WholeSaleProduct> WholeSaleProducts { get; set; }
        public DbSet<WholeSaleSellingOrder> WholeSaleSellingOrders { get; set; }
        public DbSet<WholeSaleSellingOrderDetail> WholeSaleSellingOrderDetails { get; set; }
        public GlassStoreContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}
