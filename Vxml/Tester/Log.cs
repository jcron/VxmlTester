using System;
using System.Collections.Generic;
using System.Linq;

namespace Vxml.Tester
{
    public static class Log
    {
        public static void Print(string text)
        {
            Print(text, Console.ForegroundColor);
        }

        public static void Print(string text, ConsoleColor color)
        {
            var before = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = before;
        }

        public static void Exception(Exception ex)
        {
            Print(ex.Message, ConsoleColor.Red);
            Print(ex.StackTrace, ConsoleColor.Red);
        }

        public static void ItemsInIntList(List<int> list, string initialText, ConsoleColor color)
        {
            var logString = list.Aggregate(initialText, (current, item) => string.Format("{0} {1}", current, item));
            Print(logString, color);
        }

        public static void ItemsInDictionary(Dictionary<int, string> dict, ConsoleColor color)
        {
            foreach (var item in dict)
            {
                Print(string.Format("{0} | '{1}'", item.Key, item.Value), color);
            }   
        }
    }
}
