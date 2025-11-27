using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Exceptions;
using E_Commerce.Services.Specification.ProductSpecifications;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.CommonResponse;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    // Public Because I Will Take IT And Make Register In DI Container
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var Brands = await _unitOfWork.GenericRepository<ProductBrand,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductBrand>,IEnumerable<BrandDTO>>(Brands);
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductAsync(ProductQueryParams queryParams)
        {
            var Repo = _unitOfWork.GenericRepository<Product, int>();
            var Spec = new ProductTypeAndBrandSpecifications(queryParams);
            var Products = await Repo.GetAllAsync(Spec);
            var productWithCountSpec = new ProductWithCountSpecification(queryParams);
            var TotalCount = await Repo.CountAsync(productWithCountSpec);
            var DataToReturn =  _mapper.Map<IEnumerable<ProductDTO>>(Products);
            var CountOFReturnData = DataToReturn.Count();

            return new PaginatedResult<ProductDTO>(queryParams.PageIndex, CountOFReturnData ,TotalCount ,DataToReturn);
        }

        public  async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GenericRepository <ProductType,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductType>,IEnumerable<TypeDTO>>(Types);
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id)
        {
            var spec = new ProductTypeAndBrandSpecifications(id);
            var product =await _unitOfWork.GenericRepository<Product,int>().GetByIdAsync(spec);
            if (product is null)
                return Error.NotFound("Product Is NotFound", $"Product With Id {id} NotFound");
            return _mapper.Map<ProductDTO>(product);
        }
    }
}
