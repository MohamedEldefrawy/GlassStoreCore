using System.Linq;
using GlassStoreCore.BL.Models;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs.WholeSaleProductsDtos;
using GlassStoreCore.Helpers;
using GlassStoreCore.Services.PaginationUowService;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class WholeSaleSellingIOrderDetailsController : ControllerBase
    {
        private readonly ObjectMapper _mapper;
        private readonly IPaginationUow _paginationUow;

        public WholeSaleSellingIOrderDetailsController(ObjectMapper mapper, IPaginationUow paginationUow)
        {
            _mapper = mapper;
            _paginationUow = paginationUow;
        }

        [HttpGet]
        public ActionResult<WholeSaleSellingOrderDetails> GetWholeSaleSellingOrderDetails([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (orderDetails, totalRecords) = _paginationUow.Service<WholeSaleSellingOrderDetails>()
                                                       .GetAll(filter.PageNumber, filter.PageSize).Result;

            if (orderDetails.Count == 0)
            {
                return NotFound("No OrdersDetails Available");
            }

            var odrderDetialsDto = orderDetails
                                   .Select(_mapper.Mapper
                                                  .Map<WholeSaleSellingOrderDetails, WholeSaleProductsOrderDetailsDto>)
                                   .ToList();
            var pageResponse =
                PaginationHelper.CreatePagedResponse(odrderDetialsDto, filter, totalRecords, _paginationUow, route);
            return Ok(pageResponse);
        }

    }
}
