using Microsoft.IdentityModel.Tokens;
using StudentShared.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Student_API_Project.Services.Auth
{
    // Generates JWT tokens for authenticated users
    public sealed class JwtTokenGenerator : IJwtTokenGenerator
    {
        // Access to configuration (reads JwtSettings from appsettings.json)
        private readonly IConfiguration _cfg;
        public JwtTokenGenerator(IConfiguration cfg) => _cfg = cfg;

        public string Generate(UserDto user)
        {
            // ===================== PAYLOAD (Claims) =====================
            // Build the JWT payload: user data that goes inside the token body.
            // These become the token's "payload" (Base64Url-encoded JSON).
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // subject (user id)
                new Claim(JwtRegisteredClaimNames.Email, user.Email),        // email
                new Claim(ClaimTypes.Role, user.Role),                       // role
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // unique token id
            };

            // ===================== HEADER (alg, typ) =====================
            // Create the signing key/credentials. This *implicitly* defines the JWT header:
            // { "alg": "HS256", "typ": "JWT" }
            // "alg" comes from the chosen SecurityAlgorithms (HmacSha256),
            // "typ" is set to "JWT" by the JwtSecurityToken.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Build the token object (this sets standard fields and ties header+payload together)
            var token = new JwtSecurityToken(
                issuer: _cfg["JwtSettings:Issuer"],     // "iss" in payload
                audience: _cfg["JwtSettings:Audience"], // "aud" in payload
                claims: claims,                          // payload claims
                notBefore: DateTime.UtcNow,              // "nbf"
                expires: DateTime.UtcNow.AddMinutes(60), // "exp"
                signingCredentials: creds                // defines header.alg and the key
            );

            // ========== ENCODING & SIGNATURE (Header.Payload.Signature) ==========
            // WriteToken() does the 3-step process:
            // 1) Serialize header & payload to JSON, then Base64URL-encode each part.
            // 2) Compute the signature using HMAC SHA-256 over: "{header}.{payload}" with our secret key.
            // 3) Concatenate as: "<Base64UrlHeader>.<Base64UrlPayload>.<Base64UrlSignature>".
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

