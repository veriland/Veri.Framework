using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Veri.System.Services
{
    public class TokenService : ITokenService
    {
        private readonly string secret;
        private readonly int expirationInMinutes;
        public TokenService(string secret, int expirationInMinutes = 5)
        {
            this.secret = secret;
            this.expirationInMinutes = expirationInMinutes;
        }
        public string CreateToken(string claim)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            RSACryptoServiceProvider keyService = new RSACryptoServiceProvider(2048);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, claim)
                }),
                IssuedAt = DateTime.Now,
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                //SigningCredentials = new SigningCredentials(new RsaSecurityKey(keyService.ExportParameters(true)), SecurityAlgorithms.RsaSsaPssSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
