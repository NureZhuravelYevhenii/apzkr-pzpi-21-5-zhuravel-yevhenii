using AutoMapper;
using BusinessLogicLayer.Abstractions;
using BusinessLogicLayer.Abstractions.Jwt;
using BusinessLogicLayer.Entities.AnimalCenters;
using BusinessLogicLayer.Entities.Pagination;
using BusinessLogicLayer.Services.BaseServices;
using Core.Abstractions.Services.Security;
using Core.Constants;
using Core.EntityHelpers;
using Core.Localizations;
using Core.Services.Security;
using DataAccessLayer.Abstractions;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;

namespace BusinessLogicLayer.Services
{
    public class AnimalCenterService : BaseCrudService<AnimalCenter, AnimalCenterIdDto, AnimalCenterDto, AnimalCenterCreationDto, AnimalCenterUpdateDto, KeyAttribute>, IAnimalCenterService
    {
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IStringLocalizer<Resource> _stringLocalizer;

        public AnimalCenterService(
            IJwtService jwtService,
            IPasswordHasher passwordHasher,
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IStringLocalizer<Resource> stringLocalizer
            ) : base(mapper, unitOfWork, stringLocalizer)
        {
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<AnimalCenterToken> LoginAsync(string login, string password, CancellationToken cancellationToken = default)
        {
            var repository = await GetRepositoryAsync(cancellationToken);

            var animalCenters = await repository
                .ReadByPredicateAsync(ac => ac.Name == login, int.MaxValue, 0, cancellationToken);
            if (!animalCenters.Any())
            {
                throw new ArgumentException(string.Format(_stringLocalizer["There is no animal center with {0} name."].Value, login));
            }

            var animalCenter = animalCenters.First();
            if(!_passwordHasher.VerifyPassword(password, animalCenter.PasswordHash))
            {
                throw new ArgumentException(string.Format(_stringLocalizer["Wrong password."].Value));
            }

            var tokens = await GetAnimalCenterTokenAsync(animalCenter, cancellationToken);

            await UpdateAnimalCenterRefreshTokenAsync(animalCenter, tokens, repository, cancellationToken);

            return tokens;
        }

        public async Task<AnimalCenterToken> LoginAsync(AnimalCenterToken tokens, CancellationToken cancellationToken = default)
        {
            var repository = await GetRepositoryAsync(cancellationToken);

            var claims = await _jwtService.GetClaimsFromAccessTokenAsync(tokens.AccessToken, cancellationToken);
            var idStr = claims.FirstOrDefault(c => c.Type == AnimalCenterConstants.Id)?.Value
                ?? throw new ArgumentException(string.Format(_stringLocalizer["There is no id claim"].Value));
            var id = Guid.Parse(idStr);

            var animalCenter = await repository
                .ReadByIdAsync(GeneralIdExtractor<AnimalCenterIdDto, KeyAttribute>.GetId(new AnimalCenterIdDto(id)), cancellationToken);

            if (animalCenter is null)
            {
                throw new ArgumentException(string.Format(_stringLocalizer["There is no animal center with {0} id."].Value, id));
            }

            if (animalCenter.RefreshToken != tokens.RefreshToken)
            {
                throw new ArgumentException(string.Format(_stringLocalizer["Invalid refresher token."].Value));
            }

            var newTokens = await GetAnimalCenterTokenAsync(animalCenter, cancellationToken);

            await UpdateAnimalCenterRefreshTokenAsync(animalCenter, newTokens, repository, cancellationToken);

            return newTokens;
        }

        public async Task<AnimalCenterToken> RegisterAsync(AnimalCenterCreationDto animalCenter, CancellationToken cancellationToken = default)
        {
            var repository = await GetRepositoryAsync(cancellationToken);

            var animalCentersWithSameName = await repository.ReadByPredicateAsync(ac => ac.Name == animalCenter.Name, int.MaxValue, 0, cancellationToken);

            if (animalCentersWithSameName.Any())
            {
                throw new ArgumentException(string.Format(_stringLocalizer["There is already animal center with {0} name."].Value, animalCenter.Name));
            }

            animalCenter.Password = _passwordHasher.HashPassword(animalCenter.Password);

            var newAnimalCenter = await repository.CreateAsync(_mapper.Map<AnimalCenter>(animalCenter), cancellationToken);

            var tokens = await GetAnimalCenterTokenAsync(newAnimalCenter, cancellationToken);

            await UpdateAnimalCenterRefreshTokenAsync(newAnimalCenter, tokens, repository, cancellationToken);

            return tokens;
        }

        private async Task UpdateAnimalCenterRefreshTokenAsync(
            AnimalCenter animalCenter,
            AnimalCenterToken tokens,
            IRepository<AnimalCenter, Expression<Func<AnimalCenter, bool>>> repository,
            CancellationToken cancellationToken = default)
        {
            animalCenter.RefreshToken = tokens.RefreshToken;

            await repository.UpdateAsync(animalCenter, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task<AnimalCenterToken> GetAnimalCenterTokenAsync(AnimalCenter animalCenter, CancellationToken cancellationToken) 
        {
            var claims = GetAnimalCenterClaims(animalCenter);

            return new AnimalCenterToken
            {
                AccessToken = await _jwtService.GenerateAccessTokenAsync(claims, cancellationToken),
                RefreshToken = await _jwtService.GenerateRefreshTokenAsync(cancellationToken),
            }; 
        }

        private IEnumerable<Claim> GetAnimalCenterClaims(AnimalCenter animalCenter) => new List<Claim>
        {
            new Claim(AnimalCenterConstants.Id, animalCenter.Id.ToString()),
        };
    }
}
