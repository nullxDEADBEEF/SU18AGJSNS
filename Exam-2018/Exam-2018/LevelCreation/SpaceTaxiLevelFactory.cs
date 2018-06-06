using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using Exam_2018.GameEntities;

namespace Exam_2018.LevelCreation {
    public class SpaceTaxiLevelFactory : ISpaceTaxiLevelFactory{
        private ILevelLoader levelLoader;
        private ILevelParser levelParser;
        private ImageContainer imageContainer;

        public SpaceTaxiLevelFactory() {
            levelLoader = new LevelLoader();
            levelParser = new LevelParser();
            imageContainer = ImageContainer.GetInstance();
        }

        /// <summary>
        /// Builds a level object from the given level file path.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public Level GetLevelFromFile(string filepath) {
            var levelContent = levelLoader.ReadFileContents(filepath);

            //parsing map
            var levelMap = levelContent[Constants.MAP];
            if (levelMap.Count != 23 || levelMap[0].Length != 40) {
                throw new Exception(
                    "Error building level. Level must be 40 lines x 23 characters.");
            }
            
            var platformIds = levelParser.ParsePlatformChars(levelContent[Constants.PLATFORMS]);
            if (platformIds.Count < 1) {
                throw new Exception("Error building level. Level must contain platforms.");
            }
            
            var levelImages = levelParser.ParseImages(levelContent[Constants.IMAGES]);
            if (levelImages.Count < 1) {
                throw new Exception("Error building level. Level must contain image mapping.");
            }
            
            Dictionary<char, Platform> platformDictionary = new Dictionary<char, Platform>();
            foreach (char platformChar in platformIds) {
                platformDictionary.Add(platformChar, new Platform(platformChar));
            }
            EntityContainer<Obstacle> obstacles = new EntityContainer<Obstacle>();
            EntityContainer portals = new EntityContainer();
            
            Vec2F extent = new Vec2F(Constants.EXTENT_X, Constants.EXTENT_Y);

            Vec2F playerStartingPosition = null;
            Player player = new Player(null, null);

            //parsing level map
            int i = 0;
            foreach (string mapString in levelMap) {
                for (int j = 0; j < mapString.Length; j++) {
                    char mapChar = mapString[j];
                    Vec2F position = 
                        new Vec2F(
                            j * Constants.EXTENT_X, 
                            1 - i *Constants.EXTENT_Y - Constants.EXTENT_Y
                            );
                    
                    StationaryShape shape = new StationaryShape(position, extent);
                    if (mapChar == ' ') {
                        continue;
                    }

                    //checking the individual map chars
                    if (platformIds.Contains(mapString[j])) {
                        platformDictionary[mapChar].AddPlatform(
                            shape, imageContainer.GetImageByName(levelImages[mapChar]));

                    } else if (mapChar == '^') {
                        portals.AddStationaryEntity(shape, null);

                    } else if (mapChar == '<' || mapChar == '>') {
                        playerStartingPosition = 
                            new Vec2F(j * Constants.EXTENT_X, 1 - i * Constants.EXTENT_Y);
                        player.Shape.Position = playerStartingPosition;
                    } else {
                        var newObstacle = new Obstacle(shape, null) {
                            Image = imageContainer.GetImageByName(levelImages[mapChar])
                        };
                        obstacles.AddStationaryEntity(newObstacle);
                    }
                }
                i++;
            }

            if (playerStartingPosition == null) {
                throw new Exception(
                    "Error building level. Level map must contain a starting position for player.");
            }

            if (portals.CountEntities() < 1) {
                throw new Exception(
                    "Error building level. Level map must contain portal entities.");
            }

            var levelCustomers = 
                levelParser.ParseCustomerStrings(levelContent[Constants.CUSTOMERS]);
            
            var name = levelParser.ParseLevelName(levelContent[Constants.NAME]);
            
            return new Level(
                name, 
                portals, 
                obstacles, 
                platformDictionary, 
                levelCustomers, 
                player, 
                playerStartingPosition);
        }
    }
}