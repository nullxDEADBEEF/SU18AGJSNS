using System;
using DIKUArcade;
using exam_2018.LevelCreation;
using NUnit.Framework;

namespace Tests.LevelCreationTests {
    [TestFixture]
    public class LevelManagerTests {
        private Window win;
        private LevelManager levelManager;

        [SetUp]
        public void Init() {
            win = new Window("LevelManagerTests", 200, AspectRatio.R1X1);
            levelManager = LevelManager.GetInstance();
            levelManager.BuildLevel(levelManager.LevelInfos[0]);
            
        }

        /// <summary>
        /// Should test if the active level is changed if BuildLevel() is called by checking the
        /// name of the level compared to the expected name
        /// </summary>
        [Test]
        public void TestBuildLevel() {
            Console.WriteLine(levelManager.LevelInfos.ToString());
            var expectedActiveLevelName = "SHORT -N- SWEET";
            var ActiveLevel = levelManager.ActiveLevel;
            var actualActiveLevelName = ActiveLevel.Name;
            
            Assert.AreEqual(expectedActiveLevelName, actualActiveLevelName);
        }
    }
}