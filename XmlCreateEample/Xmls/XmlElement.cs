using System;
using System.Collections.Generic;

namespace XmlCreateEample.Xmls
{
    public class XmlElement
    {
        public string Name { get; set; }
        public string TextContent { get; set; }
        public IEnumerable<XmlElement> Children { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class XmlElementPrefixAttribute : Attribute
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public bool SetNamespace { get; set; }

        public XmlElementPrefixAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class XmlElementAttrAttribute : Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string NamespacePrefix { get; set; }

        public XmlElementAttrAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
