namespace megamart_backend.DTOs
{
    public record OrderCreateDto(
        int UserId,
        List<OrderItemCreateDto> Items
    );

    public record OrderResponseDto(
        int Id,
        int UserId,
        decimal totalAmount,
        string Status,
        DateTime CreatedAt,
        List<OrderItemResponseDto> Items
    );

}
