using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Veri.System.Services
{
    public class TextService : ITextService
    {
        readonly HttpClient http = new HttpClient();
        private readonly string apiKey;
        private readonly string serviceUri;

        public TextService(string apiKey, string serviceUri)
        {
            this.serviceUri = serviceUri;
            this.apiKey = apiKey;
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.apiKey}");
        }

        public async Task SendMessageAsync(string from, string to, string message)
        {
            var values = new Dictionary<string, string>
            {
                { "from", from },
                { "to", to },
                { "message", message },
                { "format", "json" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await http.PostAsync(serviceUri, content);
            _ = await response.Content.ReadAsStringAsync();
        }
    }
}
