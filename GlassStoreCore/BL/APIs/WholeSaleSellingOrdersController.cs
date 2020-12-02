using System.Linq;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs.WholeSaleProductsDtos;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Helpers;
using GlassStoreCore.Services.PaginationUowService;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WholeSaleSellingOrdersController : ControllerBase
    {
        private readonly ObjectMapper _mapper;
        private readonly IPaginationUow _paginationUow;

        public WholeSaleSellingOrdersController(ObjectMapper mapper, IPaginationUow paginationUow)
        {
            _mapper = mapper;
            _paginationUow = paginationUow;
        }

        [HttpGet]
        public ActionResult<WholeSaleSellingOrder> GetWholeSaleSellingOrders([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (orders, totalRecords) = _paginationUow.Service<WholeSaleSellingOrder>()
                                                       .GetAll(filter.PageNumber, filter.PageSize).Result;

            if (orders.Count == 0)
            {
                return NotFound("No Orders Available");
            }

            var ordersDto = orders.Select(_mapper.Mapper.Map<WholeSaleSellingOrder, WholeSaleSellingOrdersDto>)
                                  .ToList();
            var pageResponse =
                PaginationHelper.CreatePagedResponse(ordersDto, filter, totalRecords, _paginationUow, route);

            return Ok(pageResponse);
        }

        [HttpGet]
        public ActionResult<WholeSaleSellingOrder> GetWholeSaleSellingOrder(string id)
        {
            var selectedOrder = _paginationUow.Service<WholeSaleSellingOrder>().FindById(id).Result;
            if (selectedOrder == null)
            {
                return NotFound("please select a valid order id");
            }

            return Ok(_mapper.Mapper.Map<WholeSaleSellingOrder, WholeSaleSellingOrdersDto>(selectedOrder));
        }

        [HttpPost]
        public ActionResult<WholeSaleSellingOrder> CreateWholeSaleSellingOrder(
            WholeSaleSellingOrdersDto wholeSaleSellingOrdersDto)
        {
            var order = _mapper.Mapper.Map<WholeSaleSellingOrdersDto, WholeSaleSellingOrder>(wholeSaleSellingOrdersDto);
            var result = _paginationUow.Service<WholeSaleSellingOrder>().Add(order).Result;

            if (result <= 0)
            {
                return BadRequest("Something wrong");
            }

            return Ok();
        }

        [HttpPut]
        public ActionResult<WholeSaleSellingOrder> UpdateWholeSaleSellingOrder(WholeSaleSellingOrdersDto orderDto, int id)
        {
            var selectedOrder = _paginationUow.Service<WholeSaleSellingOrder>().FindById(id).Result;

            if (selectedOrder == null)
            {
                return NotFound("Please select valid order id");
            }

            _mapper.Mapper.Map(orderDto, selectedOrder);

            var result = _paginationUow.Service<WholeSaleSellingOrder>().UpdateAsync(selectedOrder).Result;
            if (result <= 0)
            {
                return BadRequest("Something wrong");
            }

            return Ok();
        }

        [HttpDelete]
        public ActionResult<WholeSaleSellingOrder> DeleteWholeSaleSellingOrder(string id)
        {
            var selectedOrder = _paginationUow.Service<WholeSaleSellingOrder>().FindById(id).Result;

            if (selectedOrder == null)
            {
                return NotFound("Please select a valid Id");
            }

            var result = _paginationUow.Service<WholeSaleSellingOrder>().DeleteAsync(selectedOrder).Result;

            if (result == 0)
            {
                return BadRequest("Something wrong");
            }

            return Ok();
        }
    }
}
