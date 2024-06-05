using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.ViewModels.BaseViewModels;

namespace VetAutoMobile.ViewModels.PageViewModels
{
    public partial class ConfigureDevicesViewModel : BaseViewModel
    {
        private readonly IConfigurationService _configurationService;

        [ObservableProperty]
        private bool _isConfiguring = false;

        public ICommand Configure { get; set; }

        public ConfigureDevicesViewModel(IConfigurationService configurationService)
        {
            Configure = new AsyncRelayCommand(ConfigureAsync);
            _configurationService = configurationService;
        }

        private async Task ConfigureAsync()
        {
            IsConfiguring = true;
            await _configurationService.ConfigureDevicesAsync();
            IsConfiguring = false;
        }
    }
}
