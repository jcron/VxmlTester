
using System;
using NUnit.Framework;
using Vxml;

namespace VxmlUnitTests
{
    public class VxmlPageTests
    {
        private string _vxml;

        [SetUp]
        public void SetUp()
        {
            _vxml = "<vxml xmlns=\"http://xmlnsurl\" version=\"2.1\" ><property name=\"variableName\" value=\"variableValue\" /><property name=\"variableName1\" value=\"variableValue1\" /></vxml>";
        }

        [Test]
        public void CanParseVxmlTag()
        {
            var vxmlPage = new VxmlPage(_vxml);
            vxmlPage.Parse();

            Assert.That(vxmlPage.XmlNamespace, Is.EqualTo("http://xmlnsurl"));
            Assert.That(vxmlPage.Version, Is.EqualTo("2.1"));
        }

        [Test]
        public void CanParseOutAnElementWithAttributes()
        {
            var vxmlPage = new VxmlPage(_vxml);
            vxmlPage.Parse();

            Assert.That(vxmlPage.Elements.Count, Is.EqualTo(2));
            Assert.That(vxmlPage.Elements[0].Name, Is.EqualTo("property"));
            Assert.That(vxmlPage.Elements[0].Attributes.Count, Is.EqualTo(2));
        }

        [Test]
        public void CanParseOutMultipleElements()
        {
            var vxmlPage = new VxmlPage(_vxml);
            vxmlPage.Parse();

            Assert.That(vxmlPage.Elements.Count, Is.EqualTo(2));
            Assert.That(vxmlPage.Elements[1].Name, Is.EqualTo("property"));
            Assert.That(vxmlPage.Elements[1].Attributes.Count, Is.EqualTo(2));
        }

        [Test]
        public void HasNoElementsWhenGivenEmptyString()
        {
            var vxmlPage = new VxmlPage(string.Empty);

            Assert.That(vxmlPage.Parse(), Is.EqualTo(false));
            Assert.That(vxmlPage.Elements.Count, Is.EqualTo(0));
        }

        [Test]
        public void HasNoElementsWhenGivenInvalidXml()
        {
            var vxmlPage = new VxmlPage("<vxml></garbage>");

            Assert.That(vxmlPage.Parse(), Is.EqualTo(false));
            Assert.That(vxmlPage.Elements.Count, Is.EqualTo(0));   
        }

        [Test]
        [Ignore]
        public void CanParseVxmlWithElementsThatContainElements()
        {
            var vxml = "<vxml xmlns=\"http://xmlnsurl\" version=\"2.1\" ><catch event=\"connection.disconnect.hangup\"><goto next=\"nextUrl\" /></catch></vxml>";
            var vxmlPage = new VxmlPage(vxml);
            vxmlPage.Parse();

            Assert.That(vxmlPage.Elements.Count, Is.EqualTo(1));
            Assert.That(vxmlPage.Elements[0].Name, Is.EqualTo(VxmlElement.Catch.Name));
            Assert.That(vxmlPage.Elements[0].Attributes.Count, Is.EqualTo(1));
            Assert.That(vxmlPage.Elements[0].Elements[0].Name, Is.EqualTo(VxmlElement.GoTo.Name));
        }

        [Test]
        public void CanPrintTheVxmlAfterParsing()
        {
            var vxmlPage = new VxmlPage(_vxml);
            vxmlPage.Parse();

            var formattedVxml = "<vxml xmlns=\"http://xmlnsurl\" version=\"2.1\" >\n\t<property name=\"variableName\" value=\"variableValue\" />\n\t<property name=\"variableName1\" value=\"variableValue1\" />\n</vxml>";

            Assert.That(vxmlPage.Print(), Is.EqualTo(formattedVxml));
        }
    }
}
