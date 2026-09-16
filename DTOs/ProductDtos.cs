namespace megamart_backend.DTOs
{
    public record ProductCreateDto(
        string Name,
        decimal Price,
        int StockQuantity,
        string? ImageUrl,
        string? ImagePublicId,
        string Description,
        int CategoryId
    );

    public record ProductResponseDto(
        int Id,
        string Name,
        decimal Price,
        int StockQuanity,
        string? ImageUrl,
        string Description,
        int CategoryId,
        string? CategoryName
    );

}
