using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using megamart_backend.DTOs;
using megamart_backend.Services.Interfaces;

namespace megamart_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Helper method
        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                                ?? User.FindFirst("sub")?.Value
                                ?? User.FindFirst("id")?.Value;

            if (int.TryParse(userIdClaim,out var userId))
            {
                return userId;
            }

            return null;
        }

        // POST: api/orders
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "Invalid user identification token." });
            }

            try
            {
                var order = await _orderService.CreateOrderAsync(userId.Value, dto);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }

            catch(KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        // GET: api/orders/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(OrderResponseDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

            try
            {
                var order = await _orderService.GetOrderByIdAsync(id, userId.Value, userRole);
                if (order == null)
                {
                    return NotFound(new { message = $"Order with ID {id} was not found." });
                }

                return Ok(order);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // GET: api/orders/my-orders
        [HttpGet("my-orders")]
        [ProducesResponseType(typeof(IReadOnlyList<OrderResponseDto>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var orders = await _orderService.GetOrdersByUserIdAsync(userId.Value);
            return Ok(orders);
        }

        // GET: api/orders (Admin only)
        [Authorize(Roles ="Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<OrderResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }


    }
}
