using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs.WholeSaleProductsDtos;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data.UnitOfWork;
using GlassStoreCore.Helpers;
using GlassStoreCore.Services.UriService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WholeSaleProductsController : ControllerBase
    {
        private readonly ObjectMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IUnitOfWork _unitOfWork;

        public WholeSaleProductsController(ObjectMapper mapper, IUriService uriService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _uriService = uriService;
        }

        [HttpGet]
        public ActionResult<WholeSaleProduct> GetWholeSaleProducts([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (products, totalRecords) =
                _unitOfWork.WholeSaleProductsService.GetAll(filter.PageNumber, filter.PageSize).Result;

            if (products == null)
            {
                return NotFound("No Products Available");
            }

            var productsDto = products.Select(_mapper.Mapper.Map<WholeSaleProduct, WholeSaleProductsDto>).ToList();
            var pageResponse = PaginationHelper.CreatePagedResponse(productsDto, filter, totalRecords, _uriService, route);
            return Ok(pageResponse);
        }

        [HttpGet("id")]
        public ActionResult<WholeSaleProduct> GetWholeSaleProduct(string id)
        {
            GuidConverter converter = new GuidConverter();
            var product = _unitOfWork.WholeSaleProductsService.Get(id).Result;
            if (product == null)
            {
                return NotFound("Please select a valid id");
            }
            return Ok(_mapper.Mapper.Map<WholeSaleProduct, WholeSaleProductsDto>(product));
        }
    }
}
