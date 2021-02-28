using System;
using System.Collections.Generic;
using System.Linq;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs.WholeSaleProductsDtos;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Helpers;
using GlassStoreCore.Services;
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
        private readonly IService<WholeSaleProduct> _wholeSaleProductService;

        public WholeSaleSellingOrdersController(ObjectMapper mapper, IPaginationUow paginationUow)
        {
            _mapper = mapper;
            _paginationUow = paginationUow;
            _wholeSaleProductService = _paginationUow.Service<WholeSaleProduct>();
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

            var order = MappingSellingOrder(wholeSaleSellingOrdersDto);

            var result = _paginationUow.Service<WholeSaleSellingOrder>().Add(order).Result;

            if (result <= 0)
            {
                return BadRequest("Something wrong");
            }

            return Ok();
        }

        private WholeSaleSellingOrder MappingSellingOrder(WholeSaleSellingOrdersDto wholeSaleSellingOrdersDto)
        {
            WholeSaleSellingOrder order = new WholeSaleSellingOrder
            {
                Id = wholeSaleSellingOrdersDto.Id,
                OrderDate = wholeSaleSellingOrdersDto.OrderDate,
                UserId = wholeSaleSellingOrdersDto.UserId,

            };

            var orderDetails = new List<WholeSaleSellingOrderDetail>();

            foreach (var orderDetail in wholeSaleSellingOrdersDto.WholeSaleSellingOrderDetails)
            {
                var temp = new WholeSaleSellingOrderDetail();
                temp.Id = orderDetail.Id;
                temp.Price = orderDetail.Price;
                temp.Quantity = orderDetail.Quantity;
                temp.WholeSaleProduct = _wholeSaleProductService.FindById(Guid.Parse(orderDetail.WholeSaleProductId)).Result;
                temp.WholeSaleSellingOrder = order;

                orderDetails.Add(temp);
                _paginationUow.Complete();

            }

            order.WholeSaleSellingOrderDetails = orderDetails;
            return order;
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
