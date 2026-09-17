using megamart_backend.Data;
using megamart_backend.Models;

namespace megamart_backend.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public IUserRepository Users { get; }
        public IOrderRepository Orders { get; }
        public IGenericRepository <OrderItem> OrderItems { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            // Repositories share the same exact same DbContext instance
            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
            Users = new UserRepository(_context);
            Orders = new OrderRepository(_context);
            OrderItems = new GenericRepository<OrderItem>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
