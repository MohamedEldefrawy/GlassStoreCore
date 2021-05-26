using System;
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
                _paginationUow.Service<WholeSaleProducts>().GetAll(filter.PageNumber, filter.PageSize);

            if (products.Count == 0)
            {
                return NotFound(new JsonResults
                {
                    StatusMessage = "Selected Wholesale product not found.",
                    Success = false
                });
            }

            var productsDto = products.Select(_mapper.Mapper.Map<WholeSaleProducts, WholeSaleProductsDto>).ToList();
            var pageResponse = PaginationHelper.CreatePagedResponse(productsDto, filter, totalRecords, _paginationUow, route);
            pageResponse.Message = "request has been completed successfully";
            pageResponse.Succeeded = true;
            return Ok(pageResponse);
        }

        [HttpGet("{id}")]
        public ActionResult<WholeSaleProducts> GetWholeSaleProduct(string id)
        {
            Guid.TryParse(id, out var guid);

            var product = _paginationUow.Service<WholeSaleProducts>().FindById(guid);

            if (product == null)
            {
                return NotFound(new JsonResults
                {
                    StatusMessage = "No products found.",
                    Success = false
                });
            }

            return Ok(new JsonResults
            {
                StatusMessage = "Product Has been selected successfully.",
                Success = true,
                Data = _mapper.Mapper.Map<WholeSaleProducts, WholeSaleProductsDto>(product)
            });
        }

        [HttpGet]
        public ActionResult<WholeSaleProducts> GetWholeSaleProductsBySerial([FromQuery] string serial)
        {
            var products = _paginationUow.Service<WholeSaleProducts>().GetAll(p => p.SerialNumber.Contains(serial)).ToList();

            if (products == null)
            {
                return NotFound(new JsonResults
                {
                    StatusMessage = "No products found.",
                    Success = false
                });
            }

            return Ok(new JsonResults
            {
                StatusMessage = "Products has been found successfully.",
                Success = true,
                Data = products.Select(_mapper.Mapper.Map<WholeSaleProducts, WholeSaleProductsDto>)
            });

        }

        [HttpPost]
        public ActionResult<WholeSaleProducts> CreateWholeSaleProduct(WholeSaleProductsDto wholeSaleProductsDto)
        {
            var product = _mapper.Mapper.Map<WholeSaleProductsDto, WholeSaleProducts>(wholeSaleProductsDto);
            var result
            = _paginationUow.Service<WholeSaleProducts>().Add(product);

            if (result <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusMessage = "Faild to create product.",
                    Success = false
                });
            }

            return Ok(new JsonResults
            {
                StatusMessage = "Whole sale product has been created successfully.",
                Success = true
            });
        }

        [HttpPut("{id}")]
        public ActionResult<WholeSaleProducts> UpdateWholeSaleProduct(WholeSaleProductsDto wholeSaleProductsDto, Guid id)
        {
            var selectedProduct = _mapper.Mapper.Map<WholeSaleProductsDto, WholeSaleProducts>(wholeSaleProductsDto);
            selectedProduct.Id = id;
            var result = _paginationUow.Service<WholeSaleProducts>().Update(selectedProduct);

            if (result <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusMessage = "Couldn't Update selected product.",
                    Success = false
                });
            }

            return Ok(new JsonResults
            {
                StatusMessage = "Selected product updated successfully.",
                Success = true
            });
        }

        [HttpDelete("{id}")]
        public ActionResult<WholeSaleProducts> DeleteWholeSaleProduct(string id)
        {
            Guid.TryParse(id, out var guid);
            var wholeSaleProductService = _paginationUow.Service<WholeSaleProducts>();
            var selectedProduct = wholeSaleProductService.FindById(guid);
            if (selectedProduct == null)
            {
                return NotFound(new JsonResults
                {
                    StatusMessage = "Selected product not found.",
                    Success = false
                });
            }

            var result = wholeSaleProductService.Delete(selectedProduct);

            if (result <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusMessage = "Couldn't Delete selected product.",
                    Success = false
                });
            }

            return Ok(new JsonResults
            {
                StatusMessage = "Selected product has been deleted succesfully.",
                Success = true
            });
        }

    }
}
