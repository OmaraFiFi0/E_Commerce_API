using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specification.ProductSpecifications
{
    public class ProductWithCountSpecification:BaseSpecification<Product,int>
    {
        public ProductWithCountSpecification(ProductQueryParams queryParams) 
  : base(ProductSpecificationHelper.GetCriteria(queryParams))

        {

        }
    }
}
