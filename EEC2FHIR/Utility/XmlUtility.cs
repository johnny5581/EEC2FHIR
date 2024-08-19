using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EEC2FHIR.Utility
{
    public static class XmlUtility
    {
        public static string XPathEvaluateString(this XElement node, string xpath, IXmlNamespaceResolver resolver, bool throwOnEmpty = false)
        {
            var contents = node.XPathEvaluate(xpath, resolver);
            var attr = ((IEnumerable)contents).OfType<XAttribute>().SingleOrDefault();
            if (attr != null)
                return attr.Value;
            var element = ((IEnumerable)contents).OfType<XElement>().SingleOrDefault();
            if (element != null)
                return element.Value;
            if (throwOnEmpty)
                throw new Exception("no evaluate string");
            return null;
        }

        public static XmlNamespaceManager CreateCdaR2NamespaceManager(this XDocument document)
        {
            var nsMgr = new XmlNamespaceManager(document.CreateNavigator().NameTable);

            // 新增命名空間
            nsMgr.AddNamespace("", "urn:hl7-org:v3");
            nsMgr.AddNamespace("ns", "urn:hl7-org:v3");
            nsMgr.AddNamespace("cdp", "http://www.hl7.org.tw/EMR/CDocumentPayload/v1.0");
            nsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            nsMgr.AddNamespace("xades", "http://uri.etsi.org/01903/v1.4.1#");
            nsMgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            nsMgr.AddNamespace("xmime", "http://www.w3.org/2005/05/xmlmime");

            return nsMgr;
        }
    }
}
