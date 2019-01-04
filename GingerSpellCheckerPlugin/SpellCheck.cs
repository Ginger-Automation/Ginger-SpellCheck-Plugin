using NHunspell;
using System;
using System.Reflection;

namespace GingerSpellCheckerPlugin
{
    internal class SpellCheck
    {
        internal bool Check(string curText, out string suggested)
        {
            //string binPath = Assembly.GetExecutingAssembly().Location;
            //binPath = binPath.Replace("GingerSpellCheckerPlugin.dll", "");
            ////using (Hunspell hunspell = new Hunspell(binPath + @"/Dictionairy/index.aff",
            //                        //binPath + @"/Dictionairy/index.dic"))
            //using (Hunspell hunspell = new Hunspell(@"C:\Users\shoha\source\repos\OCR1\OCR1\DIctionairy\index.aff",
            //                       @"C:\Users\shoha\source\repos\OCR1\OCR1\DIctionairy\/index.dic"))
            //{
            //    bool correct = hunspell.Spell(curText);
            //    if (!correct)
            //    {
            //        suggested = hunspell.Suggest(curText)[0];
            //        return false;
            //    }
            //}
            suggested = "";
            return true;
        }
    }
}