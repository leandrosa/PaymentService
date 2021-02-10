using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Payments.Presentation.Authentication
{
    public class JwtTokenManager
    {
        private readonly JwtTokenConfiguration _jwtTokenConfiguration;
        
        public JwtTokenManager(JwtTokenConfiguration jwtTokenConfiguration)
        {
            _jwtTokenConfiguration = jwtTokenConfiguration;
        }

        public JwtSecurityToken CreateJwtToken(
            string username,
            string userId,
            IEnumerable<string> userRoles,
            IEnumerable<Claim> userClaims)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", userId)
            }
            .Union(userClaims)
            .Union(userRoles.Select(x => new Claim("roles", x)));

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfiguration.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtTokenConfiguration.Issuer,
                audience: _jwtTokenConfiguration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtTokenConfiguration.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
