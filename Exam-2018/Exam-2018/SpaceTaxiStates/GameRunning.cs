using System;
using DIKUArcade.EventBus;
using DIKUArcade.State;
using Exam_2018.LevelCreation;

namespace Exam_2018.SpaceTaxiStates {
    /// <summary>
    /// In-game state
    /// </summary>
    public class GameRunning : IGameState {
        private static GameRunning instance = null;
        private GameEventBus<object> eventBus;
        private LevelManager levelManager;


        GameRunning() {
            eventBus = SpaceTaxiBus.GetBus();
            levelManager = LevelManager.GetInstance();
        }
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        
        public void GameLoop() {
        }   

        public void InitializeGameState() {
            levelManager.ActiveLevel.RestartLevel();
        }
        public void UpdateGameLogic() {
            levelManager.ActiveLevel.Update();
        }

        public void RenderState() {
            levelManager.ActiveLevel.Render();
        }
        
        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
            case "KEY_PRESS":
                switch (keyValue) {
                case "KEY_ESCAPE":
                    Game.Win.CloseWindow();
                    break;
                case "KEY_F12":
                    Console.WriteLine("Saving screenshot");
                    Game.Win.SaveScreenShot();
                    break;
                case "KEY_UP":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "KEY_PRESS", ""));
                    break;
                case "KEY_LEFT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "KEY_PRESS", ""));
                    break;
                case "KEY_RIGHT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "KEY_PRESS", ""));
                    break;
                case "KEY_P":
                    levelManager.ActiveLevel.PauseLevel();
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, 
                            this, 
                            "CHANGE_STATE", 
                            "GAME_PAUSED", 
                            "INITIALIZE"));
                    break;
                }
                break;
            
            case "KEY_RELEASE":
                switch (keyValue) {
                case "KEY_LEFT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent,
                            this, "STOP_ACCELERATE_LEFT",
                            "KEY_RELEASE",
                            ""));
                    break;
                case "KEY_RIGHT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "STOP_ACCELERATE_RIGHT", 
                            "KEY_RELEASE", 
                            ""));
                    break;
                case "KEY_UP":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent,
                            this, 
                            "STOP_ACCELERATE_UP",
                            "KEY_RELEASE",
                            ""));
                    break;
                }
                break;
            }
        }
    }
}