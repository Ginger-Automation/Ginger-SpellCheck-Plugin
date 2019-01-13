namespace GingerSpellCheckerPlugin
{
    public class TextBox
    {
        public string text { get; set; }
        public int xPosition { get; set; }
        public int yPosition { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool isCorrectSpelling;
        public string suggestion { get; set; }
    }
}