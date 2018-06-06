using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;
using exam_2018.LevelCreation;
using exam_2018.SpaceTaxiStates;

namespace exam_2018 {
    public class Game {
        private GameEventBus<object> eventBus;
        private GameTimer gameTimer;
        public static Window Win;
        private LevelManager levelManager;
        private StateMachine stateMachine;


        public Game() {
            // window
            Game.Win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R4X3);
            
            // event bus
            eventBus = SpaceTaxiBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.GameStateEvent,
                GameEventType.InputEvent,
                GameEventType.WindowEvent,
                GameEventType.PlayerEvent,
                GameEventType.TimedEvent
            });
            Game.Win.RegisterEventBus(eventBus);  
            
            levelManager = LevelManager.GetInstance();
            
            // state machine
            stateMachine = new StateMachine();
            
            // game timer
            gameTimer = new GameTimer(60); // 60 UPS, no FPS limit 
        }

        public void GameLoop() {
            while (Game.Win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    Game.Win.PollEvents();
                    eventBus.ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
                }

                if (gameTimer.ShouldRender()) {
                    Game.Win.Clear();
                    stateMachine.ActiveState.RenderState();
                    Game.Win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps from the timer
                    Game.Win.Title = "Space Taxi | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
                                 gameTimer.CapturedFrames;
                }
            }
        }


    }
}