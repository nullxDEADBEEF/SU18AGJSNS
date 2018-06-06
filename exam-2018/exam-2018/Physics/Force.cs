using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace exam_2018.Physics {
    /// <summary>
    /// Class for adding forces, represented as Vec2F objects, to DynamicShape instances.
    /// </summary>
    public static class Force {
        private static Vec2F gravity = new Vec2F(Constants.GRAVITY_X, Constants.GRAVITY_Y);
        
        /// <summary>
        /// Applies the constants GRAVITY_X and GRAVITY_Y to a DynamicShape's Direction vector
        /// </summary>
        /// <param name="shape">DynamicShape which will be subject to the gravity force</param>
        public static void ApplyGravity(DynamicShape shape) {
            shape.Direction += Force.gravity;
            if (shape.Direction.Y > Constants.MAXSPEED_Y) {
                shape.Direction.Y = Constants.MAXSPEED_Y;
            }else if (shape.Direction.Y < -Constants.MAXSPEED_Y) {
                shape.Direction.Y = -Constants.MAXSPEED_Y;
            }
            if (shape.Direction.X > Constants.MAXSPEED_X) {
                shape.Direction.X = Constants.MAXSPEED_X;
            }else if (shape.Direction.X < -Constants.MAXSPEED_X) {
                shape.Direction.X = -Constants.MAXSPEED_X;
            }
        }

        /// <summary>
        /// Applies an arbitrary force to a DynamicShape's Direction vector
        /// </summary>
        /// <param name="force">Force to be applied, represented as a Vec2F</param>
        /// <param name="shape">DynamicShape that the force will be applied to</param>
        public static void ApplyForce(Vec2F force, DynamicShape shape) {
            shape.Direction += force;
            if (shape.Direction.Y > Constants.MAXSPEED_Y) {
                shape.Direction.Y = Constants.MAXSPEED_Y;
            }else if (shape.Direction.Y < -Constants.MAXSPEED_Y) {
                shape.Direction.Y = -Constants.MAXSPEED_Y;
            }
            if (shape.Direction.X > Constants.MAXSPEED_X) {
                shape.Direction.X = Constants.MAXSPEED_X;
            }else if (shape.Direction.X < -Constants.MAXSPEED_X) {
                shape.Direction.X = -Constants.MAXSPEED_X;
            }
        }
    }
}