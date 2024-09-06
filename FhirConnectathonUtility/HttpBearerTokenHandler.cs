using Microsoft.Owin.Hosting;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

namespace FhirConn.Utility
{
    public class HttpBearerTokenHandler : HttpClientHandler
    {
        private string lastToken;
        private DateTime expiredTime;
        private string clientSecret;
        private readonly IHttpMessageHandlerCallback callback;
        private string server;
        private readonly string clientId;

        public HttpBearerTokenHandler(IHttpMessageHandlerCallback callback, string server, string clientId, string clientSecret)
        {
            this.callback = callback;
            this.server = server;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
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
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
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

    public class OAuth2BearerTokenHandler : HttpClientHandler
    {
        private static string latestCode;
        private string lastToken;
        private DateTime expiredTime;
        private string clientSecret;
        private readonly string callbackUrl;
        private readonly IHttpMessageHandlerCallback callback;
        private string server;
        private readonly string clientId;

        public OAuth2BearerTokenHandler(IHttpMessageHandlerCallback callback, string server, string clientId, string clientSecret)
        {
            this.callback = callback;
            this.server = server;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
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

        public class CallbackController : ApiController
        {
            public IHttpActionResult Get(string code = null)
            {
                latestCode = code;
                return Ok();
            }
        }

        // 取Token
        private string GetToken()
        {
            if (expiredTime > DateTime.Now)
                return lastToken;

            // 啟動Selfhost吃callback

            var hostUrl = "http://+:12080";
            var host = WebApp.Start(hostUrl, b =>
            {
                var conf = new HttpConfiguration();
                conf.Routes.MapHttpRoute("default", "{controller}/{id}", new { id = RouteParameter.Optional });
                b.UseWebApi(conf);
            });

            latestCode = null;
            var psi = Process.Start("http://172.18.0.58:8080/realms/mitw/protocol/openid-connect/auth?client_id=EBM_client&grant_type=authorization_code&response_type=code&redirect_uri=http://172.18.2.8:12080/callback");

            while (string.IsNullOrEmpty(latestCode))
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(1000);
            }

            // recreate token
            var request = new HttpRequestMessage(HttpMethod.Post, server);
            var parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("code", latestCode),
                    new KeyValuePair<string, string>("redirect_uri", "http://172.18.2.8:12080/callback")
                };

            var sendTime = DateTime.Now;
            request.Content = new FormUrlEncodedContent(parameters);

            // 紀錄request
            var tokeGuid = Guid.NewGuid().ToString();
            callback.SaveRequest(tokeGuid, request.Method.Method, request.RequestUri.ToString(), request.Content.ReadAsStringAsync().Result);

            // 關閉連線
            host.Dispose();

            using (var client = new HttpClient())
            {
                var response = client.SendAsync(request).Result;

                // 紀錄response
                callback.SaveResponse(tokeGuid, response.Content.ReadAsStringAsync().Result);

                var jsonContent = response.Content.ReadAsStringAsync().Result;
                var jsonObj = JObject.Parse(jsonContent);
                var bearerToken = Convert.ToString(jsonObj["access_token"]);

                if (string.IsNullOrEmpty(bearerToken))
                    throw new Exception("invalid access token");

                lastToken = bearerToken;
                expiredTime = sendTime.AddMinutes(3);
                return lastToken;
            }
        }
    }

}
