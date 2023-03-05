using Microsoft.IdentityModel.Tokens;
using portal.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace portal.UnitTest
{
    public class JWTProviderTests
    {
        [Fact]
        public void Generate_ReturnsJwtSecurityToken()
        {
            var issuer = "https://example.com";
            var audience = "https://example.com/api";
            var secret = "my-secret-key";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "John Doe"),
                new Claim(ClaimTypes.Email, "john.doe@example.com")
            };

            var provider = new JWTTokenProvider(issuer, audience, secret);

            var token = provider.Generate(claims);

            Assert.IsType<JwtSecurityToken>(token);
            Assert.Equal(issuer, token.Issuer);
            Assert.Equal(audience, token.Audiences.FirstOrDefault());
            Assert.NotEmpty(token.Claims);
        }
    }
}