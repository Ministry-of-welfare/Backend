using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace server_pra.Services
{
    // שירות ליצירת JWT; תומך במפתח כ‑Base64 או כמחרוזת רגילה.
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string userId, string role, int validHours = 2)
        {
            var jwtSection = _config.GetSection("Jwt");
            if (!jwtSection.Exists())
            {
                jwtSection = _config.GetSection("Logging:Jwt"); // fallback
            }

            var keyString = jwtSection["Key"];
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];

            if (string.IsNullOrWhiteSpace(keyString))
                throw new InvalidOperationException("JWT key not configured (Jwt:Key or Logging:Jwt:Key).");

            // support Base64-encoded key or plain text key
            byte[] keyBytes;
            try
            {
                keyBytes = Convert.FromBase64String(keyString);
            }
            catch
            {
                keyBytes = Encoding.UTF8.GetBytes(keyString);
            }

            // HS256 requires >= 256 bits = 32 bytes
            if (keyBytes.Length < 32)
                throw new InvalidOperationException($"JWT key too short: {keyBytes.Length * 8} bits. HS256 requires at least 256 bits (32 bytes). Generate a longer key (e.g. 32 random bytes, Base64-encoded).");

            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId ?? string.Empty),
                new Claim(ClaimTypes.Role, role ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expires = DateTime.UtcNow.AddHours(validHours);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}