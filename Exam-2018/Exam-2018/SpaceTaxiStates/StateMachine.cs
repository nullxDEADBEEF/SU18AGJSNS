using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace Exam_2018.SpaceTaxiStates {
    /// <summary>
    /// State machine responsible for handling game state switching and sending messages to
    /// the currently active state.
    /// </summary>
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }
        private StateTransformer stateTransformer;
        public StateMachine() {
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance();
            stateTransformer = new StateTransformer();
        }
        /// <summary>
        /// Switch state
        /// </summary>
        /// <param name="stateType">State to be switched to</param>
        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
            case GameStateType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                break;
            case GameStateType.GameRunning:
                ActiveState = GameRunning.GetInstance();
                break;
            case GameStateType.GamePaused:
                ActiveState = GamePaused.GetInstance();
                break;
            case GameStateType.GameOver:
                ActiveState = GameOver.GetInstance();
                break;
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            switch (eventType) {
            case GameEventType.WindowEvent:
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    Game.Win.CloseWindow();
                    break;
                }
                break;
            case GameEventType.InputEvent:
                ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
                break;
            case GameEventType.GameStateEvent:
                switch (gameEvent.Message) {
                case "CHANGE_STATE":
                    switch (gameEvent.Parameter2) {
                    //if parameter 2 empty, process event as usually
                    case "":
                        SwitchState(stateTransformer.TransformStringToState(gameEvent.Parameter1));
                        break;
                    case "INITIALIZE":
                        SwitchState(stateTransformer.TransformStringToState(gameEvent.Parameter1));
                        ActiveState.InitializeGameState();
                        break;
                    case "UNPAUSE":
                        SwitchState(stateTransformer.TransformStringToState(gameEvent.Parameter1));
                        ActiveState.HandleKeyEvent("KEY_PRESS", "UNPAUSE");
                        break;
                    }
                    break;
                }
                break;
            }
        }
    }
}