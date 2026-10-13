using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using megamart_backend.DTOs;
using megamart_backend.Services.Interfaces;
using System.Security.Claims;

namespace megamart_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Helper to extract the logged-in user's ID from JWT Claims
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            // 👇 Fixed: Correct syntax for int.TryParse with out parameter
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("User Id not found in token");
            }

            return userId;
        }

        [HttpGet]
        public async Task<ActionResult<CartResponseDto>> GetCart()
        {
            try
            {
                // In a production setup, you extract the UserId from JWT claims. 
                // For testing purposes or if you pass it manually, adjust as needed.
                int userId = GetUserId();
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartResponseDto>> AddToCart([FromBody] AddToCartDto dto)
        {
            try
            {
                int userId = GetUserId();
                var cart = await _cartService.AddToCartAsync(userId, dto);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut("items/{cartItemId}")]
        public async Task<ActionResult<CartResponseDto>> UpdateItemQuantity(int cartItemId, [FromBody] UpdateCartItemDto dto)
        {
            try
            {
                int userId = GetUserId();
                var cart = await _cartService.UpdateItemQuantityAsync(userId, cartItemId, dto);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("items/{cartItemId}")]
        public async Task<ActionResult<CartResponseDto>> RemoveItem(int cartItemId)
        {
            try
            { 
                int userId = GetUserId();
                var cart = await _cartService.RemoveItemAsync(userId, cartItemId);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                int userId = GetUserId();
                await _cartService.ClearCartAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
