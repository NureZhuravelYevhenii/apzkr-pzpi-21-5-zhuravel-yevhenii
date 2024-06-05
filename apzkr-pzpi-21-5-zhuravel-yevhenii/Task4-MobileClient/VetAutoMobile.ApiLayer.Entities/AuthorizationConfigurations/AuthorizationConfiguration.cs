using System.ComponentModel;

namespace VetAutoMobile.ApiLayer.Entities.AuthorizationConfigurations
{
    public class AuthorizationConfiguration : INotifyPropertyChanged
    {
        private string? _accessToken;
        public string? AccessToken 
        {
            get => _accessToken;
            set
            {
                _accessToken = value;
                PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(AccessToken)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
