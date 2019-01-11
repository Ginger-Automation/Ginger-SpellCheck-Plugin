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
            spellCheckService.SpellCheckBitmap(GA, filename);

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
            spellCheckService.SpellCheckBitmap(GA, filename);

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
            spellCheckService.SpellCheckBitmap(GA, filename);

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
            spellCheckService.SpellCheckBitmap(GA, filename);

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
            spellCheckService.SpellCheckBitmap(GA, filename);

            // Assert
            Assert.AreNotEqual(null, GA.Errors, "null != GA.Errors");
        }

        [TestMethod]
        public void TestGreenSpell()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA = new GingerAction();
            string filename = TestResources.GetTestResourcesFile("TestGreenBackg.png");

            // Act
            spellCheckService.SpellCheckBitmap(GA, filename);
            int Incorrect = (from x in GA.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();

            // Assert
            Assert.AreEqual(null, GA.Errors, "Errors=null");            
            Assert.AreNotEqual("0", Incorrect, "0 = Incorrect");
        }

        [TestMethod]
        public void TestSpellCheckTextAllCorrect()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA1 = new GingerAction();
            string textCorrect = "Everything here is spelled correctly";

            // Act
            spellCheckService.SpellCheckText(GA1, textCorrect);
            int IncorrectGA1 = (from x in GA1.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            int CorrectGA1 = (from x in GA1.Output.OutputValues where x.Param == "Correct" select (int)x.Value).SingleOrDefault();

            // Assert
            Assert.AreEqual(null, GA1.Errors, "Errors=null");
            Assert.AreEqual(0, IncorrectGA1, "0 = Incorrect");
            Assert.AreEqual(5, CorrectGA1, "5 = Correct");
        }

        [TestMethod]
        public void TestSpellCheckTextAllCorrectUpperLowerMix()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA2 = new GingerAction();
            string textCorrect2 = "EvEryThInG hErE Is sPeLlEd cOrReCtLy ToO";

            // Act
            spellCheckService.SpellCheckText(GA2, textCorrect2);
            int IncorrectGA2 = (from x in GA2.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            int CorrectGA2 = (from x in GA2.Output.OutputValues where x.Param == "Correct" select (int)x.Value).SingleOrDefault();

            // Assert
            Assert.AreEqual(null, GA2.Errors, "Errors=null");
            Assert.AreEqual(0, IncorrectGA2, "0 = Incorrect2");
            Assert.AreEqual(6, CorrectGA2, "6 = Correct2");
        }

        [TestMethod]
        public void TestSpellCheckTextAllIncorrect()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA3 = new GingerAction();            
            string textIncorrect = "Evirytheng hir ees spelld encorrictly";           

            // Act
            spellCheckService.SpellCheckText(GA3, textIncorrect);            
            int IncorrectGA3 = (from x in GA3.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();           
            int CorrectGA3 = (from x in GA3.Output.OutputValues where x.Param == "Correct" select (int)x.Value).SingleOrDefault();            

            // Assert            
            Assert.AreEqual(null, GA3.Errors, "Errors=null");               
            Assert.AreEqual(5, IncorrectGA3, "5 = Incorrect1");
            Assert.AreEqual(0, CorrectGA3, "0 = Correct1");           
        }

        [TestMethod]
        public void TestSpellCheckTextAllIncorrectUpperLowerMix()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();            
            GingerAction GA4 = new GingerAction();
            string textIncorrect2 = "EviryThenG hir ees sPeLld encOrRiCtLy Tuu";

            // Act            
            spellCheckService.SpellCheckText(GA4, textIncorrect2);
            int IncorrectGA4 = (from x in GA4.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            int CorrectGA4 = (from x in GA4.Output.OutputValues where x.Param == "Correct" select (int)x.Value).SingleOrDefault();

            // Assert
            Assert.AreEqual(null, GA4.Errors, "Errors=null");
            Assert.AreEqual(6, IncorrectGA4, "6 = Incorrect22");
            Assert.AreEqual(0, CorrectGA4, "0 = Correct22");
        }

        [TestMethod]
        public void TestSpellCheckTextMixed()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA5 = new GingerAction();
            string mixed = "Right rong nutright rIgHt comma,should, be ,right";

            // Act            
            spellCheckService.SpellCheckText(GA5, mixed);
            int IncorrectGA5 = (from x in GA5.Output.OutputValues where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            int CorrectGA5 = (from x in GA5.Output.OutputValues where x.Param == "Correct" select (int)x.Value).SingleOrDefault();

            // Assert
            Assert.AreEqual(null, GA5.Errors, "Errors=null");
            Assert.AreEqual(2, IncorrectGA5, "2 = IncorrectMixed");
            Assert.AreEqual(6, CorrectGA5, "6 = CorrectMixed");
        }

        [TestMethod]
        public void TestSpellCheckTextFile()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA = new GingerAction();
            string filename = TestResources.GetTestResourcesFile("TestTextFile.txt");

            // Act    
            spellCheckService.SpellCheckTextFile(GA, filename);
            int IncorrectLine1 = (from x in GA.Output.OutputValues where x.Path == "Line 1" where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            int CorrectLine1 = (from x in GA.Output.OutputValues where x.Path == "Line 1" where x.Param == "Correct" select (int)x.Value).SingleOrDefault();
            int IncorrectLine5 = (from x in GA.Output.OutputValues where x.Path == "Line 5" where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            int CorrectLine5 = (from x in GA.Output.OutputValues where x.Path == "Line 5" where x.Param == "Correct" select (int)x.Value).SingleOrDefault();

            // Assert
            Assert.AreEqual(null, GA.Errors, "Errors=null");
            Assert.AreEqual(0, IncorrectLine1, "0 = IncorrectLine1");
            Assert.AreEqual(4, CorrectLine1, "4 = CorrectLine1");
            Assert.AreEqual(4, IncorrectLine5, "4 = IncorrectLine5");
            Assert.AreEqual(0, CorrectLine5, "0 = CorrectLine5");
        }

        [TestMethod]
        public void SpellCheckImage2()
        {
            //Arrange
            SpellCheckService spellCheckService = new SpellCheckService();
            GingerAction GA = new GingerAction();
            string filename = TestResources.GetTestResourcesFile("BigBog.png");
            string filename2 = TestResources.GetTestResourcesFile("TestGreenBackg.png");

            // Act            
            spellCheckService.SpellCheckAndReturnBitmap(GA, filename);
            spellCheckService.SpellCheckAndReturnBitmap(GA, filename2);
            //int IncorrectLine1 = (from x in GA.Output.OutputValues where x.Path == "Line: 1" where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            //int CorrectLine1 = (from x in GA.Output.OutputValues where x.Path == "Line: 1" where x.Param == "Correct" select (int)x.Value).SingleOrDefault();
            //int IncorrectLine5 = (from x in GA.Output.OutputValues where x.Path == "Line: 5" where x.Param == "Incorrect" select (int)x.Value).SingleOrDefault();
            //int CorrectLine5 = (from x in GA.Output.OutputValues where x.Path == "Line: 5" where x.Param == "Correct" select (int)x.Value).SingleOrDefault();

            // Assert
            //Assert.AreEqual(null, GA.Errors, "Errors=null");
            //Assert.AreEqual(0, IncorrectLine1, "0 = IncorrectLine1");
            //Assert.AreEqual(4, CorrectLine1, "4 = CorrectLine1");
            //Assert.AreEqual(4, IncorrectLine5, "4 = IncorrectLine5");
            //Assert.AreEqual(0, CorrectLine5, "0 = CorrectLine5");
        }
    }
}
