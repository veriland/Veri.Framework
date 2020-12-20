using RestSharp;
using RestSharp.Authenticators;
using System;

namespace Veri.System.Services
{
    public class MailApiService : IMailService
    {
        private readonly string uri, apikey, domain, sender;

        public MailApiService(string uri, string apikey, string domain, string sender)
        {
            this.uri = uri;
            this.apikey = apikey;
            this.domain = domain;
            this.sender = sender;
        }

        public string SendMessage(string to, string subject, string body)
        {             
            RestClient client = new RestClient
            {
                BaseUrl = new Uri(uri),
                Authenticator =
                new HttpBasicAuthenticator("api", apikey)
            };
            RestRequest request = new RestRequest();
            request.AddParameter("domain", domain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", sender);
            request.AddParameter("to", to);
            request.AddParameter("subject", subject);
            request.AddParameter("html", body);
            request.Method = Method.POST;
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return $"Ok:{response.StatusCode}:{response.StatusDescription}";
            }
            else
            {
                return $"Err:{response.StatusCode}:{response.StatusDescription}:{response.ErrorMessage}";
            }
        }
    }
}
