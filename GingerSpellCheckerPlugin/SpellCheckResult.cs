using System;
using System.Collections.Generic;
using System.Text;

namespace GingerSpellCheckerPlugin
{
    public class SpellCheckResult
    {
        public int Incorrect { get; set; }
        public int Correct { get; set; }

        public SpellCheckResult() { }

        public SpellCheckResult(int setIncorrect, int setCorrect)
        {
            Incorrect = setIncorrect;
            Correct = setCorrect;
        }
    }
}
