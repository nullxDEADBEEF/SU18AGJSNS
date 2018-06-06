using System;
using exam_2018.SpaceTaxiStates;
using NUnit.Framework;

namespace Tests.StateMachineTests {
    [TestFixture]
public class StateTransformerTest {
    private StateTransformer testStateTransformer;

        [SetUp]
        public void Init() {
           testStateTransformer = new StateTransformer();
        }

        [Test]
        public void TransformStateToStringImplementedGameStateTypesReturnExpectedString() {
            var string1 = testStateTransformer.TransformStateToString(GameStateType.GameOver);
            var string2 = testStateTransformer.TransformStateToString(GameStateType.GamePaused);
            var string3 = testStateTransformer.TransformStateToString(GameStateType.GameRunning);
            var string4 = testStateTransformer.TransformStateToString(GameStateType.MainMenu);
            
            Assert.IsTrue(
                (string1 == "GAME_OVER" &&
                 string2 == "GAME_PAUSED" &&
                 string3 == "GAME_RUNNING" &&
                 string4 == "MAIN_MENU"));
        }

        [Test]
        public void TransformStateToStringUnimplementedGameStateTypeThrowsException() {
            var ex = Assert.Throws<ArgumentException>(
                () => testStateTransformer.TransformStateToString(GameStateType.TestingOnly));
            
            Assert.IsTrue(ex.Message.Contains("TestingOnly"));
        }

        [Test]
        public void TransformStringToStateReturnsStringForImplementedGameStateTypes() {
            var string1 = testStateTransformer.TransformStateToString(GameStateType.GameOver);
            var string2 = testStateTransformer.TransformStateToString(GameStateType.GamePaused);
            var string3 = testStateTransformer.TransformStateToString(GameStateType.GameRunning);
            var string4 = testStateTransformer.TransformStateToString(GameStateType.MainMenu);

            var state1 = testStateTransformer.TransformStringToState(string1);
            var state2 = testStateTransformer.TransformStringToState(string2);
            var state3 = testStateTransformer.TransformStringToState(string3);
            var state4 = testStateTransformer.TransformStringToState(string4);
            
            Assert.IsTrue(
                (state1 == GameStateType.GameOver &&
                 state2 == GameStateType.GamePaused &&
                 state3 == GameStateType.GameRunning &&
                 state4 == GameStateType.MainMenu));
        }

        [Test]
        public void TransformStringToStateThrowsExceptionForUnimplementedGameStateTypes() {
            var ex = Assert.Throws<ArgumentException>(
                () => testStateTransformer.TransformStringToState("GIBBERISH"));
            Assert.IsTrue(ex.Message.Contains("GIBBERISH"));
        }
        
        
    }
}