using VetAutoMobile.ApiLayer.Entities.AuthorizationConfigurations;

namespace VetAutoMobile.ApiLayer.Abstractions
{
    public interface IAuthorizationApiService
    {
        Task AuthorizeAsync(LoginModel loginModel);
        Task RegisterAsync(RegisterModel registerModel);
    }
}
