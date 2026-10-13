using megamart_backend.DTOs;

namespace megamart_backend.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCartByUserIdAsync(int userId);
        Task<CartResponseDto> AddToCartAsync(int userId, AddToCartDto dto);
        Task<CartResponseDto> UpdateItemQuantityAsync(int userId, int cartItemId, UpdateCartItemDto dto);
        Task<CartResponseDto> RemoveItemAsync(int userId, int cartItemId);
        Task ClearCartAsync(int userId);
    }
}
