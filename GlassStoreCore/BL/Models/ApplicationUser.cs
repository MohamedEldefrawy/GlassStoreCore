using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.BL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<WholeSaleSellingOrder> wholeSaleSellingOrders { get; set; }
    }
}
