using Amdocs.Ginger.Plugin.Core;
using System;
using System.Threading;

namespace GingerSpellCheckerPlugin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting SpellCheck Service");
            using (GingerNodeStarter gingerNodeStarter = new GingerNodeStarter())
            {
                if (args.Length > 0)
                {
                    gingerNodeStarter.StartFromConfigFile(args[0]);  // file name 
                }
                else
                {
                    gingerNodeStarter.StartNode("Spellcheck Service", new SpellCheckService());
                }
                gingerNodeStarter.Listen();
            }
        }

    }
}
