

using System.Drawing;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Exam_2018.LevelCreation;

namespace Exam_2018.SpaceTaxiStates {
    /// <summary>
    /// Represents the paused state of the game
    /// </summary>
    public class GamePaused : IGameState {
        private static GamePaused instance;
        private GameEventBus<object> eventBus;
        private ImageContainer imageContainer;
        private Entity backgroundImg;
        private Text pausedText;
        GamePaused() {
            InitializeGameState();
        }
        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }
        public void GameLoop() {
        }

        public void InitializeGameState() {
            eventBus = SpaceTaxiBus.GetBus();
            imageContainer = ImageContainer.GetInstance();
            backgroundImg = new Entity(new StationaryShape(0,0,1,1), 
                imageContainer.GetImageByName("SpaceBackground.png"));
            pausedText = new Text(
                "Game paused. \n\nPress P to continue \nPress ESC to quit", 
                new Vec2F(0.23f,0.1f),
                new Vec2F(0.6f, 0.6f));
            pausedText.SetColor(Color.White);
            pausedText.SetFontSize(40);
            
        }

        public void UpdateGameLogic() {
        }

        public void RenderState() {
            backgroundImg.RenderEntity();
            pausedText.RenderText();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
            case "KEY_PRESS":
                switch (keyValue) {
                case "KEY_ESCAPE":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                    break;
                case "KEY_P":
                    eventBus.RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent,
                        this,
                        "CHANGE_STATE",
                        "GAME_RUNNING", ""));
                    break;
                }
                break;
            case "KEY_RELEASE":
                break;
            }    
        }
    }
}