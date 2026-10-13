namespace megamart_backend.DTOs
{
    public record AddToCartDto
    (
        int ProductId,
        int Quantity
    );

    public record UpdateCartItemDto
    (
        int Quantity
    );

    public record CartItemResponseDto
    (
        int Id,
        int ProductId,
        string ProductName,
        decimal UnitPrice,
        string? ImageUrl,
        int Quantity
    )
    {
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}