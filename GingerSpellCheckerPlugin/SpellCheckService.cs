﻿using Amdocs.Ginger.Plugin.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

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
            // Check if the file exist, if not set the proper message in GA.Error and return
            if (!File.Exists(fileName))
            {
                GA.AddError("Could not find the file: '" + fileName + "'");
                return;
            }

            //Act
            // Do the spell check or catch error.
            BitmapScanner bitmapScanner = new BitmapScanner();
            int incorrectCount = 0;
            List<TextBox> textBoxes = new List<TextBox>();
            try
            {
                textBoxes = bitmapScanner.getTextBoxes(fileName, true, out incorrectCount);
            } catch (Exception ex)
            {
                GA.AddError("Error while processing bitmap: " + ex.Message);
            }            

            //Out
            // Add all the words; param: word, path: position, value: Spelled In/Correctly.
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
        public void SpellCheckText(IGingerAction GA, string text, string line = "", int totalIncorrect = 0, int totalCorrect = 0)
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
                GA.AddOutput(word, spelling, line);
            }
            totalIncorrect = incorrect;
            totalCorrect = correct;
            GA.AddOutput("Incorrect", incorrect, line);
            GA.AddOutput("Correct", correct, line);
        }

        [GingerAction("SpellCheckTextFile", "Spell check all the words in a text file")]
        public void SpellCheckTextFile(IGingerAction GA, string fileName)
        {
            Console.WriteLine(DateTime.Now + "> Filename: " + fileName);
            //In
            //get all the words from the text file
            string[] lines = { };
            try
            {
                lines = File.ReadAllLines(fileName);
            } catch (Exception ex)
            {
                GA.AddError("Error while converting text file to text: " + ex.Message);
                return;
            }

            //Act and Out
            //loop over lines and run spellcheck if there was text found
            if (lines.Length == 0)
            {
                GA.AddError("There is no text found in the file: '" + fileName + "'.");
                return;
            }
            int lineNumber = 1;
            int totalIncorrect = 0;
            int totalCorrect = 0;
            int lineIncorrect = 0;
            int lineCorrect = 0;
            foreach (string line in lines)
            {
                string lineNumberString = "Line: " + lineNumber.ToString();
                SpellCheckText(GA, line, lineNumberString, lineIncorrect, lineCorrect);
                totalIncorrect += lineIncorrect;
                totalCorrect += lineCorrect;
            }
            GA.AddOutput("Total Incorrect", totalIncorrect, "");
            GA.AddOutput("Total Correct", totalCorrect, "");
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
