using back.Models.Transfer;
using back.Services;
using Microsoft.AspNetCore.Mvc;
using ArgumentException = back.Exceptions.ArgumentException;

namespace back.Controllers
{
    [Route("/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITaskDbService _dbService;
        private readonly IJwtService _jwtService;

        public AuthController(ITaskDbService dbService, IJwtService jwtService)
        {
            _dbService = dbService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] UserCredentials user)
        {

            

            // Just horrible validation as a placeholder
            if (user.Username == "user" && user.Password == "pass")
            {
                var token = _jwtService.GenerateSecurityToken(user.Email ?? "pepe");
                return Ok(new { token });
            }
            else
                return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserCredentials user)
        {
            try
            {
                await _dbService.RegisterUser(user);
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
    }
}


