using System;
using System.Collections.Generic;

namespace exam_2018.LevelCreation {
    public class LevelLoader : ILevelLoader{
        /// <summary>
        /// Reads the contents of a Space Taxi level file
        /// </summary>
        /// <param name="filepath">Full path of the file to be opened</param>
        /// <returns>Returns contents of level file as a dictionary</returns>
        public Dictionary<string, List<string>> ReadFileContents(string filepath) {
            var levelFileContents = new Dictionary<string, List<string>>() {
                {Constants.NAME, new List<string>()},
                {Constants.PLATFORMS, new List<string>()},
                {Constants.IMAGES, new List<string>()},
                {Constants.CUSTOMERS, new List<string>()},
                {Constants.MAP, new List<string>()}
            };
            var openedFile = System.IO.File.OpenText(filepath);
            while (openedFile.Peek() != -1) {
                var currentLine = openedFile.ReadLine();
                if (currentLine.StartsWith("Name")) {
                    levelFileContents[Constants.NAME].Add(currentLine);
                }else if (currentLine.StartsWith("Platforms")) {
                    levelFileContents[Constants.PLATFORMS].Add(currentLine);
                }else if (currentLine.EndsWith(".png")) {
                    levelFileContents[Constants.IMAGES].Add(currentLine);
                }else if (currentLine.StartsWith("Customer")) {
                    levelFileContents[Constants.CUSTOMERS].Add(currentLine);
                }else if (!String.IsNullOrWhiteSpace(currentLine)) {
                    levelFileContents[Constants.MAP].Add(currentLine);
                }
            }
            openedFile.Close();
            
            //checking if level data exists
            if (levelFileContents[Constants.NAME].Count < 1) {
                throw new Exception("Level file must contain name string(s)");
            } else if (levelFileContents[Constants.PLATFORMS].Count < 1) {
                throw new Exception("Level file must contain platform string(s)");
            } else if (levelFileContents[Constants.IMAGES].Count < 1) {
                throw new Exception("Level file must contain character-image mapping(s)");
            } else if (levelFileContents[Constants.CUSTOMERS].Count < 1) {
                throw new Exception("Level file must contain customer string(s)");
            } else if (levelFileContents[Constants.MAP].Count < 1) {
                throw new Exception("Level file must contain level map");
            }
            
            return levelFileContents;
        }
    }
}