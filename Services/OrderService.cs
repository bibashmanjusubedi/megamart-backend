using megamart_backend.DTOs;
using megamart_backend.Models;
using megamart_backend.Repositories;
using megamart_backend.Services.Interfaces;

namespace megamart_backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<OrderResponseDto> CreateOrderAsync(int userId, OrderCreateDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
            {
                throw new ArgumentException("An order must contain at least one item.");
            }


            // 1. Verify customer exists
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} does not exist.");
            }


            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = 0m,
                OrderItems = new List<OrderItem>()
            };


            var itemDtos = new List<OrderItemResponseDto>();
            decimal calculatedTotal = 0m;

            // 2. Validate items,decrement stock,snapshot unit price
            foreach (var itemDto in dto.Items)
            {
                if (itemDto.Quantity <= 0)
                {
                    throw new ArgumentException("Item quantity must be greater than zero");
                }

                var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);

                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {itemDto.ProductId} was not found.");

                }

                if (product.StockQuantity < itemDto.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Insufficient stock for '{product.Name}'. Available: {product.StockQuantity}, Requested: {itemDto.Quantity}.");
                }

                // Decrement inventory
                product.StockQuantity -= itemDto.Quantity;
                _unitOfWork.Products.Update(product);

                // Add to order items
                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price,
                };

                order.OrderItems.Add(orderItem);

                // Populate DTO item using the current product details
                itemDtos.Add(new OrderItemResponseDto(
                    ProductId: product.Id,
                    ProductName: product.Name,
                    Quantity: itemDto.Quantity,
                    UnitPrice: product.Price
                ));

                calculatedTotal += product.Price * itemDto.Quantity;

            }

            order.TotalAmount = calculatedTotal;


            // 3. Atomically persist order and product updates
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            return new OrderResponseDto(
                Id: order.Id,
                UserId: order.UserId,
                totalAmount: order.TotalAmount,
                Status: order.Status,
                CreatedAt: order.CreatedAt,
                Items: itemDtos
            );

        }



        public async Task<OrderResponseDto?> GetOrderByIdAsync(int orderId, int userId, string userRole)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }

            // Ensure non-admin users can only view their own orders
            if (userRole != "Admin" && order.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to view this order.");
            }


            return new OrderResponseDto(
                Id: order.Id,
                UserId: order.UserId,
                totalAmount: order.TotalAmount,
                Status: order.Status,
                CreatedAt: order.CreatedAt,
                Items: order.OrderItems?.Select(oi => new OrderItemResponseDto(
                    ProductId: oi.ProductId,
                    ProductName: oi.Product?.Name ?? string.Empty,
                    Quantity: oi.Quantity,
                    UnitPrice: oi.UnitPrice
            )).ToList() ?? new List<OrderItemResponseDto>() // <-- Added () after ToList
           );

        }

        //public async Task<IReadOnlyList<OrderResponseDto>> GetOrdersByUsersAsync(int userId)
        //{
        //    var orders = await _unitOfWork.Orders.FindAsync(o => o.UserId == userId);
        //    return orders.Select(order => new OrderResponseDto(
        //        Id: order.Id,
        //        UserId: order.UserId,
        //        totalAmount: order.TotalAmount,
        //        Status: order.Status,
        //        CreatedAt:order.CreatedAt,
        //        Items: order.OrderItems?.Select(oi => new OrderItemResponseDto(
        //            ProductId: oi.ProductId,
        //            ProductName: oi.Product?.Name ?? string.Empty,
        //            Quantity: oi.Quantity,
        //            UnitPrice: oi.UnitPrice
        //            )).ToList() ?? new List<OrderItemResponseDto>()
        //        )).ToList();
        //}


        public async Task<IReadOnlyList<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();

            return orders.Select(order => new OrderResponseDto(
                Id: order.Id,
                UserId: order.UserId,
                totalAmount: order.TotalAmount,
                Status: order.Status,
                CreatedAt:order.CreatedAt,
                Items: order.OrderItems?.Select(oi => new OrderItemResponseDto(
                    ProductId: oi.ProductId,
                    ProductName: oi.Product?.Name ?? string.Empty,
                    Quantity: oi.Quantity,
                    UnitPrice: oi.UnitPrice
                )).ToList() ?? new List<OrderItemResponseDto>()
            )).ToList();

        }


        //public async Task<OrderResponseDto?> GetOrderByIdAsync(int orderId,int userId,string userRole)
        //{
        //    var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

        //    if(order == null)
        //    {
        //        return null;
        //    }

        //    // Ensure non-admin users can only view their own orders
        //    if (userId !="Admin" && order.UserId != userId)
        //    {
        //        throw new UnauthorizedAccessException("You are not authorized to view this order.");
        //    }

        //    return new OrderResponseDto(
        //        Id: order.Id,
        //        UserId: order.UserId,
        //        totalAmount: order.TotalAmount,
        //        Status: order.Status,
        //        Items: order.OrderItems?.Select(oi => new OrderItemResponseDto(
        //            Product: oi.ProductId,
        //            ProductName: oi.Product?.Name ?? string.Empty,
        //            Quantity: oi.Quantity,
        //            UnitPrice: oi.UnitPrice
        //        )).ToList() ?? new List<OrderItemResponseDto>()
        //    );

        //}


        public async Task<IReadOnlyList<OrderResponseDto>> GetOrdersByUserIdAsync(int userId)
        {
            // Retrieve all orders matching the user's ID
            var orders = await _unitOfWork.Orders.FindAsync(o => o.UserId == userId);

            return orders.Select(order => new OrderResponseDto(
                 order.Id,
                 order.UserId,
                 order.TotalAmount,
                 order.Status,
                 order.CreatedAt,
                 order.OrderItems?.Select(oi => new OrderItemResponseDto(
                     oi.ProductId,
                     oi.Product?.Name ?? string.Empty,
                     oi.Quantity,
                     oi.UnitPrice
                 )).ToList() ?? new List<OrderItemResponseDto>()
            )).ToList();

        }


    }
}

