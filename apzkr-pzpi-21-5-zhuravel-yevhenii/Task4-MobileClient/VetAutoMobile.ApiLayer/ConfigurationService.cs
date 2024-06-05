using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.ApiLayer.Entities;
using VetAutoMobile.ApiLayer.Entities.AuthorizationConfigurations;
using VetAutoMobile.ApiLayer.Entities.EndpointConfigurations;
using VetAutoMobile.ApiLayer.Entities.EndpointConfigurations.EntityEndpointConfigurations;
using VetAutoMobile.ApiLayer.Entities.HttpClientConfigurations;

namespace VetAutoMobile.ApiLayer
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly DeviceConfiguration _deviceConfiguration;
        private readonly HttpClientConfiguration _httpClientConfiguration;
        private readonly AuthorizationConfiguration _authorizationConfiguration;
        private FeederEndpointConfiguration _feederEndpointConfiguration;

        public ConfigurationService(
            IOptions<DeviceConfiguration> deviceConfigurationOptions, 
            IOptions<HttpClientConfiguration> httpClientConfigurationOptions,
            IOptions<FeederEndpointConfiguration> feederEndpointConfigurationOptions,
            AuthorizationConfiguration authorizationConfiguration)
        {
            _deviceConfiguration = deviceConfigurationOptions.Value ?? throw new ArgumentNullException(nameof(deviceConfigurationOptions));
            _httpClientConfiguration = httpClientConfigurationOptions.Value ?? throw new ArgumentNullException(nameof(httpClientConfigurationOptions));
            _feederEndpointConfiguration = feederEndpointConfigurationOptions.Value ?? throw new ArgumentNullException(nameof(feederEndpointConfigurationOptions));
            _authorizationConfiguration = authorizationConfiguration;
        }
        public Task ConfigureDevicesAsync()
        {
            return Task.Run(() =>
            {
                var thread = new Thread(SendConfiguration);
                thread.Start();
                thread.Join();
            });
        }

        private void SendConfiguration()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

            socket.SendTo(Encoding.UTF8.GetBytes(GetConfigurationString()), new IPEndPoint(

#if DEBUG
                IPAddress.Parse("10.0.2.2"),
#else
                IPAddress.Broadcast,
#endif

                _deviceConfiguration.Port));
        }

        private string GetConfigurationString() =>
            JsonConvert.SerializeObject(
                new DeviceEndpointConfiguration
                {
                    AccessToken = _authorizationConfiguration.AccessToken!,
                    AnimalFeederEndpoint = _feederEndpointConfiguration.CreateAnimalFeeder,
                    BaseUrl = _httpClientConfiguration.BaseUrl,
                    FeederByCoordinatesEndpoint = _feederEndpointConfiguration.GetByCoordinates,
                    FeederEndpoint = _feederEndpointConfiguration.CreateEndpoint
                }
            ) + "<EOM>";
    }
}
