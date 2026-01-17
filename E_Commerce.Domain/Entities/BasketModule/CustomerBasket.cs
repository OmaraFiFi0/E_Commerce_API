using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.BasketModule
{
    public class CustomerBasket
    {
        public string  Id { get; set; } = default!; // Generated From Front End [Guid]

        public int? DeliveryMethodId { get; set; }

        public decimal ShippingPrice { get; set; }

        public string? PaymentIntentID { get; set; }

        public string? ClientSecret { get; set; }

        public ICollection<BasketItems> Items { get; set; } = []; 
    }
}
