using E_Commerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specification.OrderSpecifications
{
    public class OrderSpecification:BaseSpecification<Order,Guid>
    {
        public OrderSpecification(string email):base(O=>O.UserEmail==email)
        {
            AddInclude(X => X.DeliveryMethod);
            AddInclude(X => X.Items);
            AddOrderDesc(X => X.OrderDate);
        }
        public OrderSpecification(Guid Id , string email):base(X=>X.UserEmail==email && X.Id==Id)
        {
            AddInclude(X => X.DeliveryMethod);
            AddInclude(X => X.Items);
        }
    }
}
