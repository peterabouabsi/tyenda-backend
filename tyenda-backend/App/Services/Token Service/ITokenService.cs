using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace tyenda_backend.App.Services.Token_Service
{
    public interface ITokenService
    {
        public abstract string GenerateAccessToken(List<Claim> claims);
        public abstract string GenerateRefreshToken(List<Claim> claims);
        public abstract string GenerateRandomToken(List<Claim> claims, DateTime expiresIn);
        public abstract DateTime GetTokenExpiryDate(string token);
        public abstract string GetTokenClaim(string token, string claimKey);
        public abstract string GetHeaderTokenClaim(string claimKey);
    }
}