using megamart_backend.DTOs;
using Npgsql.PostgresTypes;

namespace megamart_backend.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(int userId, OrderCreateDto dto);
        Task<OrderResponseDto?> GetOrderByIdAsync(int orderId, int userId, string userRole);
        Task<IReadOnlyList<OrderResponseDto>> GetOrdersByUserIdAsync(int userId);
        Task<IReadOnlyList<OrderResponseDto>> GetAllOrdersAsync();// For Admin Access
    }
}
