using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using megamart_backend.DTOs;
using megamart_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace megamart_backend.Controllers
{
    //[AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register (Public)
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.RegisterAsync(dto);
                return Ok(response);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/auth/login (Public)
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.LoginAsync(dto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }


        // PUT: api/auth/users/{id}/role (Admin Only)
        [Authorize(Roles = "Admin")]
        [HttpPut("users/{id:int}/role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateUserRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Role))
            {
                return BadRequest(new { message = "Role is required" });
            }

            // Extract current calling admin ID from the token
            var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier)
                             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);


            if (!int.TryParse(claimValue, out var currentAdminId))
            {
                return Unauthorized(new { message = "Invalid token user identifier." });
            }

            try
            {
                var updated = await _authService.UpdateUserRoleAsync(id, dto.Role, currentAdminId);
                if(!updated)
                {
                    return NotFound(new { message = $"User with ID {id} was not found." });
                }

                return Ok(new { message = $"User ID {id} role successfully updated to {dto.Role}." });
            }
            catch(ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


    }
}
