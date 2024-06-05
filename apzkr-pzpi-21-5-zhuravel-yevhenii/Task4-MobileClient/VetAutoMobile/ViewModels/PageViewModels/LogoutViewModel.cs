using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using VetAutoMobile.ApiLayer.Entities.AuthorizationConfigurations;
using VetAutoMobile.Constants;
using VetAutoMobile.ViewModels.BaseViewModels;

namespace VetAutoMobile.ViewModels.PageViewModels
{
    public class LogoutViewModel : BaseViewModel
    {
        private readonly AuthorizationConfiguration _authorizationConfiguration;

        public ICommand Logout { get; set; }

        public LogoutViewModel(AuthorizationConfiguration authorizationConfiguration)
        {
            Logout = new AsyncRelayCommand(LogoutAsync);
            _authorizationConfiguration = authorizationConfiguration;
        }

        public Task LogoutAsync()
        {
            Preferences.Remove(PreferenceConstants.AccessToken);
            _authorizationConfiguration.AccessToken = null;
            return Task.CompletedTask;
        }
    }
}
