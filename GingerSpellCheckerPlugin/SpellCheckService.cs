using Amdocs.Ginger.Plugin.Core;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace GingerSpellCheckerPlugin
{
    [GingerService("SpellCheckService", "Spell Check service")]
    public class SpellCheckService
    {
        [GingerAction("SpellCheckBitmap", "Spell check a bitmap")]
        public void SpellCheckBitmap(IGingerAction GA, string fileName)
        {
            Console.WriteLine(DateTime.Now + "> Filename: " + fileName);
            //In
            // Check if the file exist.
            if (!File.Exists(fileName))
            {
                GA.AddError("Could not find the file: '" + fileName + "'");
                return;
            }

            //Act
            // Do the spell check or catch error.
            BitmapScanner bitmapScanner = new BitmapScanner(fileName);
            try
            {
                bitmapScanner.ScanTextBoxes();
                bitmapScanner.DoSpellCheck();
            } catch (Exception ex)
            {
                GA.AddError("Error while processing: " + ex.Message);
                return;
            }            

            //Out
            // Add all the words; param: word, path: position, value: Spelled In/Correctly.
            if (bitmapScanner.TextBoxes.Count == 0)
            {
                GA.AddExInfo("Could not find any text");
                return;
            }
            GA.AddOutput("Total Words", bitmapScanner.TextBoxes.Count);
            GA.AddOutput("Incorrect", bitmapScanner.IncorrectCounter);
            GA.AddOutput("Correct", bitmapScanner.TextBoxes.Count - bitmapScanner.IncorrectCounter);
            foreach (TextBox textBox in bitmapScanner.TextBoxes)
            {
                string spelling;
                if (textBox.isCorrectSpelling)
                {
                    spelling = "Correct Spelling.";
                } else
                {
                    spelling = "Incorrect Spelling. Suggestion: " + textBox.suggestion;
                }                
                string position = "Position: (" + textBox.xPosition + "," + textBox.yPosition + ")";
                string size = "Width: " + textBox.width + ", Height: " + textBox.height;
                GA.AddOutput(textBox.text, spelling, position + " " + size);
            }
        }

        [GingerAction("SpellCheckBitmapAndHighlightMisspelling", "Spell check an Image and get an Image with marks on the mispelling")]
        public void SpellCheckBitmapAndHighlightMisspelling(IGingerAction GA, string filePath, string outputFolder)
        {
            Console.WriteLine(DateTime.Now + "> Filename: " + filePath);
            //In
            // Check file exist if not set proper message in GA.Error and return
            if (!File.Exists(filePath))
            {
                GA.AddError("Could not find the file: '" + filePath + "'");
                return;
            }

            //Act            
            BitmapScanner bitmapScanner = new BitmapScanner(filePath);          
            try
            {
                bitmapScanner.ScanTextBoxes();
                bitmapScanner.DoSpellCheck();
                bitmapScanner.CreateHighlightedImage();
            }
            catch (Exception ex)
            {
                GA.AddError("Error while processing bitmap: " + ex.Message);
                return;
            }

            //Out
            // Save Image
            Bitmap bitmap = bitmapScanner.BitmapWithHightlight;
            string outFilePath = Path.Combine(outputFolder, Path.GetFileName(filePath));
            bitmap.Save(outFilePath);            

            GA.AddOutput("Incorrect", bitmapScanner.IncorrectCounter, filePath);
            GA.AddOutput("Correct", bitmapScanner.CorrectCounter, filePath);
            GA.AddOutput("OutputFile", outFilePath);
        }

        [GingerAction("SpellCheckBimapFolder", "Spell check all the bitmaps in a folder")]
        public void SpellCheckFolder(IGingerAction GA, string folderName)
        {
            Console.WriteLine(DateTime.Now + "> Foldername: " + folderName);
            //In
            //get all the files in the folder
            string[] files = Directory.GetFiles(folderName, "*.*"); // TODO: search only images extension

            //Act and Out
            //loop over files and run spellcheckword
            if (files.Length == 0)
            {
                GA.AddError("There are no files in the folder: '" + folderName + "'.");
                return;
            }

            //path: filename, param: IncorrectCount, TotalCount, value. 
            int totalIncorrect = 0;
            int totalCorrect = 0;
            int words;
            int totalWords = 0;
            // foreach (string file in files)
            Parallel.ForEach(files, file => 
            { 
                {
                    BitmapScanner bitmapScanner = new BitmapScanner(file);
                    bitmapScanner.ScanTextBoxes();
                    bitmapScanner.DoSpellCheck();
                    words = bitmapScanner.TextBoxes.Count;
                    totalIncorrect += bitmapScanner.IncorrectCounter;
                    totalCorrect += bitmapScanner.CorrectCounter;
                    totalWords += words;
                    GA.AddOutput("Incorrect", bitmapScanner.IncorrectCounter, file);
                    GA.AddOutput("Correct", bitmapScanner.CorrectCounter, file);
                    GA.AddOutput("Words in file", words, file);
                }
            });
            GA.AddOutput("Total Incorrect", totalIncorrect, folderName);
            GA.AddOutput("Total Correct", totalCorrect, folderName);
            GA.AddOutput("Total Words", totalWords, folderName);
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
            //char[] seperators = { ' ', ',', ':', '(', ')', '"', '?' }; //TODO: fix
            string[] words = text.Split(" ");
            int incorrect = 0;
            int correct = 0;
            //Out
            // Add if correct or not and the suggestion
            foreach (string word in words)
            {
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }
                string spelling = "Correct Spelling.";
                if (!spellChecker.Check(word))
                {
                    string sugg = string.Join(',', spellChecker.Suggest(word));
                    spelling = "Incorrect Spelling. Suggestion: " + sugg;
                    incorrect++;
                } else
                {
                    correct++;
                }
                GA.AddOutput(word, spelling, "");
            }
            GA.AddOutput("Incorrect", incorrect);
            GA.AddOutput("Correct", correct);
            GA.AddOutput("Total", correct + incorrect);
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
            int lineNumber = 0;
            int totalIncorrect = 0;
            int totalCorrect = 0;
            SpellCheck spellCheck = new SpellCheck();
            foreach (string line in lines)
            {
                lineNumber++;
                string lineNumberString = "Line " + lineNumber;
                SpellCheckResult spellCheckResult = spellCheck.CheckLine(line);
                totalIncorrect += spellCheckResult.Incorrect;
                totalCorrect += spellCheckResult.Correct;
                GA.AddOutput("Incorrect", spellCheckResult.Incorrect, lineNumberString);
                GA.AddOutput("Correct", spellCheckResult.Correct, lineNumberString);
            }
            GA.AddOutput("Total Incorrect", totalIncorrect);
            GA.AddOutput("Total Correct", totalCorrect);
            GA.AddOutput("Total Words", totalCorrect + totalIncorrect);
        }
    }
}
