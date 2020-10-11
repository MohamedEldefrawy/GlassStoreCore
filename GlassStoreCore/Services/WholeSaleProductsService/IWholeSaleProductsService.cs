using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs.WholeSaleProductsDtos;
using GlassStoreCore.BL.Models;

namespace GlassStoreCore.Services.WholeSaleProductsService
{
    public interface IWholeSaleProductsService : IService<WholeSaleProduct>
    {
        public void Update(WholeSaleProductsDto wholeSaleProductsDto, Guid id);
        public void Delete(Guid id);

        public WholeSaleProductsDto Get(Guid id);

    }
}
