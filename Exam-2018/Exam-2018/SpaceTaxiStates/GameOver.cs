using System.Drawing;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Exam_2018.LevelCreation;

namespace Exam_2018.SpaceTaxiStates {
    /// <summary>
    /// Represents the losing state of the game
    /// </summary>
    public class GameOver : IGameState {
        private static GameOver instance;
        private GameEventBus<object> spaceTaxiBus;
        private ImageContainer imageContainer;
        private Entity backgroundImage;
        private Text gameOverText;
        private Text tryAgainButton;

        public static GameOver GetInstance() {
            return GameOver.instance ?? (GameOver.instance = new GameOver());
        }

        private GameOver() {
            InitializeGameState();
        }
        public void GameLoop() {
            throw new System.NotImplementedException();
        }

        public void InitializeGameState() {
            spaceTaxiBus = SpaceTaxiBus.GetBus();
            imageContainer = ImageContainer.GetInstance();
            backgroundImage = 
                new Entity(
                    new StationaryShape(0.0f, 0.0f, 1f, 1f),
                    imageContainer.GetImageByName("SpaceBackground.png"));
            gameOverText = new Text("GAME OVER", new Vec2F(0.31f, 0.4f), new Vec2F(0.4f, 0.3f));
            gameOverText.SetColor(Color.White);
            tryAgainButton = 
                new Text("Press ENTER to try again", new Vec2F(0.25f, 0.0f), new Vec2F(0.7f, 0.5f));
            tryAgainButton.SetColor(Color.White);
            tryAgainButton.SetFontSize(20);
        }

        public void UpdateGameLogic() {
            //empty because there is no logic, but it needs to be empty due to
            //the way in which Game runs the states
        }

        public void RenderState() {
            backgroundImage.RenderEntity();
            gameOverText.RenderText();
            tryAgainButton.RenderText();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
            case "KEY_PRESS":
                switch (keyValue) {
                case "KEY_ESCAPE":
                    Game.Win.CloseWindow();
                    break;
                case "KEY_ENTER":
                    spaceTaxiBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "CHANGE_STATE", 
                             "GAME_RUNNING", "INITIALIZE"));
                    break;
                }

                break;
            }
        }
    }
}