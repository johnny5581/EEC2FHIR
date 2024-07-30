using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace EEC2FHIR.Demo
{
    internal static class Program
    { 
        [STAThread]
        static void Main()
        {
            var client = new FhirClient("http://localhost:11180/fhir", settings: new FhirClientSettings { PreferredFormat = ResourceFormat.Json }, messageHandler: new ProxyMessageHandler());
            var parser = new Laboratory.Parser(client);
            var xml = new XmlDocument();            
            xml.Load("lab.xml");
            var bundle = parser.Parse(xml);
            client.Create(bundle);

            //var querier = new TWOrganizationQuerier(client);
            //var org = querier.GetByTwIdentifier("1132070011");

            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }

        private class ProxyMessageHandler : HttpClientHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Console.WriteLine(request.RequestUri);
                if (request.Method == HttpMethod.Post)
                {
                    var result = request.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result);
                }
                return base.SendAsync(request, cancellationToken);
            }
        }

    }
}