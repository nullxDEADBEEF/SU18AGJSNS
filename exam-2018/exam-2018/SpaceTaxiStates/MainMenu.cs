using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using exam_2018.LevelCreation;
using Image = DIKUArcade.Graphics.Image;

namespace exam_2018.SpaceTaxiStates {
    /// <summary>
    /// State for selecting levels
    /// </summary>
    public class MainMenu : IGameState {
        private static MainMenu instance = null;
        private GameEventBus<object> eventBus;
        private LevelManager levelManager;
        private FileInfo[] levelInfos;
        private string[] levelNames;
        
        private Text levelTextBox;
        private Text selectLevelText;
        private Entity background;

        public MainMenu() {
            InitializeGameState();
        }
        
        public void GameLoop() {
        }

        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }
        
        public void InitializeGameState() {
            eventBus = SpaceTaxiBus.GetBus();
            levelManager = LevelManager.GetInstance();
            levelInfos = levelManager.LevelInfos;
            
            var levels = new List<string>();
            foreach (var levelInfo in levelInfos) {
                levels.Add(levelInfo.Name);
            }

            levelNames = levels.ToArray();
            levelManager.ActiveLevelIndex = 0;
            
            selectLevelText = 
                new Text("Please select a level \n with the arrow keys. \n ENTER to confirm:", 
                new Vec2F(0.25f, 0.0f), new Vec2F(0.5f, 0.5f));
            selectLevelText.SetColor(Color.White);
            selectLevelText.SetFontSize(35);
            levelTextBox = 
                new Text(levelNames.First(), new Vec2F(0.3f, -0.2f), new Vec2F(0.5f, 0.5f));
            levelTextBox.SetColor(Color.White);
            levelTextBox.SetFontSize(35);
            background = new Entity(
                new StationaryShape(new Vec2F(0f,0f),new Vec2F(1f,1f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
        }

        public void UpdateGameLogic() {
        }

        public void RenderState() {
            background.RenderEntity();
            selectLevelText.RenderText();
            levelTextBox.RenderText();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                    case "KEY_ESCAPE":
                        Game.Win.CloseWindow();
                        break;
                    case "KEY_LEFT":
                        if (levelManager.ActiveLevelIndex > 0) {
                            levelManager.ActiveLevelIndex -= 1;
                        }
                        levelTextBox.SetText(levelNames[levelManager.ActiveLevelIndex]);
                        break;
                    case "KEY_RIGHT":
                        if (levelManager.ActiveLevelIndex < levelNames.Length-1) {
                            levelManager.ActiveLevelIndex += 1;
                        }
                        levelTextBox.SetText(levelNames[levelManager.ActiveLevelIndex]);
                        break;
                    case "KEY_ENTER":
                        levelManager.BuildLevel(levelInfos[levelManager.ActiveLevelIndex]);
                        eventBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this, 
                                "CHANGE_STATE", 
                                "GAME_RUNNING", 
                                ""));
                        break;
                    }
                    break;
                case "KEY_RELEASE":
                    switch (keyValue) {
                    }
                    break;
            }
        }
    }
}