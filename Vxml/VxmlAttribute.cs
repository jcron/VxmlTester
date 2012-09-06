
namespace Vxml
{
    public class VxmlAttribute
    {
        public static readonly VxmlAttribute Next = new VxmlAttribute("next");
        public static readonly VxmlAttribute Src = new VxmlAttribute("src");
        public static readonly VxmlAttribute Event = new VxmlAttribute("event");
        public static readonly VxmlAttribute Version = new VxmlAttribute("version");
        public static readonly VxmlAttribute Xmlns = new VxmlAttribute("xmlns");
        public static readonly VxmlAttribute Name = new VxmlAttribute("name");
        public static readonly VxmlAttribute Value = new VxmlAttribute("value");

        public VxmlAttribute(string name, string value)
        {
            AttributeName = name;
            AttributeValue = value;
        }

        public string AttributeName { get; private set; }
        public string AttributeValue { get; private set; }

        public override string ToString()
        {
            return AttributeName;
        }

        private VxmlAttribute(string name)
        {
            AttributeName = name;
            AttributeValue = string.Empty;
        }
    }
}
