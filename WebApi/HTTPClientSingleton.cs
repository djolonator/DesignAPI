using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;

namespace WebApi
{
    public class HttpClientSingleton
    {
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;

        public HttpClientSingleton(IOptions<AppSettings> appSettings)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _appSettings = appSettings.Value;

            var socketsHttpHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            };

            _httpClient = new HttpClient(socketsHttpHandler)
            {
                BaseAddress = new Uri(_appSettings.PrintfullBaseAddress!)
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _appSettings.PrintfullStoreToken);
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
        {
            return await _httpClient.PostAsync(endpoint, content);
        }
    }
}
