using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FhirConn.Utility
{
    public class HttpBearerTokenHandler : HttpClientHandler
    {
        private string lastToken;
        private DateTime expiredTime;        
        private string secret;
        private readonly IHttpMessageHandlerCallback callback;
        private string server;

        public HttpBearerTokenHandler(IHttpMessageHandlerCallback callback, string server, string secret)
        {
            this.callback = callback;
            this.server = server;
            this.secret = secret;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (server != null)
            {
                var token = GetToken();
                request.Headers.Add("Authorization", "Bearer " + token);
            }

            var guid = Guid.NewGuid().ToString();
            // 抓request
            callback.SaveRequest(guid, request.Method.Method, request.RequestUri.ToString(), request.Content?.ReadAsStringAsync()?.Result);

            var response = base.SendAsync(request, cancellationToken).Result;
            // 抓response
            var responseText = response.Content.ReadAsStringAsync().Result;
            callback.SaveResponse(guid, responseText);

            return Task.FromResult(response);
        }



        // 取Token
        private string GetToken()
        {
            if (expiredTime > DateTime.Now)
                return lastToken;

            // recreate token
            var request = new HttpRequestMessage(HttpMethod.Post, server);
            var parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", "fhir-twcore-0.2.0"),
                    new KeyValuePair<string, string>("client_secret", secret),
                };

            var sendTime = DateTime.Now;
            request.Content = new FormUrlEncodedContent(parameters);

            // 紀錄request
            var tokeGuid = Guid.NewGuid().ToString();
            callback.SaveRequest(tokeGuid, request.Method.Method, request.RequestUri.ToString(), request.Content.ReadAsStringAsync().Result);

            using (var client = new HttpClient())
            {
                var response = client.SendAsync(request).Result;

                // 紀錄response
                callback.SaveResponse(tokeGuid, response.Content.ReadAsStringAsync().Result);

                var jsonContent = response.Content.ReadAsStringAsync().Result;
                var jsonObj = JObject.Parse(jsonContent);
                var bearerToken = Convert.ToString(jsonObj["access_token"]);

                lastToken = bearerToken;
                expiredTime = sendTime.AddMinutes(3);
                return lastToken;
            }
        }
    }

}
