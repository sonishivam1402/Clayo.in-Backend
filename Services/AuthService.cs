using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.Models;
using e_commerce_backend.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace e_commerce_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IAuthRepository authRepo, IOptions<JwtSettings> jwtOptions)
        {
            _authRepo = authRepo;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _authRepo.ValidateUserAsync(email, password);

                if (user == null || !user.isSuccess)
                {
                    return new AuthResponse
                    {
                        isSuccess = false,
                        Message = user?.Message ?? "Invalid credentials"
                    };
                }

                // Safe Claims creation (null-safe)
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.email ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? string.Empty),
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("RoleId", user.roleId.ToString())
                };

                var accessToken = GenerateJwtToken(claims);
                var refreshToken = GenerateRefreshToken();

                await _authRepo.SaveRefreshTokenAsync(
                    user.UserId,
                    refreshToken,
                    DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpiryInMinutes)
                );

                return new AuthResponse
                {
                    isSuccess = true,
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    UserId = user.UserId,
                    UserName = user.UserName ?? string.Empty,
                    email = user.email ?? string.Empty,
                    roleId = user.roleId,
                    profileImage = user.profileImage,
                    cartId = user.cartId,
                    Message = "Login Successful"
                };
            }
            catch (Exception ex)
            {
                // Log ex here (optional)
                return new AuthResponse
                {
                    isSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }


        public async Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
                return new AuthResponse { isSuccess = false, Message = "Invalid access token" };

            var userId = Guid.Parse(principal.Claims.First(c => c.Type == "UserId").Value);

            var isValid = await _authRepo.ValidateRefreshTokenAsync(userId, refreshToken);
            if (!isValid)
                return new AuthResponse { isSuccess = false, Message = "Invalid refresh token" };

            var claims = principal.Claims.ToArray();
            var newAccessToken = GenerateJwtToken(claims);
            var newRefreshToken = GenerateRefreshToken();

            await _authRepo.RevokeAndSaveNewRefreshTokenAsync(userId, refreshToken, newRefreshToken, DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpiryInMinutes));

            return new AuthResponse
            {
                isSuccess = true,
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                UserId = userId,
                UserName = principal.Claims.First(c => c.Type == JwtRegisteredClaimNames.Name).Value
            };
        }

        public async Task LogoutAsync(Guid userId, string refreshToken)
        {
            await _authRepo.RevokeRefreshTokenAsync(userId, refreshToken);
        }

        private string GenerateJwtToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, // Ignore expiration
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                return securityToken is JwtSecurityToken jwtSecurityToken ? principal : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
