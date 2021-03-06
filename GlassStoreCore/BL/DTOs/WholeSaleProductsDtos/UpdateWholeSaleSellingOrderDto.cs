using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlassStoreCore.BL.DTOs.WholeSaleProductsDtos
{
    public class UpdateWholeSaleSellingOrderDto
    {
        public string Id { get; set; }

        public ICollection<WholeSaleProductsOrderDetailsDto> WholeSaleSellingOrderDetails { get; set; }

    }
}
