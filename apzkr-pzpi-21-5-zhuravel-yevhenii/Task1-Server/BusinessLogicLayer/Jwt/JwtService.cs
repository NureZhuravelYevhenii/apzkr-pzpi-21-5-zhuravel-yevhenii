using BusinessLogicLayer.Abstractions.Jwt;
using BusinessLogicLayer.Entities.AnimalCenters;
using Core.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogicLayer.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfiguration _jwtConfig;

        public JwtService(IOptions<JwtConfiguration> jwtConfigOptions)
        {
            _jwtConfig = jwtConfigOptions.Value ?? throw new ArgumentNullException(nameof(jwtConfigOptions));
        }

        public Task<string> GenerateAccessTokenAsync(IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpireInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public Task<string> GenerateRefreshTokenAsync(CancellationToken cancellationToken = default)
        {
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            var refreshTokenByteArr = new byte[_jwtConfig.RefreshTokenSize];
            randomNumberGenerator.GetNonZeroBytes(refreshTokenByteArr);
            return Task.FromResult(Convert.ToBase64String(refreshTokenByteArr));
        }

        public Task<IEnumerable<Claim>> GetClaimsFromAccessTokenAsync(string accessToken, CancellationToken cancellationToken = default)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(accessToken);

            return Task.FromResult(token.Claims);
        }
    }
}
