using System.IdentityModel.Tokens.Jwt;
using back.Models.Transfer;
using back.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers
{
    [Route("/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserCredentials providedUser)
        {
            try {
                var storedUser = await _authService.GetUser(providedUser);
                if (storedUser.Password == providedUser.Password)
                    return Ok(_jwtService.GenerateSecurityToken(storedUser.Id));
                else
                    return Unauthorized();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserCredentials user)
        {
            try
            {
                await _authService.RegisterUser(user);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var tokenIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "TokenId");
                if (tokenIdClaim == null)
                {
                    return BadRequest("Invalid token");
                }

                var tokenId = int.Parse(tokenIdClaim.Value);
                await _jwtService.InvalidateToken(tokenId);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }
    }
}


