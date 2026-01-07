using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public class OrderDataToReturnDTO
    {
        public Guid Id { get; init; }
        public string UserEmail { get; init; }

        public ICollection<OrderItemsDTO> Items { get; init; }

        public AddressDTO Address { get; init; }

        public string DeliveryMethod { get; init; }

        public string Status { get; init; }

        public DateTimeOffset OrderDate { get; init; }

        public decimal SubTotal { get; init; }
        public decimal Total { get; init; }
    }
}
