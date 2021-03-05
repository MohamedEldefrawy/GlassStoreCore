using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;

namespace GlassStoreCore.Services.WholeSaleProductsService
{
    public class WholeSaleProductsService : Service<WholeSaleProducts>, IWholeSaleProductsService
    {
        public WholeSaleProductsService(GlassStoreContext context) : base(context)
        {

        }
    }
}
