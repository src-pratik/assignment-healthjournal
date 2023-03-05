using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace portal.Security
{
    public class JWTTokenProvider : IJWTProvider
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secret;
        private readonly SymmetricSecurityKey _key;
        private readonly SigningCredentials _signingCredentials;

        public JWTTokenProvider(string issuer, string audience, string secret, int expiryMinutes = 3600)
        {
            _issuer = issuer;
            _audience = audience;
            _secret = secret;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            _signingCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        }

        public JwtSecurityToken Generate(List<Claim> claims)
        {
            var token = new JwtSecurityToken(
               issuer: _issuer,
               audience: _audience,
               expires: DateTime.Now.AddMinutes(3600),
               claims: claims,
               signingCredentials: _signingCredentials
               );

            return token;
        }
    }
}