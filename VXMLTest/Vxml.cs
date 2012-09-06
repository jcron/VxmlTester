using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Xml;
using Vxml;

namespace VxmlTest
{
    public class Vxml
    {
        public string Server { get; set; }
        public string InitialUrl { get; set; }

        private CookieContainer _cookieContainer;
        private string _vxml;
        private Dictionary<int, string> _options;
        private bool _hungup;

        public Vxml()
        {
            Server = ConfigurationManager.AppSettings["WebServer"];
            InitialUrl = ConfigurationManager.AppSettings["InitialPage"];
            _options = new Dictionary<int, string>();
        }

        public void InvokeUrl(string newUrl)
        {
            try
            {
	            var url = GetUrl(newUrl);
                using (var webResponse = GetWebResponse(url))
                {
                    GetVxmlText(webResponse);
                }
            }
            catch (Exception ex)
            {
            	Log.Exception(ex);
            }
        }

        public void GetAllAudio()
        {
            var element = VxmlElement.Audio;
            var attribute = VxmlAttribute.Src;
            var audio = GetAttributeFromAllElements(_vxml, element, attribute);
            LogVxml.ItemsInList(audio, element, attribute, ConsoleColor.Magenta);
        }

        public void Hangup()
        {
            if (_hungup)
            {
                return;
            }

            var hangup = FindNextPage(_vxml, VxmlElement.Catch, VxmlAttribute.Event);
            InvokeUrl(hangup);
        }

        public Dictionary<int, string> GetAllNextPages()
        {
            _options.Clear();
            AddToOptions(GetAttributeFromAllElements(_vxml, VxmlElement.Submit, VxmlAttribute.Next));
            AddToOptions(GetAttributeFromAllElements(_vxml, VxmlElement.Choice, VxmlAttribute.Next));
            AddToOptions(GetAttributeFromAllElements(_vxml, VxmlElement.GoTo, VxmlAttribute.Next));
            Log.Print("-----------------------------------------------------------------------------", ConsoleColor.Blue);
            Log.ItemsInDictionary(_options, ConsoleColor.Green);
            CheckCallerDisconnected();
            return _options;
        }

        private string FindNextPage(string vxml, VxmlElement parentElement, VxmlAttribute attribute)
        {
            var attributeText = string.Empty;
            using (var stringReader = new StringReader(vxml))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                do
                {
                    if (xmlReader.ReadToFollowing(parentElement.Name))
                    {
                        attributeText = xmlReader.GetAttribute(attribute.AttributeName);
                    }

                } while (!attributeText.Contains("hangup"));

                if (xmlReader.ReadToDescendant(VxmlElement.GoTo.Name))
                {
                    attributeText = xmlReader.GetAttribute(VxmlAttribute.Next.AttributeName);
                }
            }
            return attributeText;
        }

        private void AddToOptions(List<string> options)
        {
            foreach (var item in options)
            {
                _options.Add(_options.Count + 1, item);
            }
        }

        private void CheckCallerDisconnected()
        {
            if (!DoesElementExist(_vxml, VxmlElement.Disconnect))
            {
                return;
            }

            EndCall();
        }

        private void EndCall()
        {
            _hungup = true;
            Log.Print("<<<<<< ----- End Of Call ----- >>>>>>", ConsoleColor.Red);
            Log.Print("Press Enter for a new Call", ConsoleColor.Cyan);
        }

        private List<string> GetAttributeFromAllElements(string vxml, VxmlElement element, VxmlAttribute attribute)
        {
            var options = new List<string>();

            using (var xmlReader = XmlReader.Create(new StringReader(vxml)))
            {
                while (xmlReader.ReadToFollowing(element.Name))
                {
                    var attributeText = xmlReader.GetAttribute(attribute.AttributeName);
                    options.Add(attributeText);
                }
            }
            return options;
        }

        private bool DoesElementExist(string vxml, VxmlElement element)
        {
            using (var xmlReader = XmlReader.Create(new StringReader(vxml)))
            {
                return xmlReader.ReadToFollowing(element.Name);
            }
        }

        private WebResponse GetWebResponse(string url)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.CookieContainer = _cookieContainer;
            return webRequest.GetResponse();
        }

        private string GetUrl(string newUrl)
        {
            var url = Server + newUrl;
            if (string.IsNullOrEmpty(newUrl))
            {
                url = StartNewCall(url);
            }
            Log.Print("Calling: '" + url + "'");
            return url;
        }

        private string StartNewCall(string url)
        {
            url += InitialUrl;
            _cookieContainer = new CookieContainer();
            _hungup = false;
            Log.Print("<<<<<< ----- New Call ----- >>>>>>", ConsoleColor.Yellow);
            return url;
        }

        private void GetVxmlText(WebResponse webResponse)
        {
            using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                var vxml = streamReader.ReadToEnd();
                _vxml = vxml;

                LogVxml.Print(_vxml);
            }
        }
    }
}
