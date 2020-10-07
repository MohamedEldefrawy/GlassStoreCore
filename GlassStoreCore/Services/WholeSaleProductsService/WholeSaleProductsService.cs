using System;
using GlassStoreCore.BL;
using GlassStoreCore.BL.DTOs.WholeSaleProductsDtos;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services.WholeSaleProductsService
{
    public class WholeSaleProductsService : Service<WholeSaleProduct>, IWholeSaleProductsService
    {
        private readonly GlassStoreContext _context;
        private readonly ObjectMapper _mapper = new ObjectMapper();
        public WholeSaleProductsService(GlassStoreContext context) : base(context)
        {
            _context = context;
        }

        public void Update(WholeSaleProductsDto wholeSaleProductsDto, Guid id)
        {
            //var wholeSaleProduct = _context.WholeSaleProducts.FindAsync(id).Result;
            //wholeSaleProduct.Name = wholeSaleProductsDto.Name;
            //wholeSaleProduct.Price = wholeSaleProduct.Price;
            //wholeSaleProduct.UnitsInStock = wholeSaleProduct.UnitsInStock;

            var wholeSaleProduct = _mapper.Mapper.Map<WholeSaleProductsDto, WholeSaleProduct>(wholeSaleProductsDto);
            wholeSaleProduct.Id = id;
            _context.Entry(wholeSaleProduct).State = EntityState.Modified;
        }
    }
}
