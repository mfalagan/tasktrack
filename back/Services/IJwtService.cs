using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace back.Services {
    public interface IJwtService
    {
        public string GenerateSecurityToken(string email);
    }
}

