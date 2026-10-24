using Microsoft.EntityFrameworkCore;
using megamart_backend.Data;
using megamart_backend.Models;

namespace megamart_backend.Repositories
{
    public class OrderRepository:GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context):base (context)
        {

        }

        // Override GetAllAsync to include User and OrderItems (with Product details)
        public override async Task<IReadOnlyList<Order>> GetAllAsync()
        {
            return await _context.Orders
                            .Include(o => o.User)
                            .Include(o => o.OrderItems)
                            .ThenInclude(oi => oi.Product)
                            .AsNoTracking()
                            .ToListAsync();

        }


        // Override GetByIdAsync to include User, OrderItems, and Products
        public override async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                            .Include(o => o.User)
                            .Include(o => o.OrderItems)
                            .ThenInclude(oi => oi.Product)
                            .FirstOrDefaultAsync(o => o.Id == id);

        }


        public async Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId)
                                        .Include(o => o.UserId)
                                        .Include(o => o.OrderItems)
                                        .ThenInclude(oi => oi.Product)
                                    .OrderByDescending(o => o.CreatedAt)
                                    .AsNoTracking()
                                    .ToListAsync();
        }


        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
        {
            return await _context.Orders
                            .Include(o => o.User)
                            .Include(o => o.OrderItems)
                            .ThenInclude(oi => oi.Product)
                        .FirstOrDefaultAsync(o => o.Id == orderId);
        }


    }
}
