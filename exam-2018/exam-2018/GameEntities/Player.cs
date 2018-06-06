using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using exam_2018.LevelCreation;
using exam_2018.Physics;

namespace exam_2018.GameEntities {
    public class Player : Entity, IGameEventProcessor<object> {
        private ImageContainer imageContainer;
        private TaxiOrientation taxiOrientation;
        private TaxiDirection taxiDirection;
        private int[] mA; //input key array. 0 = left, 1 = up, 2 = right

        public Player(DynamicShape shape, IBaseImage image) : base(shape, image) {
            mA = new int[3];
            taxiOrientation = TaxiOrientation.Left;
            taxiDirection = TaxiDirection.None;
            imageContainer = ImageContainer.GetInstance();
            Shape = new DynamicShape(
                new Vec2F(), 
                new Vec2F(Constants.EXTENT_X*2, Constants.EXTENT_Y));
            Image = imageContainer.GetPlayerStride(taxiOrientation, taxiDirection);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
        }

        /// <summary>
        /// Checks the currently pressed keys and changes the acceleration vector accordingly
        /// </summary>
        private void ApplyAcceleration() {
            var shape = Shape.AsDynamicShape();
            if (mA[0] == 1 && mA[1] == 0 && mA[2] == 0) {
                Force.ApplyForce(new Vec2F(-Constants.TAXI_ACCEL_X, 0f), shape);
                taxiDirection = TaxiDirection.Left;
            }else if (mA[0] == 1 && mA[1] == 1 && mA[2] == 0) {
                Force.ApplyForce(new Vec2F(-Constants.TAXI_ACCEL_X, Constants.TAXI_ACCEL_Y), shape);
                taxiDirection = TaxiDirection.LeftAndUp;
            }else if (mA[0] == 0 && mA[1] == 1 && mA[2] == 0) {
                Force.ApplyForce(new Vec2F(0f, Constants.TAXI_ACCEL_Y), shape);
                taxiDirection = TaxiDirection.Up;
            }else if (mA[0] == 0 && mA[1] == 1 && mA[2] == 1) {
                Force.ApplyForce(new Vec2F(Constants.TAXI_ACCEL_X, Constants.TAXI_ACCEL_Y), shape);
                taxiDirection = TaxiDirection.RightAndUp;
            }else if (mA[0] == 0 && mA[1] == 0 && mA[2] == 1) {
                Force.ApplyForce(new Vec2F(Constants.TAXI_ACCEL_X, 0f), shape);
                taxiDirection = TaxiDirection.Right;
            }else if (mA[0] == 0 && mA[1] == 0 && mA[2] == 0) {
                taxiDirection = TaxiDirection.None;
            }
            Force.ApplyGravity(shape.AsDynamicShape());
        }
       
        // Updates player physics
        public void Update() {
            var shape = Shape.AsDynamicShape();
            shape.Move();
            ApplyAcceleration();
        }

        // NOTE: gameEvent message might be different, change if necessary
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                case "BOOSTER_UPWARDS":
                    mA[1] = 1;
                    break;
                case "BOOSTER_TO_LEFT":
                    mA[0] = 1;
                    taxiOrientation = TaxiOrientation.Left;
                    break;
                case "BOOSTER_TO_RIGHT":
                    mA[2] = 1;
                    taxiOrientation = TaxiOrientation.Right;
                    break;
                case "STOP_ACCELERATE_LEFT":
                    mA[0] = 0;
                    break;
                case "STOP_ACCELERATE_RIGHT":
                    mA[2] = 0;
                    break;
                case "STOP_ACCELERATE_UP":
                    mA[1] = 0;
                   break;
                }
            }
        }
        
        /// <summary>
        /// Resets position, acceleration and velocity and
        /// </summary>
        public void Reset() {
            var shape = Shape.AsDynamicShape();
            shape.Direction = new Vec2F(0f, 0f);
            for (int i = 0; i < mA.Length; i++) {
                mA[i] = 0;
            }
        }

        /// <summary>
        /// Determine image of player object and render
        /// </summary>
        public void RenderPlayer() {
            Image = imageContainer.GetPlayerStride(taxiOrientation, taxiDirection);
            RenderEntity();
        }

    }
}