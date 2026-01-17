namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public record OrderItemsDTO
    {
        public string ProductName { get; init; }

        public string PictureUrl { get; init; }

        public decimal Price { get; init; }
        public int Quantity { get; init; }
    }
}