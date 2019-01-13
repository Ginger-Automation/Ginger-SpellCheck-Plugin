using System.Collections.Generic;
using System.Reflection;
using WeCantSpell.Hunspell;

namespace GingerSpellCheckerPlugin
{
    public class SpellCheck
    {

        static WordList dictionairy;

        public SpellCheck()
        {
            if (dictionairy != null) return;
            string binpath = Assembly.GetExecutingAssembly().Location;
            binpath = binpath.Replace("GingerSpellCheckerPlugin.dll", "");
            dictionairy = WordList.CreateFromFiles(binpath + @"/dictionairy/index.dic");
        }

        public bool Check(string curText)
        {
            return dictionairy.Check(curText);
        }

        public List<string> Suggest(string curText)
        {
            return (List<string>)dictionairy.Suggest(curText);
        }

        public SpellCheckResult CheckLine(string line)
        {
            int numberIncorrect = 0;
            int numberCorrect = 0;

            //char[] seperators = { ' ', ',', ':', '(', ')', '"', '?' }; //TODO: Enable different seperators
            string[] words = line.Split(" ");

            foreach (string word in words)
            {
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }
                if (Check(word))
                {
                    numberCorrect++;
                }
                else
                {
                    numberIncorrect++;
                }
            }
            SpellCheckResult spellCheckResult = new SpellCheckResult(numberIncorrect, numberCorrect);
            return spellCheckResult;
        }
    }
}