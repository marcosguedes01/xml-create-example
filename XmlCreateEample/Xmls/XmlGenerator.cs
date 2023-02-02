using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;

namespace XmlCreateEample.Xmls
{
    public class XmlGenerator
    {
        public string DocumentVersion { get; set; } = "1.0";
        public string DocumentEncoding { get; set; } = "UTF-8";

        public string GenerateToString(XmlElement element)
        {
            var root = GetXElement(element);

            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(root.ToString()));

            XmlDeclaration xmldecl;
            xmldecl = doc.CreateXmlDeclaration(DocumentVersion, null, null);
            xmldecl.Encoding = DocumentEncoding;

            doc.InsertBefore(xmldecl, doc.DocumentElement);

            return doc.OuterXml;
        }

        public XElement GetXElement(XmlElement element)
        {
            var prefixAttribute = GetPrefixAttributeValue(element);
            var attributes = GetAttrAttributeValue(element);

            XElement xElement;

            if (prefixAttribute != null)
            {
                XNamespace env = prefixAttribute.Namespace;
                if (prefixAttribute.SetNamespace)
                {
                    xElement = new XElement(
                        env + element.Name,
                        new XAttribute(XNamespace.Xmlns + prefixAttribute.Name, prefixAttribute.Namespace)
                    );
                }
                else
                {
                    xElement = new XElement(env + element.Name);
                }
            }
            else
            {
                xElement = new XElement(
                    element.Name
                );
            }

            ProcessElementAttributes(attributes, xElement);

            if (!string.IsNullOrEmpty(element.TextContent))
            {
                xElement.SetValue(element.TextContent);
            }

            ProcessChildrenElements(element, xElement);

            return xElement;
        }

        private static void ProcessElementAttributes(IEnumerable<XmlElementAttrAttribute> attributes, XElement xElement)
        {
            foreach (var attribute in attributes)
            {
                if (!string.IsNullOrEmpty(attribute.NamespacePrefix))
                {
                    XNamespace env = attribute.NamespacePrefix;

                    xElement.SetAttributeValue(env + attribute.Name, attribute.Value);
                }
                else
                {
                    xElement.SetAttributeValue(XNamespace.Xmlns + attribute.Name, attribute.Value);
                }
            }
        }

        private void ProcessChildrenElements(XmlElement element, XElement xElement)
        {
            if (element.Children != null) foreach (var child in element.Children)
                {
                    var nodes = xElement.Nodes().ToList();
                    nodes.Add(GetXElement(child));

                    xElement.ReplaceNodes(nodes);
                }
        }

        private XmlElementPrefixAttribute GetPrefixAttributeValue(XmlElement element)
        {
            // Get instance of the attribute.
            return (XmlElementPrefixAttribute) Attribute.GetCustomAttribute(element.GetType(), typeof(XmlElementPrefixAttribute));
        }

        private IEnumerable<XmlElementAttrAttribute> GetAttrAttributeValue(XmlElement element)
        {
            // Get instance of the attribute.
            return (XmlElementAttrAttribute[]) Attribute.GetCustomAttributes(element.GetType(), typeof(XmlElementAttrAttribute));
        }
    }
}
