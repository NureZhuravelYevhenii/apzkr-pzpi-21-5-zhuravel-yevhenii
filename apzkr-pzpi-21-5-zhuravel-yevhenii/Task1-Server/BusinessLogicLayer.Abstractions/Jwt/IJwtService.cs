using BusinessLogicLayer.Entities.AnimalCenters;
using System.Security.Claims;

namespace BusinessLogicLayer.Abstractions.Jwt
{
    public interface IJwtService
    {
        Task<string> GenerateAccessTokenAsync(IEnumerable<Claim> claims, CancellationToken cancellationToken = default);
        Task<string> GenerateRefreshTokenAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Claim>> GetClaimsFromAccessTokenAsync(string accessToken, CancellationToken cancellationToken = default);
    }
}
