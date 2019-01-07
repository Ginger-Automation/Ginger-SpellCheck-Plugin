using System;
using System.Collections.Generic;
using Tesseract;

namespace GingerSpellCheckerPlugin
{
    internal class BitmapScanner
    {
        public List<TextBox> getTextBoxes(string filePath, bool checkSpelling, out int incorrectCounter)
        {
            List<TextBox> textBoxes = new List<TextBox>();
            incorrectCounter = 0;
            Pix img = Pix.LoadFromFile(filePath);
            TesseractEngine Engine = new TesseractEngine(@"./tessdata", "eng");
            PageIteratorLevel myLevel = PageIteratorLevel.Word;
            using (Page page = Engine.Process(img))
            {
                using (ResultIterator iter = page.GetIterator())
                {
                    iter.Begin();
                    do
                    {
                        Rect rect;
                        if (iter.TryGetBoundingBox(myLevel, out rect))
                        {
                            string curText = iter.GetText(myLevel);
                            int posX = rect.X1;
                            int posY = rect.Y1;
                            int height = rect.Height;
                            int width = rect.Width;
                            bool correct = false;
                            string suggested = "";
                            if (checkSpelling)
                            {
                                SpellCheck spellCheck = new SpellCheck();
                                correct = spellCheck.Check(curText, out suggested);
                            }
                            TextBox textBox = new TextBox() { text = curText, xPosition = posX, yPosition = posY,
                                height = height, width = width, isCorrectSpelling = correct, suggestion = suggested};
                            textBoxes.Add(textBox);
                            if (!correct)
                            {
                                incorrectCounter++;
                            }
                        }
                    } while (iter.Next(myLevel));
                }
            }
            return textBoxes;
        }
    }
}