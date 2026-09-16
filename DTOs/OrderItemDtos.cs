namespace megamart_backend.DTOs
{
    public record OrderItemCreateDto(
        int ProductId,
        int Quantity
    );

    public record OrderItemResponseDto(
        int ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice
    );
}
