
using System;
using System.Collections.Generic;
using System.Linq;
using Vxml.Exceptions;

namespace Vxml
{
    public class VxmlElement
    {
        private const bool CannotModify = false;
        public const bool CanModify = true;
        public static readonly VxmlElement Vxml = new VxmlElement("vxml", CannotModify);
        public static readonly VxmlElement Submit = new VxmlElement("submit", CannotModify);
        public static readonly VxmlElement Audio = new VxmlElement("audio", CannotModify);
        public static readonly VxmlElement GoTo = new VxmlElement("goto", CannotModify);
        public static readonly VxmlElement Disconnect = new VxmlElement("disconnect", CannotModify);
        public static readonly VxmlElement Choice = new VxmlElement("choice", CannotModify);
        public static readonly VxmlElement Catch = new VxmlElement("catch", CannotModify);
        public static readonly VxmlElement NoInput = new VxmlElement("noinput", CannotModify);
        public static readonly VxmlElement NoMatch = new VxmlElement("nomatch", CannotModify);
        public static readonly VxmlElement Property = new VxmlElement("property", CannotModify);

        private readonly bool _modifiable;

        public VxmlElement(string name, bool modifiable)
        {
            _modifiable = modifiable;
            Name = name;
        }

        public VxmlElement(VxmlElement element)
        {
            _modifiable = CanModify;
            Name = element.Name;
        }

        public string Name { get; private set; }

        private List<VxmlAttribute> _attributes;
        public List<VxmlAttribute> Attributes
        {
            get
            {
                return _attributes;
            }
            set
            {
                if (_modifiable)
                {
                    _attributes = value;
                }
                else
                {
                    throw new CannotModifyElementException();
                }
            }
        }

        private List<VxmlElement> _elements;
        public List<VxmlElement> Elements
        {
            get
            {
                return _elements;
            }
            set
            {
                if (_modifiable)
                {
                    _elements = value;
                }
                else
                {
                    throw new CannotModifyElementException();
                }
            }
        }

        public VxmlAttribute GetAttribute(VxmlAttribute attribute)
        {
            return Attributes.Where(item => item.AttributeName == attribute.AttributeName).FirstOrDefault();
        }

        public VxmlElement GetElement(VxmlElement element)
        {
            return Elements.Where(item => item.Name == element.Name).FirstOrDefault();
        }

        public override string ToString()
        {
            var elementString = string.Format("<{0} ", Name);
            if (Attributes != null)
            {
                foreach (var item in Attributes)
                {
                    elementString = string.Format("{0}{1}=\"{2}\" ", elementString, item.AttributeName, item.AttributeValue);
                }
            }
            if (Elements != null)
            {
                foreach (var item in Elements)
                {
                    elementString = string.Format("{0}/>\n\t{1}", elementString, item);
                }
                elementString = string.Format("{0}\n</{1}>", elementString, Name);
            }
            else
            {
                elementString = string.Format("{0}/>", elementString);   
            }
            return elementString;
        }
    }
}
