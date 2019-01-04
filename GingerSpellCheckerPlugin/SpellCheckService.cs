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
        [GingerAction("SpellCheck", "Spell check a bitmap")]
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
            //Out
            // add ALL words, param is the word, path is the index, for each word found, value = OK/NOK(bad spell)
            if (textBoxes.Count == 0)
            {
                GA.AddExInfo("Could not find any text");
                return;
            }
            int i = 0;
            GA.AddOutput("Total Words", textBoxes.Count);
            GA.AddOutput("Incorrect", incorrectCount);
            GA.AddOutput("Correct", textBoxes.Count - incorrectCount);
            foreach (TextBox textBox in textBoxes)
            {
                string spelling = "Correct Spelling";
                if (!textBox.isCorrectSpelling)
                {
                    spelling = "Incorrect Spelling. Suggestion: " + textBox.suggestion;
                }
                string position = "Position: (" + textBox.xPosition + "," + textBox.yPosition + ")";
                string size = "Width: " + textBox.width + "Height: " + textBox.height;
                GA.AddOutput("Word " + i + ": " + textBox.text, spelling, position + " " + size);
                i++;
            }
            // EX: GA.AddOutput("Hello", "OK", "1");
            //   GA.AddOutput("jokmlopo", "NOK", "2");
            // output also bitmap width, height, dpi and anything interesting
            // GA.AddExInfo("KooKoo");   // write ex info on the bitmap
        }

        [GingerAction("SpellCheck", "Spell check a bitmap")]
        public void SpellCheckTxt(IGingerAction GA, string text)
        {
        }
    }
}
