﻿using System;
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
    public class WholeSaleProductsController : ControllerBase
    {
        private readonly ObjectMapper _mapper;
        private readonly IPaginationUow _paginationUow;

        public WholeSaleProductsController(ObjectMapper mapper, IPaginationUow uriService)
        {
            _mapper = mapper;
            _paginationUow = uriService;
        }

        [HttpGet]
        public ActionResult<WholeSaleProducts> GetWholeSaleProducts([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (products, totalRecords) =
                _paginationUow.Service<WholeSaleProducts>().GetAllAsync(filter.PageNumber, filter.PageSize).Result;

            if (products.Count == 0)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected Wholesale product not found."
                });
            }

            var productsDto = products.Select(_mapper.Mapper.Map<WholeSaleProducts, WholeSaleProductsDto>).ToList();
            var pageResponse = PaginationHelper.CreatePagedResponse(productsDto, filter, totalRecords, _paginationUow, route);
            pageResponse.Message = "request has been completed successfully";
            return Ok(pageResponse);
        }

        [HttpGet("{id}")]
        public ActionResult<WholeSaleProducts> GetWholeSaleProduct(string id)
        {
            Guid.TryParse(id, out var guid);

            var product = _paginationUow.Service<WholeSaleProducts>().FindByIdAsync(guid).Result;

            if (product == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "No products found."
                });
            }

            return Ok(_mapper.Mapper.Map<WholeSaleProducts, WholeSaleProductsDto>(product));
        }

        [HttpPost]
        public ActionResult<WholeSaleProducts> CreateWholeSaleProduct(WholeSaleProductsDto wholeSaleProductsDto)
        {
            var product = _mapper.Mapper.Map<WholeSaleProductsDto, WholeSaleProducts>(wholeSaleProductsDto);
            var result
            = _paginationUow.Service<WholeSaleProducts>().AddAsync(product);

            if (result.Result <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Faild to create product."
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Whole sale product has been created successfully."
            });
        }

        [HttpPut("{id}")]
        public ActionResult<WholeSaleProducts> UpdateWholeSaleProduct(WholeSaleProductsDto wholeSaleProductsDto, Guid id)
        {
            var selectedProduct = _mapper.Mapper.Map<WholeSaleProductsDto, WholeSaleProducts>(wholeSaleProductsDto);
            selectedProduct.Id = id;
            var result = _paginationUow.Service<WholeSaleProducts>().UpdateAsync(selectedProduct);

            if (result.Result <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't Update selected product."
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Selected product updated successfully."
            });
        }

        [HttpDelete("{id}")]
        public ActionResult<WholeSaleProducts> DeleteWholeSaleProduct(string id)
        {
            Guid.TryParse(id, out var guid);
            var wholeSaleProductService = _paginationUow.Service<WholeSaleProducts>();
            var selectedProduct = wholeSaleProductService.FindByIdAsync(guid).Result;
            if (selectedProduct == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected product not found."
                });
            }

            var result = wholeSaleProductService.DeleteAsync(selectedProduct);

            if (result.Result <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't Delete selected product."
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Selected product has been deleted succesfully."
            });
        }
    }
}
