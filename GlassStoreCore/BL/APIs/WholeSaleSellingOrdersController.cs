﻿using System;
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
            var (orders, totalRecords) = _wholeSaleSellingOrderService
                                                       .GetAll(filter.PageNumber, filter.PageSize);

            if (orders.Count == 0)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "No orders found.",
                    Success = false
                });
            }

            var ordersDto = orders.Select(_mapper.Mapper.Map<WholeSaleSellingOrder, WholeSaleSellingOrdersDto>)
                                  .ToList();

            var (orderDetailsDto, x) = _wholeSaleSellingOrderDetailsService.GetAll(filter.PageNumber, filter.PageSize);
            var od = orderDetailsDto.AsQueryable();
            foreach (var order in ordersDto)
            {

                order.WholeSaleSellingOrderDetails = od.Where(o => o.WholeSaleSellingOrderId == order.Id).AsEnumerable()
                    .Select(_mapper.Mapper.Map<WholeSaleSellingOrderDetails, WholeSaleProductsOrderDetailsDto>).ToList();
            }
            var pageResponse =
                PaginationHelper.CreatePagedResponse(ordersDto, filter, totalRecords, _paginationUow, route);

            pageResponse.Message = "Orders returned successfully.";

            return Ok(pageResponse);
        }

        [HttpGet]
        public ActionResult<WholeSaleSellingOrder> GetWholeSaleSellingOrder(DeleteWholeSaleSellingOrderDto orderDto)
        {
            var selectedOrder = _wholeSaleSellingOrderService.FindByIdWithRelatedEntites("WholeSaleSellingOrderDetails"
                , x => x.Id == orderDto.Id);

            if (selectedOrder == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected order not found.",
                    Success = false
                });
            }

            return Ok(_mapper.Mapper.Map<WholeSaleSellingOrder, WholeSaleSellingOrdersDto>(selectedOrder));
        }

        [HttpPost]
        public ActionResult<WholeSaleSellingOrder> CreateWholeSaleSellingOrder(
            CreateWholeSaleSellingOrdersDto wholeSaleSellingOrdersDto)
        {
            var order = MappingSellingOrder(wholeSaleSellingOrdersDto);
            var orderDetails = order.WholeSaleSellingOrderDetails;
            order.WholeSaleSellingOrderDetails = null;

            int orderResult = 0;
            int orderDetailsResult = 0;

            var (isUnitInStockAvailable, productName) = IsUnitInStockAvailable(orderDetails.ToList());

            if (!isUnitInStockAvailable)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = string.Format("The {0} is out of stock.", productName),
                    Success =false
                });
            }

            orderResult += _wholeSaleSellingOrderService.Add(order);


            foreach (var orderDetail in orderDetails)
            {

                var product = _wholeSaleProductService.FindById(orderDetail.WholeSaleProductId);
                product.UnitsInStock -= orderDetail.Quantity;
                _wholeSaleProductService.Update(product);
                orderDetail.WholeSaleSellingOrderId = order.Id;
                orderDetailsResult += _wholeSaleSellingOrderDetailsService.Add(orderDetail);
            }

            if (orderDetailsResult <= 0 || orderResult <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't create the order.",
                    Success = false
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Order has been created successfully.",
                Success =false
            });
        }

        private WholeSaleSellingOrder MappingSellingOrder(CreateWholeSaleSellingOrdersDto wholeSaleSellingOrdersDto)
        {
            WholeSaleSellingOrder order = new WholeSaleSellingOrder
            {
                OrderDate = wholeSaleSellingOrdersDto.OrderDate,
                UserId = wholeSaleSellingOrdersDto.UserId,
            };

            var orderDetails = new List<WholeSaleSellingOrderDetails>();

            foreach (var orderDetail in wholeSaleSellingOrdersDto.WholeSaleSellingOrderDetails)
            {
                var temp = new WholeSaleSellingOrderDetails();
                temp.Price = orderDetail.Price;
                temp.Quantity = orderDetail.Quantity;
                temp.WholeSaleProductId = Guid.Parse(orderDetail.WholeSaleProductId);
                temp.WholeSaleSellingOrderId = orderDetail.WholeSaleSellingOrderId;
                orderDetails.Add(temp);
            }

            order.WholeSaleSellingOrderDetails = orderDetails;
            order.User = _paginationUow.Service<ApplicationUser>().FindById(order.UserId);
            _paginationUow.Complete();
            return order;
        }

        private (bool, string) IsUnitInStockAvailable(List<WholeSaleSellingOrderDetails> od)
        {
            foreach (var orderDetail in od)
            {
                var product = _wholeSaleProductService.FindById(orderDetail.WholeSaleProductId);

                if (product.UnitsInStock < orderDetail.Quantity)
                {
                    return (false, product.Name);
                }
            }

            return (true, "");
        }

        [HttpPut]
        public ActionResult<WholeSaleSellingOrder> UpdateWholeSaleSellingOrder(UpdateWholeSaleSellingOrderDto orderDto)
        {
            var selectedOd = _wholeSaleSellingOrderDetailsService.GetAll(o => o.WholeSaleSellingOrderId == orderDto.Id);

            if (selectedOd == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected Order not found.",
                    Success =false
                });
            }

            var newOd = new List<WholeSaleSellingOrderDetails>();

            for (int i = 0; i < selectedOd.Count; i++)
            {
                newOd.Add(new WholeSaleSellingOrderDetails
                {
                    Price = orderDto.WholeSaleSellingOrderDetails.ToList()[i].Price,
                    Quantity = orderDto.WholeSaleSellingOrderDetails.ToList()[i].Quantity,
                    WholeSaleProductId = Guid.Parse(orderDto.WholeSaleSellingOrderDetails.ToList()[i].WholeSaleProductId),
                    WholeSaleSellingOrderId = orderDto.WholeSaleSellingOrderDetails.ToList()[i].WholeSaleSellingOrderId,
                });

                var products = _wholeSaleProductService.FindById(Guid.Parse(orderDto.WholeSaleSellingOrderDetails.ToList()[i].WholeSaleProductId));
                products.UnitsInStock += selectedOd[i].Quantity;
                _wholeSaleProductService.Update(products);

            }

            int result = 0;
            foreach (var orderDetail in newOd)
            {
                result += _wholeSaleSellingOrderDetailsService.Update(orderDetail);

                var products = _wholeSaleProductService.FindById(orderDetail.WholeSaleProductId);
                products.UnitsInStock -= orderDetail.Quantity;
                _wholeSaleProductService.Update(products);
            }

            if (result <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't update selected order.",
                    Success =false
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Selected order has been updated successfully.",
                Success = true
            });
        }

        [HttpDelete]
        public ActionResult<WholeSaleSellingOrder> DeleteWholeSaleSellingOrder(DeleteWholeSaleSellingOrderDto orderDto)
        {
            var selectedOrder = _wholeSaleSellingOrderService.FindByIdWithRelatedEntites("WholeSaleSellingOrderDetails"
                , x => x.Id == orderDto.Id);

            if (selectedOrder == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected order not found.",
                    Success =false
                });
            }

            ReturnProductsToStock(selectedOrder.WholeSaleSellingOrderDetails);

            selectedOrder.WholeSaleSellingOrderDetails.Clear();


            if (selectedOrder.WholeSaleSellingOrderDetails.Count != 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Faild to delete order details.",
                    Success =false
                });
            }

            var result = _wholeSaleSellingOrderService.Delete(selectedOrder);

            if (result == 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't delete selected order.",
                    Success = false
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Selected order has been deleted successfully.",
                Success = true
            });
        }

        private void ReturnProductsToStock(ICollection<WholeSaleSellingOrderDetails> orderDetails)
        {
            foreach (var orderDetail in orderDetails)
            {
                var product = _wholeSaleProductService.FindById(orderDetail.WholeSaleProductId);
                product.UnitsInStock += orderDetail.Quantity;
            }
        }

        [HttpDelete]
        public ActionResult<WholeSaleSellingOrderDetails> DeleteWholeSaleSillingOrderDetail(DeleteWholeSaleSellingOrderDetail orderDetail)
        {
            var selectedOd = _wholeSaleSellingOrderDetailsService.GetAll(p => p.WholeSaleProductId == orderDetail.WholeSaleProductId && p.WholeSaleSellingOrderId == orderDetail.WholeSaleSellingOrderId);

            if (selectedOd == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected order detail not found.",
                    Success = false
                });
            }

            ReturnProductsToStock(selectedOd);

            var result = _wholeSaleSellingOrderDetailsService.Delete(selectedOd.SingleOrDefault());

            if (result <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Faild to delete selected order detail.",
                    Success = false
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Selected order detail has been deleted successfully.",
                Success = true
            });
        }
    }
}
