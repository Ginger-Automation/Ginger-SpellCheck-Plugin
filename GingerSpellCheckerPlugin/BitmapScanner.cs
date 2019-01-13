using System;
using System.Collections.Generic;
using System.Drawing;
using Tesseract;

namespace GingerSpellCheckerPlugin
{
    public class BitmapScanner
    {
        string mFilePath;
        Pix mPix;
        Bitmap mBitmap;
        Bitmap mBitmapWithHighlight;
        List<TextBox> mTextBoxes;
        int mIncorrectCounter;
        int mCorrectCounter;
        // static TesseractEngine mEngine;
        TesseractEngine mEngine;
        static SpellCheck mSpellCheck;
        Brush pinkBrush = new SolidBrush(Color.FromArgb(alpha: 128, red: 255, green: 40, blue: 150));

        public Bitmap Bitmap
        {
            get
            {
                if (mBitmap == null)
                {
                    mBitmap = new Bitmap(mFilePath);
                }
                return mBitmap;
            }
        }
        public Bitmap BitmapWithHightlight { get { return mBitmapWithHighlight; } }
        public string FilePath { get { return mFilePath; } }
        public List<TextBox> TextBoxes { get { return mTextBoxes; } }
        public int IncorrectCounter { get { return mIncorrectCounter; } }
        public int CorrectCounter { get { return mCorrectCounter; } }
        

        public BitmapScanner(string filePath)
        {
            mFilePath = filePath;
            if (mEngine == null)
            {
                mEngine = new TesseractEngine(@"./tessdata", "eng");
            }
        }

        public void ScanTextBoxes()
        {
            mTextBoxes = new List<TextBox>();
            mPix = Pix.LoadFromFile(mFilePath);
            
            PageIteratorLevel myLevel = PageIteratorLevel.Word;
            using (Page page = mEngine.Process(mPix))
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
                            TextBox textBox = new TextBox() { text = curText, xPosition = posX, yPosition = posY,
                                                                height = height, width = width};
                            mTextBoxes.Add(textBox);                            
                        }
                    } while (iter.Next(myLevel));
                }
            }
        }

        public void DoSpellCheck()
        {
            if (mSpellCheck == null)
            {
                mSpellCheck = new SpellCheck();
            }
            mIncorrectCounter = 0;
            mCorrectCounter = 0;
            foreach (TextBox textBox in TextBoxes)
            {
                textBox.isCorrectSpelling = mSpellCheck.Check(textBox.text);
                textBox.suggestion = string.Join(',',mSpellCheck.Suggest(textBox.text));
                if (textBox.isCorrectSpelling)
                {
                    mCorrectCounter++;
                }
                else
                {
                    mIncorrectCounter++;
                }
            }
        }

        public void CreateHighlightedImage()
        {
            mBitmapWithHighlight = new Bitmap(this.Bitmap);
            foreach (TextBox text in mTextBoxes)
            {
                if (!text.isCorrectSpelling)
                {
                    using (Graphics g = Graphics.FromImage(mBitmapWithHighlight))
                    {
                        g.FillRectangle(pinkBrush, text.xPosition, text.yPosition, text.width, text.height);
                    }
                }
            }
        }
    }
}