using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EEC2FHIR
{
    public abstract class ParserBase
    {
        protected FhirClient client;
        public ParserBase(FhirClient client)
        {
            this.client = client;
        }

        public abstract Bundle Parse(string xml);
        

        protected T CreateResource<T>(T resource)
            where T : Resource
        {
            if (!string.IsNullOrEmpty(resource.Id))
            {
                // 根據FHIR規範，如果有ID，就使用PUT產生資源
                return client.Update(resource);
            }
            else
            {
                return client.Create(resource);
            }
        }

        protected T UpdateResource<T>(T resource)
            where T : Resource
        {
            return client.Update(resource);
        }

        protected XDocument ConvertToDoc(string xml)
        {
            return XDocument.Parse(xml);
        }

        protected string GetCodeSystem(string root, string system)
        {
            if (root.EndsWith("/"))
                return root + system;
            return root + "/" + system;
        }
    }
}
