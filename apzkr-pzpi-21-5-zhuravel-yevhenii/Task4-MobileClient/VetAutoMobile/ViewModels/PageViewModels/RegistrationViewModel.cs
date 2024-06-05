using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.ApiLayer.Entities.AuthorizationConfigurations;
using VetAutoMobile.ViewModels.BaseViewModels;

namespace VetAutoMobile.ViewModels.PageViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _name = string.Empty;
        [ObservableProperty]
        private string _password = string.Empty;
        [ObservableProperty]
        private string _confirmPassword = string.Empty;
        [ObservableProperty]
        private string _address = string.Empty;
        [ObservableProperty]
        private string _info = string.Empty;
        [ObservableProperty]
        private ICollection<string> _errors = [];
        private readonly IAuthorizationApiService _authorizationApiService;

        public ICommand Register { get; set; }

        public RegistrationViewModel(IAuthorizationApiService authorizationApiService)
        {
            Register = new AsyncRelayCommand(RegisterAsync);
            _authorizationApiService = authorizationApiService;
        }

        private async Task RegisterAsync()
        {
            Errors = [];
            if (string.IsNullOrEmpty(Name))
            {
                Errors.Add("Name is empty");
            }
            if (string.IsNullOrEmpty(Password))
            {
                Errors.Add("Password is empty");
            }
            if (Password != ConfirmPassword)
            {
                Errors.Add("Confirm password is not same as password");
            }
            if (string.IsNullOrEmpty(Address))
            {
                Errors.Add("Address is empty");
            }

            if (Errors.Count == 0)
            {
                await _authorizationApiService.RegisterAsync(new RegisterModel
                {
                    Name = Name,
                    Password = Password,
                    Address = Address,
                    Info = Info
                });
            }
        }
    }
}
