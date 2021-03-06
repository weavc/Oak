using System.Threading.Tasks;
using ContactMe.Models;
using Microsoft.Extensions.Options;
using Oak.ContactMe.Models;
using Oak.Events;
using Oak.Webhooks;
using Oak.Webhooks.Clients;

namespace ContactMe.Webhooks
{
    public class DiscordWebhook : Webhook<DiscordWebhookRequest>, IAsyncEventHandler<ContactMeEvent>, IWebhook<DiscordWebhookRequest>
    {
        private readonly HookOptions _options;

        public DiscordWebhook(
            IWebhookClientFactory webhookClientFactory,
            IOptions<HookOptions> options) 
            : base(webhookClientFactory)
        {
            this._options = options.Value;
        }

        public override string Url { get; set; }

        public override  string Type => WebhookTypes.PostJson;

        public async Task HandleEventAsync(ContactMeEvent args)
        {
            foreach(var url in this._options.Discord)
            {
                this.Url = url;
                await base.Send(new DiscordWebhookRequest(args));
            }
        }
    }
}
