using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class CreateWholeSaleSellingOrdersDto
    {
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public ICollection<WholeSaleProductsOrderDetailsDto> WholeSaleSellingOrderDetails { get; set; }
    }
}
