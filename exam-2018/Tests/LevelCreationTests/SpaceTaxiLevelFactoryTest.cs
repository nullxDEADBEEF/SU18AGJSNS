using System;
using System.IO;
using DIKUArcade;
using DIKUArcade.Utilities;
using exam_2018.LevelCreation;
using NUnit.Framework;

namespace Tests.LevelCreationTests {
    
    [TestFixture]
    public class SpaceTaxiLevelFactoryTest {
        private Window win;
        private SpaceTaxiLevelFactory testFactory;
        private DirectoryInfo dir;

        [SetUp]
        public void Init() {
            dir = new DirectoryInfo(Path.Combine(FileIO.GetProjectPath(), "Levels",
                "CorruptLevelsForTest"));
            win = new Window("factory test", 1, 1);
            testFactory = new SpaceTaxiLevelFactory();
            
        }

        [Test]
        public void InvalidMapThrowsException() {
            var filepath = Path.Combine(dir.FullName, "the-beach-invalid-map.txt");

            var ex = Assert.Throws<Exception>(
                () => testFactory.GetLevelFromFile(filepath));
            
            Assert.IsTrue(ex.Message.Contains("Level must be 40 lines x 23 characters."));
        }

        [Test]
        public void InvalidPlatformsThrowsException() {
            var filepath = Path.Combine(dir.FullName, "the-beach-invalid-platforms.txt");

            var ex = Assert.Throws<Exception>(
                () => testFactory.GetLevelFromFile(filepath));
            
            Assert.IsTrue(ex.Message.Contains("Level must contain platforms."));
        }

        [Test]
        public void MissingPlayerStartingPositionThrowsException() {
            var filepath = 
                Path.Combine(dir.FullName, "the-beach-invalid-player-starting-position.txt");

            var ex = Assert.Throws<Exception>(
                () => testFactory.GetLevelFromFile(filepath));
            
            Assert.IsTrue(ex.Message.Contains(
                "Level map must contain a starting position for player."));
        }

        [Test]
        public void MissingPortalsThrowsException() {
            var filepath = Path.Combine(dir.FullName, "the-beach-invalid-portals.txt");

            var ex = Assert.Throws<Exception>(
                () => testFactory.GetLevelFromFile(filepath));

            Assert.IsTrue(ex.Message.Contains("Level map must contain portal entities."));
        }
        
        [Test]
        public void MissingImageMappingThrowsException() {
            var filepath = Path.Combine(dir.FullName, "the-beach-invalid-image-mapping.txt");

            var ex = Assert.Throws<Exception>(
                () => testFactory.GetLevelFromFile(filepath));

            Assert.IsTrue(ex.Message.Contains("Level must contain image mapping."));
        }
    }
}