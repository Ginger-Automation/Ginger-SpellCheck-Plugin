using Amdocs.Ginger.Plugin.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GingerSpellCheckerPlugin
{
    [GingerService("SpellCheckService", "Spell Check service")]
    public class SpellCheckService
    {
        [GingerAction("SpellCheckBitmap", "Spell check a bitmap")]
        public void SpellCheckWord(IGingerAction GA, string fileName)
        {
            Console.WriteLine(DateTime.Now + "> Filename: " + fileName);
            //In
            // Check file exist if not set proper message in GA.Error and return
            if (!File.Exists(fileName))
            {
                GA.AddError("Could not find the file: '" + fileName + "'");
                return;
            }

            //Act
            // Do spell check
            BitmapScanner bitmapScanner = new BitmapScanner();
            int incorrectCount;
            List<TextBox> textBoxes = bitmapScanner.getTextBoxes(fileName, true, out incorrectCount);
            GA.AddError("The file '" + fileName + "' is not an image");

            //Out
            // add ALL words, param is the word, path is the index, for each word found, value = OK/NOK(bad spell)
            if (textBoxes.Count == 0)
            {
                GA.AddExInfo("Could not find any text");
                return;
            }
            GA.AddOutput("Total Words", textBoxes.Count);
            GA.AddOutput("Incorrect", incorrectCount);
            GA.AddOutput("Correct", textBoxes.Count - incorrectCount);
            foreach (TextBox textBox in textBoxes)
            {
                string spelling = "Correct Spelling.";
                if (!textBox.isCorrectSpelling)
                {
                    spelling = "Incorrect Spelling. Suggestion: " + textBox.suggestion;
                }
                string position = "Position: (" + textBox.xPosition + "," + textBox.yPosition + ")";
                string size = "Width: " + textBox.width + "Height: " + textBox.height;
                GA.AddOutput(textBox.text, spelling, position + " " + size);
            }
        }

        [GingerAction("SpellCheckText", "Spell check a text")]
        public void SpellCheckText(IGingerAction GA, string text)
        {
            //In
            // Check if text is empty
            if (text == "")
            {
                GA.AddExInfo("Recieved empty string");
            }

            //Act
            //Do Spell Check
            string spelling = "Correct Spelling.";
            SpellCheck spellChecker = new SpellCheck();
            string sugg = "";
            if (spellChecker.Check(text, out sugg))
            {
                spelling = "Incorrect Spelling. Suggestion: " + sugg;
            }

            //Out
            // Add if correct or not and the suggestion
            GA.AddOutput(text, spelling, "");
        }
    }
}
