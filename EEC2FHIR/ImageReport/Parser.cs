using EEC2FHIR.Utility;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEC2FHIR.ImageReport
{
    public class Parser : ParserBase
    {
        public Parser(FhirClient client) : base(client)
        {
        }

        public override Bundle Parse(string xml)
        {
            var doc = ConvertToDoc(xml);
            var nsMgr = doc.CreateCdaR2NamespaceManager();

            // 新增命名空間
            nsMgr.AddNamespace("", "urn:hl7-org:v3");
            nsMgr.AddNamespace("ns", "urn:hl7-org:v3");
            nsMgr.AddNamespace("cdp", "http://www.hl7.org.tw/EMR/CDocumentPayload/v1.0");
            nsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            nsMgr.AddNamespace("xades", "http://uri.etsi.org/01903/v1.4.1#");
            nsMgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");



            throw new NotImplementedException();
        }
    }
}
