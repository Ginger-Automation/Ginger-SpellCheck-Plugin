using Amdocs.Ginger.Plugin.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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
            Console.WriteLine(DateTime.Now + "> Text: " + text);
            //In
            // Check if text is empty
            if (text == "")
            {
                GA.AddExInfo("Recieved empty string");
            }

            //Act
            //Do Spell Check
            SpellCheck spellChecker = new SpellCheck();
            string sugg = "";
            char[] seperators = { ' ', ',', ':', '(', ')', '"', '?' };
            string[] words = text.ToLower().Split(seperators);
            int incorrect = 0;
            int correct = 0;
            //Out
            // Add if correct or not and the suggestion
            foreach (string word in words)
            {
                if (word == "")
                {
                    continue;
                }
                string spelling = "Correct Spelling.";
                if (!spellChecker.Check(word, out sugg))
                {
                    spelling = "Incorrect Spelling. Suggestion: " + sugg;
                    incorrect++;
                } else
                {
                    correct++;
                }
                GA.AddOutput(text, spelling, "");
            }
            GA.AddOutput("Incorrect", incorrect, "");
            GA.AddOutput("Correct", correct, "");
        }

        [GingerAction("SpellCheckReturnImage", "Spell check an Image and get an Image with marks on the mispelling")]
        public void SpellCheckImage2 (IGingerAction GA, string fileName)
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

            // Create the Image with markers
            
        }

        [GingerAction("SpellCheckFolder", "Spell check all the images in a folder")]
        public void SpellCheckFolder(IGingerAction GA, string folderName)
        {
            Console.WriteLine(DateTime.Now + "> Filename: " + folderName);
            //In
            //get all the files in the folder
            string[] files = Directory.GetFiles(folderName, "*ProfileHandler.cs", SearchOption.TopDirectoryOnly);

            //Act and Out
            //loop over files and run spellcheckword
            if (files.Length == 0)
            {
                GA.AddError("There are no files in the folder: '" + folderName + "'.");
                return;
            }
            foreach (string file in files)
            {
                SpellCheckWord(GA, file);
            }
        }
    }
}
