namespace megamart_backend.DTOs
{
    public record SpecificationsDto(
        string? Model,
        string? Warranty,
        string? Delivery
    );

    public record SecondaryImageDto(
        string ImageUrl,
        string ImagePublicId
    );


    public record ProductCreateDto(
        string Name,
        decimal Price,
        int StockQuantity,
        string? ImageUrl,
        string? ImagePublicId,
        string Description,
        int CategoryId,
        List<SecondaryImageDto>? SecondaryImages = null, // optional JSON payload
        SpecificationsDto? Specifications = null //optional JSON payload
    );


    public record ProductUpdateDto(
        string Name,
        decimal Price,
        int StockQuantity,
        string? ImageUrl,
        string? ImagePublicId,
        string Description,
        int CategoryId,
        List<SecondaryImageDto>? SecondaryImages = null,
        SpecificationsDto? Specifications = null 
    );

    public record ProductResponseDto(
        int Id,
        string Name,
        decimal Price,
        int StockQuanity,
        string? ImageUrl,
        string Description,
        int CategoryId,
        string? CategoryName,
        List<SecondaryImageDto>? SecondaryImages = null,
        SpecificationsDto? Specifications = null
    );

}
