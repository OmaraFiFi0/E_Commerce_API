using E_Commerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specification.OrderSpecifications
{
    internal class OrderWithPaymentItentIdSpecifications:BaseSpecification<Order,Guid>
    {
        public OrderWithPaymentItentIdSpecifications(string paymentIntentId):base(X=>X.PaymentIntentId==paymentIntentId)
        {
            
        }
    }
}
