
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Vxml
{
    public class VxmlPage
    {

        public string Version { get; private set; }
        public string XmlNamespace { get; private set; }

        public List<VxmlElement> Elements { get; private set; }

        private readonly string _vxml;

        public VxmlPage(string vxml)
        {
            _vxml = vxml;
            Elements = new List<VxmlElement>();
        }

        public bool Parse()
        {
            using (var stringReader = new StringReader(_vxml))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                try
                {
	                xmlReader.Read();
	                XmlNamespace = xmlReader.GetAttribute(VxmlAttribute.Xmlns.AttributeName);
	                Version = xmlReader.GetAttribute(VxmlAttribute.Version.AttributeName);
	
	                while (xmlReader.Read())
	                {
	                    var element = CreateElement(xmlReader);
	
	                    if (xmlReader.HasAttributes)
	                    {
	                        AddAttributesToElement(xmlReader, element);
	                    }
                        if (xmlReader.IsStartElement())
                        {
                            AddElement(element);
                        }
	                }
                }
                catch (XmlException)
                {
                    return false;
                }
                return true;
            }
        }

        public string Print()
        {
            var printedVxml = string.Format("<{0} {1}=\"{2}\" {3}=\"{4}\" >\n",
                VxmlElement.Vxml.Name, VxmlAttribute.Xmlns.AttributeName, XmlNamespace, VxmlAttribute.Version.AttributeName, Version);

            foreach (var vxmlElement in Elements)
            {
                printedVxml = string.Format("{0}\t{1}\n", printedVxml, vxmlElement);
            }
            printedVxml = string.Format("{0}</{1}>", printedVxml, VxmlElement.Vxml.Name);

            return printedVxml;
        }

        private static void AddAttributesToElement(XmlReader xmlReader, VxmlElement element)
        {
            while (xmlReader.MoveToNextAttribute())
            {
                element.Attributes.Add(CreateAttribute(xmlReader));
            }
        }

        private static VxmlAttribute CreateAttribute(XmlReader xmlReader)
        {
            return new VxmlAttribute(xmlReader.Name, xmlReader.Value);
        }

        private static VxmlElement CreateElement(XmlReader xmlReader)
        {
            return new VxmlElement(xmlReader.Name, VxmlElement.CanModify)
                       {
                           Attributes = new List<VxmlAttribute>()
                       };
        }

        private void AddElement(VxmlElement element)
        {
            if (element.Name != VxmlElement.Vxml.Name)
            {
                Elements.Add(element);
            }
        }
    }
}
