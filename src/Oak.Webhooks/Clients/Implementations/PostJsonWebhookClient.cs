using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Oak.Events;
using Newtonsoft.Json;
using System.Text;

namespace Oak.Webhooks.Clients.Implementations
{
    public class PostJsonWebhookClient : WebhookClientBase, IWebhookClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<PostJsonWebhookClient> _logger;

        public PostJsonWebhookClient(
            HttpClient client, 
            ILogger<PostJsonWebhookClient> logger = null,
            IEventDispatcher eventDispatcher = null) 
            : base(eventDispatcher)
        {
            this._client = client;
            this._logger = logger;
        }

        public override string Type => WebhookTypes.PostJson;

        protected override async Task<Result> PostData<T>(string url, T data)
        {
            var json = JsonConvert.SerializeObject(data);
            var response = await this._client.PostAsync($"{url}", new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                this._logger?.LogError($"Response from {url}: ({(int)response.StatusCode}) {error}");
                return new Result(success: false, message: error);
            }

            return new Result(success: true);
        }
    }
}