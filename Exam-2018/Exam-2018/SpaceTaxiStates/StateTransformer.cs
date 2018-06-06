using System;

namespace Exam_2018.SpaceTaxiStates {
    /// <summary>
    /// Transforms strings into game state types and game state types into strings
    /// </summary>
    public class StateTransformer {
        public GameStateType TransformStringToState(string state) {
            switch (state) {
            case "GAME_RUNNING":
                return GameStateType.GameRunning;
            case "GAME_PAUSED":
                return GameStateType.GamePaused;
            case "MAIN_MENU":
                return GameStateType.MainMenu;
            case "GAME_OVER":
                return GameStateType.GameOver;
            default:
                throw new ArgumentException("The argument '" + state + "' is not implemented");
            }
        }
        public string TransformStateToString(GameStateType state) {
            switch (state) {
            case GameStateType.GameRunning:
                return "GAME_RUNNING";
            case GameStateType.GamePaused:
                return "GAME_PAUSED";
            case GameStateType.MainMenu:
                return "MAIN_MENU";
            case GameStateType.GameOver:
                return "GAME_OVER";
            default:
                throw new ArgumentException("The argument '" + state + "' is not implemented");
            }
        }
    }
}