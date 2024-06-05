using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;
using VetAutoIoT.Abstractions;
using VetAutoIoT.Core.Configurations;

namespace VetAutoIoT
{
    public class ConfigurationService : IConfigurationService
    {
        const int DefaultPort = 8888;

        public async Task<ApiConfiguration?> GetConfigurationAsync(CancellationToken cancellationToken = default)
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var buffer = new byte[1024];

            socket.Bind(new IPEndPoint(IPAddress.Any, DefaultPort));
            await socket.ReceiveFromAsync(buffer, new IPEndPoint(IPAddress.Loopback, DefaultPort), cancellationToken);

            var result = JsonConvert.DeserializeObject<ApiConfiguration>(Encoding.UTF8.GetString(buffer).Split("<EOM>").First());

#if DEBUG 
            if (result is not null)
            {
                result.BaseUrl = result.BaseUrl.Replace("10.0.2.2", "localhost");
            }
#endif

            return result;
        }
    }
}
