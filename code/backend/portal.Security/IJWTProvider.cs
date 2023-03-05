using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace portal.Security
{
    public interface IJWTProvider
    {
        public JwtSecurityToken Generate(List<Claim> claims);
    }
}