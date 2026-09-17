using megamart_backend.Models;

namespace megamart_backend.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        IOrderRepository Orders { get; }
        IGenericRepository<OrderItem> OrderItems { get; }

        Task<int> CompleteAsync();
    }
}
