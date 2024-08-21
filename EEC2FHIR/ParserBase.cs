using EEC2FHIR.Utility;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EEC2FHIR
{
    public abstract class ParserBase
    {
        protected FhirClient client;
        public ParserBase(FhirClient client)
        {
            this.client = client;
        }
        /// <summary>
        /// 體系SystemCode
        /// </summary>
        public string SystemCodeGlobal { get; set; } = "https://www.cgmh.org.tw";
        /// <summary>
        /// 院區SystemCode
        /// </summary>
        public string SystemCodeLocal { get; set; } = "https://lnk.cgmh.org.tw";

        
        public static string SystemCodeLoinc { get; } = "http://loinc.org";
        public static string SystemCodeSnomed { get; } = "http://snomed.info/sct";

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
        protected Hl7.Fhir.Model.Quantity CreateQuantity(decimal value, string unit, string codeSystem = "http://www.cgmh.org.tw/unit")
        {
            return new Hl7.Fhir.Model.Quantity(value, unit, codeSystem);
        }

        
        
    }
}
