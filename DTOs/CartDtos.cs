namespace megamart_backend.DTOs
{
    public record CartResponseDto(
        int Id,
        int UserId,
        List<CartItemResponseDto> Items
    )
    {
        public decimal GrandTotal => Items.Sum(item => item.TotalPrice);
    }
}
