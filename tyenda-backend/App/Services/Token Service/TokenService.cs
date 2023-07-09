using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace tyenda_backend.App.Services.Token_Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateAccessToken(List<Claim> claims)
        {
            var key = _configuration["Jwt:key"];
            var encodedKey = Encoding.ASCII.GetBytes(key!);
            var authSigningKey = new SymmetricSecurityKey(encodedKey);
            
            
            var issuer = _configuration["Jwt:issuer"];
            var audience = _configuration["Jwt:audience"];
            
            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(3),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return jwtToken;
        }
        public string GenerateRefreshToken(List<Claim> claims)
        {
            var key = _configuration["Jwt:key"];
            var encodedKey = Encoding.ASCII.GetBytes(key!);
            var authSigningKey = new SymmetricSecurityKey(encodedKey);
            
            
            var issuer = _configuration["Jwt:issuer"];
            var audience = _configuration["Jwt:audience"];
            
            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(6),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return jwtToken;
        }
        public string GenerateRandomToken(List<Claim> claims, DateTime expiresIn)
        {
            var key = _configuration["Jwt:randomKey"];
            var encodedKey = Encoding.ASCII.GetBytes(key!);
            var authSigningKey = new SymmetricSecurityKey(encodedKey);
            
            
            var issuer = _configuration["Jwt:issuer"];
            var audience = _configuration["Jwt:audience"];
            
            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresIn,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return jwtToken;
        }
        public DateTime GetTokenExpiryDate(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.ValidTo;
        }
        public string GetTokenClaim(string token, string claimKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var claimValue = jwtToken.Claims.FirstOrDefault(claim => claim.Type == claimKey)!.Value;

            return claimValue;
        }
        public string GetHeaderTokenClaim(string claimKey)
        {
            var authHeader = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"];
            var token = authHeader.ToString().Split(" ")[1];
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var claimValue = jwtToken.Claims.FirstOrDefault(claim => claim.Type == claimKey)!.Value;

            return claimValue;
        }
    }
}