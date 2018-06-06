using System.Collections.Generic;
using System.IO;
using System.Linq;
using DIKUArcade;
using DIKUArcade.Entities;
using exam_2018;
using exam_2018.GameEntities;
using exam_2018.LevelCreation;
using exam_2018;
using NUnit.Framework;

namespace Tests.LevelCreationTests {
    [TestFixture]

public class LevelParserTests {
        private Window win;
        private LevelLoader levelLoader;
        private LevelParser levelParser;
        private Dictionary<string, List<string>> levelDictionary;

        [SetUp]
        public void Init() {
            win = new Window("test", 1, 1);
            levelLoader = new LevelLoader();
            levelParser = new LevelParser();
            var dir = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            dir = dir.Parent.Parent;
            //using level short-n-sweet
            levelDictionary =
                levelLoader.ReadFileContents(dir.FullName + "/Levels/short-n-sweet.txt");
        }

        [Test]
        public void TestParseCustomerStrings() {
            //expecting 1 customer in level file short-n-sweet.txt
            var expectedCustomer = new Customer(new Shape(), null) {
                Name = "Alice",
                NextLevel = true,
                WhenToSpawn = 10,
                Points = 100,
                TimeToDeliver = 10,
                PlatformId = '1',
                DestinationPlatformId = 'J'
            };
            var parsedCustomer =
                levelParser.ParseCustomerStrings(levelDictionary[Constants.CUSTOMERS]).First();
            
            Assert.IsTrue(
                (expectedCustomer.Name == parsedCustomer.Name &&
                 expectedCustomer.NextLevel == parsedCustomer.NextLevel &&
                 expectedCustomer.WhenToSpawn == parsedCustomer.WhenToSpawn &&
                 expectedCustomer.Points == parsedCustomer.Points &&
                 expectedCustomer.TimeToDeliver == parsedCustomer.TimeToDeliver &&
                 expectedCustomer.PlatformId == parsedCustomer.PlatformId &&
                 expectedCustomer.DestinationPlatformId == parsedCustomer.DestinationPlatformId));
        }

        [Test]
        public void TestParseLevelName() {
            var expectedLevelName = "SHORT -N- SWEET";
            var parsedName = levelParser.ParseLevelName(levelDictionary[Constants.NAME]);
            Assert.AreEqual(expectedLevelName, parsedName);
        }

        [Test]
        public void TestParsePlatformChars() {
            //Only one platform char in the chosen test level
            var expectedPlatformChar = '1';
            var parsedPlatformCharSet =
                levelParser.ParsePlatformChars(levelDictionary[Constants.PLATFORMS]);
            char parsedPlatformChar = '0';
            foreach (char c in parsedPlatformCharSet) {
                parsedPlatformChar = c;
            }
            
            Assert.AreEqual(expectedPlatformChar, parsedPlatformChar);
        }

        [Test]
        public void TestParseImagesCorrect() {
            var expectedImageDictionary = new Dictionary<char, string>() {
                {'%', "white-square.png"},
                {'#', "ironstone-square.png"},
                {'1', "neptune-square.png"},
                {'2', "green-square.png"},
                {'3', "yellow-stick.png"},
                {'o', "purple-circle.png"},
                {'G', "green-upper-left.png"},
                {'H', "green-upper-right.png"},
                {'g', "green-lower-left.png"},
                {'h', "green-lower-right.png"},
                {'I', "ironstone-upper-left.png"},
                {'J', "ironstone-upper-right.png"},
                {'i', "ironstone-lower-left.png"},
                {'j', "ironstone-lower-right.png"},
                {'N', "nepune-upper-left.png"},
                {'M', "neptune-upper-right.png"},
                {'n', "neptune-lower-left.png"},
                {'m', "neptune-lower-right.png"},
                {'W', "white-upper-left.png"},
                {'X', "white-upper-right.png"},
                {'w', "white-lower-left.png"},
                {'x', "white-lower-right.png"}
            };
            var parsedImageDictionary = levelParser.ParseImages(levelDictionary[Constants.IMAGES]);

            var expectedCharCombination = "";
            foreach (var val in expectedImageDictionary) {
                expectedCharCombination += val.Key;
            }
            var parsedCharCombination = "";
            foreach (var val in parsedImageDictionary) {
                parsedCharCombination += val.Key;
            }
            Assert.AreEqual(expectedCharCombination, parsedCharCombination);
        }
    }
}