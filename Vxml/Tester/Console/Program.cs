using System;
using System.Collections.Generic;
using Vxml.Tester;

namespace Vxml.Tester
{
    internal class Program
    {
        private const string EndingInput = "q";

        public static void Main(string[] args)
        {
            try
            {
	            var test = new Vxml();
	            var url = string.Empty;
	            var option = string.Empty;
                var breadCrumb = new List<int>();

                Log.Print("Press q at anytime to disconnect", ConsoleColor.Cyan);
	
	            do
                {
                    var choice = 0;
                    test.InvokeUrl(url);
                    //test.GetAllAudio();
	                var options = test.GetAllNextPages();
	                option = Console.ReadLine();
	                if (Int32.TryParse(option, out choice))
	                {
	                    if (!options.TryGetValue(choice, out url)) continue;
                        breadCrumb.Add(choice);
                        Log.ItemsInIntList(breadCrumb, "breadCrumb:", ConsoleColor.Yellow);
	                }
	                else
	                {
	                    url = string.Empty;
                        breadCrumb = new List<int>();
	                }
	            } while (option != null && option.ToLower() != EndingInput);
	            test.Hangup();
            }
            catch (Exception ex)
            {
            	Log.Exception(ex);
                Console.ReadLine();
            }
        }
    }
}
