using Amdocs.Ginger.Plugin.Core;
using GingerSpellCheckerPlugin;
using GingerTestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace GingerSpellCheckerPluginTest
{
    [TestClass]
    public class SpellCheckerTest
    {
        [TestMethod]
        public void TestBigBogScan()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA = new GingerAction();
            string filename = TestResources.GetTestResourcesFile("BigBog.png");

            // Act
            spellCheckService.SpellCheckWord(GA, filename);

            // Assert
            Assert.AreEqual(null, GA.Errors, "Errors=null");
            Assert.AreEqual("Test", GA.Output.OutputValues[3].Param, "Test = GA.Output.OutputValues[3].Param");
            Assert.AreEqual("BIG", GA.Output.OutputValues[4].Param, "BIG = GA.Output.OutputValues[4].Param");
            Assert.AreEqual("Test", GA.Output.OutputValues[5].Param, "Test = GA.Output.OutputValues[5].Param");
            Assert.AreEqual("BOG", GA.Output.OutputValues[6].Param, "BOG = GA.Output.OutputValues[6].Param");
        }

        [TestMethod]
        public void TestFileNotFound()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA = new GingerAction();
            string filename = "igBog.png";

            // Act
            spellCheckService.SpellCheckWord(GA, filename);

            // Assert
            Assert.AreEqual("Could not find the file: '" + filename+ "'", GA.Errors, 
                "Could not find the file: '" + filename + "'" + "= GA.Errors");
        }

        [TestMethod]
        public void TestWhiteScan()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA = new GingerAction();
            string filename = TestResources.GetTestResourcesFile("TestWhite.png");

            // Act
            spellCheckService.SpellCheckWord(GA, filename);

            // Assert
            Assert.AreEqual("Could not find any text", GA.ExInfo, "Could not find any text = GA.Errors");
        }

        [DataRow("BigBog.jpg")]
        [DataRow("BigBog.bmp")]
        [DataRow("BigBog.gif")]
        [DataRow("BigBog.tif")]
        [DataRow("BigBog.png")]
        [TestMethod]
        public void TestBigBogDifferentTypesScan(string fileName)
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA = new GingerAction();
            
            string filename = TestResources.GetTestResourcesFile(fileName);
            
            // Act
            spellCheckService.SpellCheckWord(GA, filename);

            // Assert
            Assert.AreEqual(null, GA.Errors, "Errors=null");
            Assert.AreEqual("Test", GA.Output.OutputValues[3].Param, "Test = GA.Output.OutputValues[3].Param");
            Assert.AreEqual("BIG", GA.Output.OutputValues[4].Param, "BIG = GA.Output.OutputValues[4].Param");
            Assert.AreEqual("Test", GA.Output.OutputValues[5].Param, "Test = GA.Output.OutputValues[5].Param");
            Assert.AreEqual("BOG", GA.Output.OutputValues[6].Param, "BOG = GA.Output.OutputValues[6].Param");
        }

        [TestMethod]
        public void TestNotBitmapScan()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA = new GingerAction();
            string filename = TestResources.GetTestResourcesFile("Todo_SpellChecker.txt");

            // Act
            spellCheckService.SpellCheckWord(GA, filename);

            // Assert

            Assert.AreEqual("Could not find any text", GA.ExInfo, "Could not find any text = GA.ExInfo");
        }

        [TestMethod]
        public void TestGreenSpell()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA = new GingerAction();
            string filename = TestResources.GetTestResourcesFile("TestGreenBackg.png");

            // Act
            spellCheckService.SpellCheckWord(GA, filename);
            int Incorrect = (from x in GA.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();

            // Assert
            Assert.AreEqual(null, GA.Errors, "Errors=null");            
            Assert.AreNotEqual("0", Incorrect, "0 = Incorrect");
        }

        [TestMethod]
        public void TestSpellCheckText()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA1 = new GingerAction();
            GingerAction GA2 = new GingerAction();
            GingerAction GA3 = new GingerAction();
            GingerAction GA4 = new GingerAction();
            GingerAction GA5 = new GingerAction();
            string textCorrect = "Everything here is spelled correctly";
            string textCorrect2 = "EvEryThInG hErE Is sPeLlEd cOrReCtLy ToO";
            string textIncorrect = "Evirytheng hir ees spelld encorrictly";
            string textIncorrect2 = "EviryThenG hir ees sPeLld encOrRiCtLy Tuu";
            string mixed = "Right rong nutright rIgHt comma,should, be ,right";

            // Act
            spellCheckService.SpellCheckText(GA1, textCorrect);
            spellCheckService.SpellCheckText(GA2, textCorrect2);
            spellCheckService.SpellCheckText(GA3, textIncorrect);
            spellCheckService.SpellCheckText(GA4, textIncorrect2);
            spellCheckService.SpellCheckText(GA5, mixed);
            int IncorrectGA1 = (from x in GA1.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            int IncorrectGA2 = (from x in GA2.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            int IncorrectGA3 = (from x in GA3.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            int IncorrectGA4 = (from x in GA4.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            int IncorrectGA5 = (from x in GA5.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            int CorrectGA1 = (from x in GA1.Output.OutputValues where x.Param == "Correct" select (int)x.Value).SingleOrDefault();
            int CorrectGA2 = (from x in GA2.Output.OutputValues where x.Param == "Correct" select (int)x.Value).SingleOrDefault();
            int CorrectGA3 = (from x in GA3.Output.OutputValues where x.Param == "Correct" select (int)x.Value).SingleOrDefault();
            int CorrectGA4 = (from x in GA4.Output.OutputValues where x.Param == "Correct" select (int)x.Value).SingleOrDefault();
            int CorrectGA5 = (from x in GA5.Output.OutputValues where x.Param == "Correct" select (int)x.Value).SingleOrDefault();

            // Assert
            Assert.AreEqual(null, GA1.Errors, "Errors=null");
            Assert.AreEqual(null, GA2.Errors, "Errors=null");
            Assert.AreEqual(null, GA3.Errors, "Errors=null");
            Assert.AreEqual(null, GA4.Errors, "Errors=null");
            Assert.AreEqual(null, GA5.Errors, "Errors=null");
            Assert.AreEqual(0, IncorrectGA1, "0 = Incorrect");
            Assert.AreEqual(0, IncorrectGA2, "0 = Incorrect2");
            Assert.AreEqual(5, IncorrectGA3, "5 = Incorrect1");
            Assert.AreEqual(6, IncorrectGA4, "6 = Incorrect22");
            Assert.AreEqual(2, IncorrectGA5, "2 = IncorrectMixed");
            Assert.AreEqual(5, CorrectGA1, "5 = Correct");
            Assert.AreEqual(6, CorrectGA2, "6 = Correct2");
            Assert.AreEqual(0, CorrectGA3, "0 = Correct1");
            Assert.AreEqual(0, CorrectGA4, "0 = Correct22");
            Assert.AreEqual(6, CorrectGA5, "6 = CorrectMixed");
        }
    }
}
