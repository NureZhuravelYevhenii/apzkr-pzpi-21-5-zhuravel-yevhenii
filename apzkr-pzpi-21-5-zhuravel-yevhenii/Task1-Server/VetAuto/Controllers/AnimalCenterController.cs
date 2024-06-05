using BusinessLogicLayer.Abstractions;
using BusinessLogicLayer.Entities.AnimalCenters;
using Core.Constants;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using VetAuto.Models;

namespace VetAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalCenterController : BaseController<AnimalCenter, AnimalCenterDto, AnimalCenterIdDto, AnimalCenterCreationDto, AnimalCenterUpdateDto, BaseFilter>
    {
        private readonly IAnimalCenterService _animalCenterService;

        public AnimalCenterController(IAnimalCenterService animalCenterService) : base(animalCenterService)
        {
            _animalCenterService = animalCenterService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(AnimalCenterCreationDto animalCenterCreationDto, CancellationToken cancellationToken)
        {
            var tokens = await _animalCenterService.RegisterAsync(animalCenterCreationDto, cancellationToken);

            AddRefreshToken(tokens.RefreshToken);

            return Ok(tokens);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var tokens = await _animalCenterService.LoginAsync(loginDto.Login, loginDto.Password, cancellationToken);

            AddRefreshToken(tokens.RefreshToken);

            return Ok(tokens);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(AnimalCenterToken tokens, CancellationToken cancellationToken)
        {
            tokens.RefreshToken = Request.Cookies.First(c => c.Key == JwtConstants.RefreshToken).Value;

            var newTokens = await _animalCenterService.LoginAsync(tokens, cancellationToken);

            AddRefreshToken(newTokens.RefreshToken);

            return Ok(newTokens);
        }

        private void AddRefreshToken(string refreshToken)
        {
            Response.Cookies.Append(JwtConstants.RefreshToken, refreshToken, new CookieOptions
            {
                HttpOnly = true,
            });
        }
    }
}
