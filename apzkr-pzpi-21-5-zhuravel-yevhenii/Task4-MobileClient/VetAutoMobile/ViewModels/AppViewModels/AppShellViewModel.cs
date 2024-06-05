using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using VetAutoMobile.ApiLayer.Entities.AuthorizationConfigurations;
using VetAutoMobile.Constants;
using VetAutoMobile.ViewModels.BaseViewModels;

namespace VetAutoMobile.ViewModels.AppViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private readonly AuthorizationConfiguration _authorizationConfiguration;
        [ObservableProperty]
        private bool _isLoggedIn;
        [ObservableProperty]
        private bool _isLoggedOut;

        public ICommand ChangeLocale { get; set; }

        public AppShellViewModel(AuthorizationConfiguration authorizationConfiguration)
        {
            _authorizationConfiguration = authorizationConfiguration;
            _authorizationConfiguration.PropertyChanged += TokenChanged;
            IsLoggedIn = _authorizationConfiguration.AccessToken is not null;
            IsLoggedOut = _authorizationConfiguration.AccessToken is null;

            ChangeLocale = new RelayCommand<string>(ChangeLocaleMethod);
        }

        private void TokenChanged(object? sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(AuthorizationConfiguration.AccessToken))
            {
                IsLoggedIn = _authorizationConfiguration.AccessToken is not null;
                IsLoggedOut = _authorizationConfiguration.AccessToken is null;
                Preferences.Set(PreferenceConstants.AccessToken, _authorizationConfiguration.AccessToken);
            }
        }

        private void ChangeLocaleMethod(string? locale)
        {
            if (locale is null)
            {
                return;
            }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);
        }
    }
}
