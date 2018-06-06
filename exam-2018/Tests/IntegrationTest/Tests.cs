using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using exam_2018.GameEntities;
using exam_2018.LevelCreation;
using NUnit.Framework;


namespace Tests.IntegrationTest
{
  [TestFixture]
  public class Test 
  {
    private Window win;
    private LevelManager levelMan;
    private LevelLoader levelLoader;
    private LevelParser levelParser;
    private Dictionary<string, List<string>> levelDictionary;
    private Dictionary<string, List<string>> loadedLevelDictionary;
    private ImageContainer imageContainer;
    private Dictionary<char, Platform> PlatformDictionary;
    private EntityContainer<Obstacle> Obstacles;

    [SetUp]
    public void Init() {
      win = new Window("LevelCreationTest", 200, AspectRatio.R1X1);
      levelLoader = new LevelLoader();
      levelParser = new LevelParser();
      var dir = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
      dir = dir.Parent.Parent;
      levelDictionary =
        levelLoader.ReadFileContents(dir.FullName + "/Levels/short-n-sweet.txt");
      levelMan = LevelManager.GetInstance();
      levelMan.BuildLevel(levelMan.LevelInfos[0]);
      levelLoader = new LevelLoader();
      PlatformDictionary = new Dictionary<char, Platform>();
      
    

    }

    [Test]
    public void Test1() {
      var expectedActiveLevelName = "SHORT -N- SWEET";
      var ActiveLevel = levelMan.ActiveLevel;
      var actualActiveLevelName = ActiveLevel.Name;
      var expectedNumberOfPlatforms = '1';
      var parsedPlatformCharSet = levelParser.ParsePlatformChars(levelDictionary["Platforms"]);
      char parsedPlatformChar = '0';     
      foreach (char c in parsedPlatformCharSet) {
        parsedPlatformChar = c;
      }

      Assert.AreEqual(expectedActiveLevelName,actualActiveLevelName);
      Assert.AreEqual(expectedNumberOfPlatforms,parsedPlatformChar);
      
       
    }




    }
  }

