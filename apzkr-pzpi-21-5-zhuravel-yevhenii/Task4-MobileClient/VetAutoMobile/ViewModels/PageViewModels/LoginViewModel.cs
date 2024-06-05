using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.ApiLayer.Entities.AuthorizationConfigurations;
using VetAutoMobile.ViewModels.BaseViewModels;

namespace VetAutoMobile.ViewModels.PageViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthorizationApiService _authorizationApiService;

        [ObservableProperty]
        private string _login = "";
        [ObservableProperty]
        private string _password = "";
        [ObservableProperty]
        private ICollection<string> _errors = [];

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(IAuthorizationApiService authorizationApiService)
        {
            _authorizationApiService = authorizationApiService;

            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        private async Task LoginAsync()
        {
            Errors = [];
            if (string.IsNullOrEmpty(Login))
            {
                Errors.Add("Login is empty");
            }
            if (string.IsNullOrEmpty(Password))
            {
                Errors.Add("Password is empty");
            }

            if (Errors.Count == 0)
            {
                await _authorizationApiService.AuthorizeAsync(new LoginModel
                {
                    Login = Login,
                    Password = Password,
                });
            }
        }
    }
}
