using System.IO;
using DIKUArcade.Utilities;

namespace exam_2018.LevelCreation {
    
    /// <summary>
    /// Class responsible for managing levels
    /// </summary>
    public class LevelManager{
        private static LevelManager instance;
        private ISpaceTaxiLevelFactory spaceTaxiLevelFactory;
        public FileInfo[] LevelInfos { get;}
        public Level ActiveLevel { get; private set; }
        public int ActiveLevelIndex;

        public static LevelManager GetInstance() {
            return LevelManager.instance ?? (LevelManager.instance = new LevelManager());
        }
        
        private LevelManager() {
            ActiveLevelIndex = 0;
            LevelInfos = GetLevelInfos();
            spaceTaxiLevelFactory = new SpaceTaxiLevelFactory();
        }
        
        /// <summary>
        /// Changes the active level of the levelManager to a desired level
        /// </summary>
        /// <param name="levelInfo">FileInfo representing a Space Taxi level</param>
        public void BuildLevel(FileInfo levelInfo) {
            if (ActiveLevel == null) {
                ActiveLevel = spaceTaxiLevelFactory.GetLevelFromFile(levelInfo.FullName);
            } else {
                var newLevel = spaceTaxiLevelFactory.GetLevelFromFile(levelInfo.FullName);
                newLevel.CustomerDespawnEvents = ActiveLevel.CustomerDespawnEvents;
                newLevel.PlayerScore = ActiveLevel.PlayerScore;
                if (ActiveLevel.CurrentlyCarriedCustomer != null) {
                    newLevel.CurrentlyCarriedCustomer = ActiveLevel.CurrentlyCarriedCustomer;
                    newLevel.CurrentlyCarriedCustomer.NextLevel = false;
                }

                ActiveLevel = newLevel;
            }
        }
        
        /// <summary>
        /// Finds all level files in *project name*/Levels
        /// </summary>
        /// <returns></returns>
        private FileInfo[] GetLevelInfos() {
            var projectDirectory = new DirectoryInfo(FileIO.GetProjectPath());
            var levelDirectory = new DirectoryInfo(
                Path.Combine(projectDirectory.FullName + "/Levels"));

            return levelDirectory.GetFiles();
        }

        /// <summary>
        /// Cycles through the LevelInfos FileInfo[] array to find the level at the index of
        /// ActiveLevelIndex+1. If this is out of bounds, it loops back to the first level in the
        /// LevelInfos array.
        /// </summary>
        public void NextLevel() {
            ActiveLevelIndex++;
            if (ActiveLevelIndex > LevelInfos.Length - 1) {
                ActiveLevelIndex = 0;
            }
            BuildLevel(LevelInfos[ActiveLevelIndex]);
        }
    }
}