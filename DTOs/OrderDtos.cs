namespace megamart_backend.DTOs
{
    public record OrderCreateDto(
        int UserId,
        List<OrderItemCreateDto> Items,
        string? DeliveryAddress = null, // Optional for Normal Orders
        string? PaymentMethod = null // Optional for Normal Orders
    );

    public record OrderResponseDto(
        int Id,
        int UserId,
        string Email,
        decimal totalAmount,
        string Status,
        DateTime CreatedAt,
        List<OrderItemResponseDto> Items,
        string? DeliveryAddress = null, //Optional for Normal Orders
        string? PaymentMethod = null // Optional for Normal Orders
    );

}
