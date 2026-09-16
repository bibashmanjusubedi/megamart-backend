using megamart_backend.Models;

namespace megamart_backend.Repositories
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
        Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order?> GetOrderWithItemsAsync(int orderId);

    }
}
