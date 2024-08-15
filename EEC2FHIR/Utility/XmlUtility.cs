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
    internal static class XmlUtility
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
    }
}
