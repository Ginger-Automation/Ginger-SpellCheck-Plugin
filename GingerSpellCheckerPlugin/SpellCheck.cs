using System.Collections.Generic;
using System.Reflection;
using WeCantSpell.Hunspell;

namespace GingerSpellCheckerPlugin
{
    internal class SpellCheck
    {

        static WordList dictionairy;

        public SpellCheck()
        {
            if (dictionairy != null) return;
            string binpath = Assembly.GetExecutingAssembly().Location;
            binpath = binpath.Replace("GingerSpellCheckerPlugin.dll", "");
            dictionairy = WordList.CreateFromFiles(binpath + @"/dictionairy/index.dic");
        }

        internal bool Check(string curText, out string suggested)
        {
            bool correct = dictionairy.Check(curText);
            if (!correct)
            {
                var vv = dictionairy.Suggest(curText);
                var first = ((List<string>)dictionairy.Suggest(curText))[0];
                suggested = first;
                return false;
            }
            suggested = "";
            return true;
        }
    }
}