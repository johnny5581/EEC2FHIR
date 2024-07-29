using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EEC2FHIR.Utility
{
    internal static class XmlUtility
    {
        public static string GetAttributeValue(this XmlNode node, string name)
        {
            var attr = node.Attributes[name];
            if (attr != null)
                return attr.Value;
            return null;
        }
        public static string GetAttributeValue(this XmlNode node, string localName, string namespaceUri)
        {
            var attr = node.Attributes[localName, namespaceUri];
            if (attr != null)
                return attr.Value;
            return null;
        }

        
    }
}
