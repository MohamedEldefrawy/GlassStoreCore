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
        private readonly IService<WholeSaleProducts> _wholeSaleProductService;
        private readonly IService<WholeSaleSellingOrder> _wholeSaleSellingOrderService;
        private readonly IService<WholeSaleSellingOrderDetails> _wholeSaleSellingOrderDetailsService;

        public WholeSaleSellingOrdersController(ObjectMapper mapper, IPaginationUow paginationUow)
        {
            _mapper = mapper;
            _paginationUow = paginationUow;
            _wholeSaleProductService = _paginationUow.Service<WholeSaleProducts>();
            _wholeSaleSellingOrderService = _paginationUow.Service<WholeSaleSellingOrder>();
            _wholeSaleSellingOrderDetailsService = _paginationUow.Service<WholeSaleSellingOrderDetails>();
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
            var od = order.WholeSaleSellingOrderDetails;
            order.WholeSaleSellingOrderDetails = null;
            var result = _wholeSaleSellingOrderService.Add(order).Result;

            int odResult = 0;
            foreach (var orderDetail in od)
            {
                odResult += _wholeSaleSellingOrderDetailsService.Add(orderDetail).Result;

            }


            if (result <= 0 || odResult <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't create the order."
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Order has been created successfully."
            });
        }

        private WholeSaleSellingOrder MappingSellingOrder(WholeSaleSellingOrdersDto wholeSaleSellingOrdersDto)
        {
            WholeSaleSellingOrder order = new WholeSaleSellingOrder
            {
                Id = wholeSaleSellingOrdersDto.Id,
                OrderDate = wholeSaleSellingOrdersDto.OrderDate,
                UserId = wholeSaleSellingOrdersDto.UserId,

            };

            var orderDetails = new List<WholeSaleSellingOrderDetails>();

            foreach (var orderDetail in wholeSaleSellingOrdersDto.WholeSaleSellingOrderDetails)
            {
                var temp = new WholeSaleSellingOrderDetails();
                temp.Price = orderDetail.Price;
                temp.Quantity = orderDetail.Quantity;
                temp.WholeSaleProduct = _wholeSaleProductService.FindById(Guid.Parse(orderDetail.WholeSaleProductId)).Result;
                temp.WholeSaleSellingOrder = order;
                temp.WholeSaleProductId = temp.WholeSaleProduct.Id;
                temp.WholeSaleSellingOrderId = temp.WholeSaleSellingOrder.Id;
                orderDetails.Add(temp);
            }

            order.WholeSaleSellingOrderDetails = orderDetails;
            order.User = _paginationUow.Service<ApplicationUser>().FindById(order.UserId).Result;
            return order;
        }

        [HttpPut]
        public ActionResult<WholeSaleSellingOrder> UpdateWholeSaleSellingOrder(UpdateWholeSaleSellingOrderDto orderDto)
        {
            var selectedOd = _wholeSaleSellingOrderDetailsService.GetAll(o => o.WholeSaleSellingOrderId == orderDto.Id).Result;

            if (selectedOd == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected Order not found."
                });
            }

            selectedOd = new List<WholeSaleSellingOrderDetails>();

            foreach (var order in orderDto.WholeSaleSellingOrderDetails)
            {
                selectedOd.Add(new WholeSaleSellingOrderDetails
                {
                    Price = order.Price,
                    Quantity = order.Quantity,
                    WholeSaleProductId = Guid.Parse(order.WholeSaleProductId),
                    WholeSaleSellingOrderId = order.WholeSaleSellingOrderId,
                });
            }

            int result = 0;
            foreach (var orderDetail in selectedOd)
            {
                result += _wholeSaleSellingOrderDetailsService.UpdateAsync(orderDetail).Result;
            }

            if (result <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't update selected order."
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Selected order has been updated successfully."
            });
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
