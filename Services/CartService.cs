using megamart_backend.DTOs;
using megamart_backend.Models;
using megamart_backend.Repositories;
using megamart_backend.Services.Interfaces;
using Microsoft.VisualBasic;

namespace megamart_backend.Services
{
    public class CartService: ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<CartResponseDto> GetCartByUserIdAsync(int userId)
        {
            var cart = await GetOrCreateCartInternalAsync(userId);
            return MapToDto(cart);
        }

        public async Task<CartResponseDto> AddToCartAsync(int userId,AddToCartDto dto)
        {
            if (dto.Quantity < 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            var cart = await GetOrCreateCartInternalAsync(userId);

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            await _unitOfWork.CompleteAsync();
            return MapToDto(cart);

        }

        public async Task<CartResponseDto> UpdateItemQuantityAsync(int userId,int cartItemId,UpdateCartItemDto dto)
        {
            var cart = await GetOrCreateCartInternalAsync(userId);
            var item = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (item == null)
            {
                throw new KeyNotFoundException("Cart item not found in user's cart.");
            }

            if (dto.Quantity <= 0)
            {
                _unitOfWork.CartItems.Delete(item);
            }
            else
            {
                item.Quantity = dto.Quantity;
            }

            await _unitOfWork.CompleteAsync();
            return MapToDto(cart);

        }

        public async Task<CartResponseDto> RemoveItemAsync(int userId,int cartItemId)
        {
            var cart = await GetOrCreateCartInternalAsync(userId);
            var item = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (item != null)
            {
                _unitOfWork.CartItems.Delete(item);
                await _unitOfWork.CompleteAsync();
            }

            return MapToDto(cart);

        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await GetOrCreateCartInternalAsync(userId);
            if (cart.CartItems.Any())
            {
                foreach (var item in cart.CartItems.ToList())
                {
                    _unitOfWork.CartItems.Delete(item);
                }

                await _unitOfWork.CompleteAsync();
            }
        }

        private async Task<Cart> GetOrCreateCartInternalAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _unitOfWork.Carts.AddCartAsync(cart);
                await _unitOfWork.CompleteAsync();

                // Re-fetch to ensure navigation properties/includes are fully loaded
                cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            }

            return cart ?? throw new InvalidOperationException("Failed to retrieve or create cart.");
        }

        private static CartResponseDto MapToDto(Cart cart)
        {
            var items = cart.CartItems?.Select(ci => new CartItemResponseDto(
                ci.Id,
                ci.ProductId,
                ci.Product?.Name ?? "Unknown Product",
                ci.Product?.Price ?? 0m,
                ci.Product?.ImageUrl,
                ci.Quantity
            )).ToList() ?? new List<CartItemResponseDto>();

            return new CartResponseDto(cart.Id, cart.UserId, items);
        }

    }
}
