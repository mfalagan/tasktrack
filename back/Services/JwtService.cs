using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace back.Services {
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateSecurityToken(int userId)
        {
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
                    // TODO: uncomment & add token table to db
                    // new Claim("TokenId", tokenId)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

            // var tokenHandler = new JwtSecurityTokenHandler();
            // var key = Encoding.ASCII.GetBytes(
            //     _configuration["Jwt:Key"] ??
            //     throw new InvalidOperationException("Jwt:Key is missing in appsettings.json")
            // );
            // var tokenDescriptor = new SecurityTokenDescriptor
            // {
            //     Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }),
            //     Expires = DateTime.UtcNow.AddDays(7),
            //     SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            //     Issuer = _configuration["Jwt:Issuer"],
            //     Audience = _configuration["Jwt:Audience"]
            // };

            // var token = tokenHandler.CreateToken(tokenDescriptor);
            // return tokenHandler.WriteToken(token);
        }
    }
}

