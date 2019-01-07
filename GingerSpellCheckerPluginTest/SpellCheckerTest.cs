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
    }
}
