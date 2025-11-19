using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specification.ProductSpecifications
{
    internal class ProductTypeAndBrandSpecifications:BaseSpecification<Product,int>
    {
        public ProductTypeAndBrandSpecifications(ProductQueryParams queryParams)
            :base(ProductSpecificationHelper.GetCriteria(queryParams))
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

            switch (queryParams.sort)
            {
                case (ProductSortingOptions.NameAsc):
                    AddOrderBy(P=>P.Name);
                    break;
                case (ProductSortingOptions.NameDesc):
                    AddOrderDesc(P => P.Name);
                    break;
                case (ProductSortingOptions.PriceAsc):
                    AddOrderBy(P => P.Price);
                    break;
                case (ProductSortingOptions.PriceDesc):
                    AddOrderDesc(P => P.Price);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }
            
            ApplyPaginated(queryParams.PageSize, queryParams.PageIndex);
        }
        public ProductTypeAndBrandSpecifications(int id):base(X=>X.Id == id) 
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
    }
}
