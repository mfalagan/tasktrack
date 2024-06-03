using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using back.Models.Internal;
using System.Text;
using back.Models;
using Task = System.Threading.Tasks.Task;
using Microsoft.EntityFrameworkCore;

namespace back.Services
{
    public interface IJwtService
    {
        public string GenerateSecurityToken(int userId);
        public Task InvalidateToken(int tokenId);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public JwtService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string GenerateSecurityToken(int userId)
        {
            var user = _context.Users.Find(userId) 
                       ?? throw new InvalidOperationException("User not found");

            var token = new Token
            {
                Value = "",
                Expiration = DateTime.UtcNow.AddHours(1),
                User = user,
                UserId = userId,
                IsValid = true
            };

            _context.Tokens.Add(token);
            _context.SaveChanges();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(
                _configuration["Jwt:Key"] ??
                throw new InvalidOperationException("Jwt:Key is missing in appsettings.json")
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim("TokenId", token.Id.ToString())
                }),
                Expires = token.Expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            var jwtTokenString = tokenHandler.WriteToken(jwtToken);

            token.Value = jwtTokenString;
            _context.Tokens.Update(token);
            _context.SaveChanges();

            return jwtTokenString;
        }

        public IDictionary<string, string> DecodeJwtToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var token = tokenHandler.ReadJwtToken(jwtToken);

            var claims = new Dictionary<string, string>();
            foreach (var claim in token.Claims)
            {
                claims[claim.Type] = claim.Value;
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            return claims;
        }

        public async Task InvalidateToken(int tokenId)
        {
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Id == tokenId);
            if (token != null)
            {
                token.IsValid = false;
                _context.Tokens.Update(token);
                await _context.SaveChangesAsync();
            }
        }
    }
}
