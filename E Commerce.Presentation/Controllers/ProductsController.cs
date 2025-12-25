using E_Commerce.Presentation.Attributes;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{

    public class ProductsController:ApiBaseController 
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        [Authorize]
        [RadisCache(10)]
        // GET:BaseUrl/api/Products
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _productService.GetAllProductAsync(queryParams);
            return Ok(products);
        }
            
        
        [HttpGet("{id}")]
        // GET:BaseUrl/api/Products/5
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            return HandelResult<ProductDTO>(result);
        }


        [HttpGet("brands")]
        // GET:BaseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var brands = await _productService.GetAllBrandsAsync();
            return Ok(brands);
        }


        [HttpGet("types")]
        // GET:BaseUrl/api/Products/types
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var types = await _productService.GetAllTypesAsync();
            return Ok(types);
        }


    }
}
