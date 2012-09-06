
using System.Collections.Generic;
using NUnit.Framework;
using Vxml;
using Vxml.Exceptions;

namespace VxmlUnitTests
{
    public class ElementTests
    {
        [Test]
        public void ItHasAName()
        {
            var submit = new VxmlElement(VxmlElement.Submit);
            Assert.That("submit", Is.EqualTo(submit.Name));
        }

        [Test]
        public void ItHasAListOfVxmlAttributes()
        {
            var submit = new VxmlElement(VxmlElement.Submit);
            var attributes = new List<VxmlAttribute>
                                 {
                                     new VxmlAttribute("next", "NextUrlToGoTo"),
                                     new VxmlAttribute("method", "get"),
                                     new VxmlAttribute("namelist", "contactNumber lastResult")
                                 };
            submit.Attributes = attributes;
            Assert.That(3, Is.EqualTo(submit.Attributes.Count));
            Assert.That("next", Is.EqualTo(submit.GetAttribute(VxmlAttribute.Next).AttributeName));
        }

        [Test]
        public void ThereAreStaticElementsUsedLikeEnums()
        {
            Assert.That("audio", Is.EqualTo(VxmlElement.Audio.Name));
            Assert.That("catch", Is.EqualTo(VxmlElement.Catch.Name));
            Assert.That("choice", Is.EqualTo(VxmlElement.Choice.Name));
            Assert.That("disconnect", Is.EqualTo(VxmlElement.Disconnect.Name));
            Assert.That("goto", Is.EqualTo(VxmlElement.GoTo.Name));
            Assert.That("noinput", Is.EqualTo(VxmlElement.NoInput.Name));
            Assert.That("nomatch", Is.EqualTo(VxmlElement.NoMatch.Name));
            Assert.That("submit", Is.EqualTo(VxmlElement.Submit.Name));
        }

        [Test]
        [ExpectedException(typeof(CannotModifyElementException))]
        public void CannotModifyStaticElementsAttributes()
        {
            var staticAudio = VxmlElement.Audio;
            staticAudio.Attributes = new List<VxmlAttribute>();
        }

        [Test]
        public void CanHaveListOfSubElements()
        {
            var noinput = new VxmlElement(VxmlElement.NoInput);
            noinput.Elements = new List<VxmlElement>
                                   {
                                       new VxmlElement(VxmlElement.Submit)
                                   };
            Assert.That(noinput.Elements.Count, Is.EqualTo(1));
            Assert.That(noinput.GetElement(VxmlElement.Submit).Name, Is.EqualTo(VxmlElement.Submit.Name));
        }

        [Test]
        [ExpectedException(typeof(CannotModifyElementException))]
        public void CannotModifyStaticElementsSubElements()
        {
            var staticNoInput = VxmlElement.NoInput;
            staticNoInput.Elements = new List<VxmlElement>();
        }

        [Test]
        public void CanPrintEmptyElement()
        {
            var submit = new VxmlElement(VxmlElement.Submit);
            Assert.That(submit.ToString(), Is.EqualTo("<submit />"));
        }

        [Test]
        public void CanPrintElementWithAttributes()
        {
            var submit = new VxmlElement(VxmlElement.Submit)
                             {
                                 Attributes = new List<VxmlAttribute>
                                     {
                                         new VxmlAttribute("next", "newUrl"),
                                         new VxmlAttribute("method", "get"),
                                         new VxmlAttribute("namelist", "contactNumber lastResult")
                                     }
                             };
            Assert.That(submit.ToString(), Is.EqualTo("<submit next=\"newUrl\" method=\"get\" namelist=\"contactNumber lastResult\" />"));
        }

        [Test]
        public void CanPrintElementWithSubElements()
        {
            var nomatch = new VxmlElement(VxmlElement.NoMatch)
                              {
                                  Attributes = new List<VxmlAttribute>
                                                   {
                                                       new VxmlAttribute("attName", "attValue")
                                                   },
                                  Elements = new List<VxmlElement>
                                                 {
                                                     new VxmlElement(VxmlElement.Submit)
                                                         {
                                                             Attributes = new List<VxmlAttribute>
                                                                              {
                                                                                  new VxmlAttribute("next", "newUrl")
                                                                              }
                                                         }
                                                 }
                              };
            Assert.That(nomatch.ToString(), Is.EqualTo("<nomatch attName=\"attValue\" />\n\t<submit next=\"newUrl\" />\n</nomatch>"));
        }
    }
}
