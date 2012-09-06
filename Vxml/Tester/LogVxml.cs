using System;
using System.Collections.Generic;
using System.IO;
using Vxml;

namespace Vxml.Tester
{
    public static class LogVxml
    {
        public static void Print(string vxml)
        {
            using (var stringReader = new StringReader(vxml))
            {
                var line = string.Empty;
                while (true)
                {
                    line = stringReader.ReadLine();
                    if (line == null)
                    {
                        Log.Print(string.Empty);
                        break;
                    }
                    Log.Print(line, GetColor(line));
                }
            }
        }

        public static void ItemsInList(List<string> list, VxmlElement element, VxmlAttribute attribute, ConsoleColor color)
        {
            var count = 0;
            foreach (var item in list)
            {
                Log.Print(string.Format("{3} | {0}-{1}: '{2}'", element, attribute, item, ++count), color);
            }
        }

        private static ConsoleColor GetColor(string line)
        {
            var color = ConsoleColor.White;
            if (LineContains(line, VxmlElement.GoTo) || LineContains(line, VxmlElement.Submit) || LineContains(line, VxmlElement.Choice))
            {
                color = ConsoleColor.Green;
            }
            if (LineContains(line, VxmlElement.Catch) || LineContains(line, VxmlElement.Disconnect))
            {
                color = ConsoleColor.Red;
            }
            if (LineContains(line, VxmlElement.NoInput) || LineContains(line, VxmlElement.NoMatch))
            {
                color = ConsoleColor.Yellow;
            }
            if (LineContains(line, VxmlElement.Audio))
            {
                color = ConsoleColor.Magenta;
            }
            return color;
        }

        private static bool LineContains(string line, VxmlElement element)
        {
            return line.Contains(element.Name);
        }
    }
}
