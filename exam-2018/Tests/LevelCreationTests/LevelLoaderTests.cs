using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DIKUArcade.Utilities;
using exam_2018.LevelCreation;
using NUnit.Framework;

namespace Tests.LevelCreationTests {
    [TestFixture]
    public class LevelLoaderTests {
        private LevelLoader levelLoader;
        private Dictionary<string, List<string>> levelDictionary;
        private Dictionary<string, List<string>> loadedLevelDictionary;

        [SetUp]
        public void Init() {
            levelLoader = new LevelLoader();

            levelDictionary = new Dictionary<string, List<string>>() {
                {"Name", new List<string>() {"Name: SHORT -N- SWEET"}},

                {"Platforms", new List<string>() {"Platforms: 1"}}, {
                    "Images", new List<string>() {
                        "%) white-square.png",
                        "#) ironstone-square.png",
                        "1) neptune-square.png",
                        "2) green-square.png",
                        "3) yellow-stick.png",
                        "o) purple-circle.png",
                        "G) green-upper-left.png",
                        "H) green-upper-right.png",
                        "g) green-lower-left.png",
                        "h) green-lower-right.png",
                        "I) ironstone-upper-left.png",
                        "J) ironstone-upper-right.png",
                        "i) ironstone-lower-left.png",
                        "j) ironstone-lower-right.png",
                        "N) neptune-upper-left.png",
                        "M) neptune-upper-right.png",
                        "n) neptune-lower-left.png",
                        "m) neptune-lower-right.png",
                        "W) white-upper-left.png",
                        "X) white-upper-right.png",
                        "w) white-lower-left.png",
                        "x) white-lower-right.png"
                    }
                },

                {"Customers", new List<string>() {"Customer: Alice 10 1 ^J 10 100"}}, {
                    "Map", new List<string>() {
                        "%#%#%#%#%#%#%#%#%#^^^^^^%#%#%#%#%#%#%#%#",
                        "#               JW      JW             %",
                        "%      h2g                             #",
                        "#      222                     >       %",
                        "%      H2G                        o    #",
                        "#       3                           o  %",
                        "%       3                              #",
                        "#       3                           o  %",
                        "%       3                       j%i    #",
                        "#       3                       W Xi   %",
                        "%       3                          %   #",
                        "#                                 xI   %",
                        "%    o                           xI    #",
                        "#                               xI     %",
                        "%                              xI      #",
                        "#  o   o                      xI       %",
                        "%                            xI        #",
                        "#    o                      xI         %",
                        "%      o                   xI          #",
                        "#  o                      xI           %",
                        "%       o                 I            #",
                        "#         m1111111111111n              %",
                        "%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#"
                    }
                }
            };
            var dir = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            dir = dir.Parent.Parent;

            loadedLevelDictionary =
                levelLoader.ReadFileContents(dir.FullName + "/Levels/short-n-sweet.txt");
        }

        /// <summary>
        /// Tests if the LevelLoader returns the expected name value.
        /// </summary>
        [Test]
        public void ParsedNameStringsAreCorrect() {

    //Asserting index 0 as level name does not span several lines
            Assert.AreEqual(loadedLevelDictionary["Name"][0], levelDictionary["Name"][0]);
        }

        /// <summary>
        /// Tests if the LevelLoader returns the expected platform characters.
        /// </summary>
        [Test]
        public void ParsedPlatformStringsAreCorrect() {
            StringBuilder comparisonStrings = new StringBuilder();
            foreach (string s in levelDictionary["Platforms"]) {
                comparisonStrings.Append(s);
            }
            StringBuilder loadedStrings = new StringBuilder();
            foreach (string s in loadedLevelDictionary["Platforms"]) {
                loadedStrings.Append(s);
            }
            Assert.AreEqual(comparisonStrings.ToString(), loadedStrings.ToString());
        }

        /// <summary>
        /// Tests if LevelLoader returns the expected image filename strings.
        /// </summary>
        [Test]
        public void ParsedImageFileStringsAreCorrect() {
            StringBuilder comparisonStrings = new StringBuilder();
            foreach (string s in levelDictionary["Images"]) {
                comparisonStrings.Append(s);
            }
            StringBuilder loadedStrings = new StringBuilder();
            foreach (string s in loadedLevelDictionary["Images"]) {
                loadedStrings.Append(s);
            }
            Assert.AreEqual(comparisonStrings.ToString(), loadedStrings.ToString());
        }

        /// <summary>
        /// Tests if LevelLoader returns the expected map strings for a loaded level.
        /// </summary>
        [Test]
        public void ParsedMapStringsAreCorrect() {
            StringBuilder comparisonStrings = new StringBuilder();
            foreach (string s in levelDictionary["Map"]) {
                comparisonStrings.Append(s);
            }
            StringBuilder loadedStrings = new StringBuilder();
            foreach (string s in loadedLevelDictionary["Map"]) {
                loadedStrings.Append(s);
            }
            Assert.AreEqual(comparisonStrings.ToString(), loadedStrings.ToString());
        }

        /// <summary>
        /// Tests if LevelLoader returns the expected customer strings.
        /// </summary>
        [Test]
        public void ParsedCustomerStringsAreCorrect() {
            StringBuilder comparisonStrings = new StringBuilder();
            foreach (string s in levelDictionary["Customers"]) {
                comparisonStrings.Append(s);
            }
            StringBuilder loadedStrings = new StringBuilder();
            foreach (string s in loadedLevelDictionary["Customers"]) {
                loadedStrings.Append(s);
            }
            Assert.AreEqual(comparisonStrings.ToString(), loadedStrings.ToString());
        }

        [Test]
        public void MissingMapThrowsException() {
            var dir = new DirectoryInfo(
                Path.Combine(FileIO.GetProjectPath(), "Levels", "CorruptLevelsForTest"));

            var ex = Assert.Throws<Exception>(
                () => loadedLevelDictionary =
                    levelLoader.ReadFileContents(
                        Path.Combine(dir.FullName, "the-beach-missing-map.txt")));
            
            Assert.IsTrue(ex.Message.Contains("Level file must contain level map"));
        }
        
        [Test]
        public void MissingPlatformsThrowsException() {
            var dir = new DirectoryInfo(
                Path.Combine(FileIO.GetProjectPath(), "Levels", "CorruptLevelsForTest"));

            var ex = Assert.Throws<Exception>(
                () => loadedLevelDictionary =
                    levelLoader.ReadFileContents(
                        Path.Combine(dir.FullName, "the-beach-missing-platforms.txt")));
            
            Assert.IsTrue(ex.Message.Contains("Level file must contain platform string(s)"));
        }
        
        [Test]
        public void MissingImagesThrowsException() {
            var dir = new DirectoryInfo(
                Path.Combine(FileIO.GetProjectPath(), "Levels", "CorruptLevelsForTest"));

            var ex = Assert.Throws<Exception>(
                () => loadedLevelDictionary =
                    levelLoader.ReadFileContents(
                        Path.Combine(dir.FullName, "the-beach-missing-images.txt")));
            
            Assert.IsTrue(ex.Message.Contains(
                "Level file must contain character-image mapping(s)"));
        }
        
        [Test]
        public void MissingCustomersThrowsException() {
            var dir = new DirectoryInfo(
                Path.Combine(FileIO.GetProjectPath(), "Levels", "CorruptLevelsForTest"));

            var ex = Assert.Throws<Exception>(
                () => loadedLevelDictionary =
                    levelLoader.ReadFileContents(
                        Path.Combine(dir.FullName, "the-beach-missing-customers.txt")));
            
            Assert.IsTrue(ex.Message.Contains("Level file must contain customer string(s)"));
        }
        
        [Test]
        public void MissingNameThrowsException() {
            var dir = new DirectoryInfo(
                Path.Combine(FileIO.GetProjectPath(), "Levels", "CorruptLevelsForTest"));

            var ex = Assert.Throws<Exception>(
                () => loadedLevelDictionary =
                    levelLoader.ReadFileContents(
                        Path.Combine(dir.FullName, "the-beach-missing-name.txt")));
            
            Assert.IsTrue(ex.Message.Contains("Level file must contain name string(s)"));
        }
    }
}