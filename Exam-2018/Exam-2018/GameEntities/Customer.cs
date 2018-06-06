using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using Exam_2018.LevelCreation;
using Exam_2018.Physics;

namespace Exam_2018.GameEntities {
    public class Customer : Entity {
        private CustomerDirection direction;
        private ImageContainer imageContainer;
        public string Name;
        public char PlatformId;
        public bool NextLevel;
        public char DestinationPlatformId;
        public int TimeToDeliver;
        public int WhenToSpawn;
        public int Points;

        public Customer(Shape shape, IBaseImage image) : base(shape, image) {
            Shape.AsDynamicShape().Direction = new Vec2F();
            imageContainer = ImageContainer.GetInstance();
            direction = CustomerDirection.Right;
        }

        /// <summary>
        /// Recovers the correct walking animation for the instance and renders it.
        /// </summary>
        public void RenderCustomer() {
            Image = imageContainer.GetCustomerStride(direction);
            RenderEntity();
        }

        /// <summary>
        /// Update function which moves the customer.
        /// </summary>
        public void Update(Shape platform) {
            var shape = Shape.AsDynamicShape();
            shape.Move();
            Force.ApplyForce(
                direction == CustomerDirection.Right
                    ? new Vec2F(Constants.CUSTOMER_ACCEL_X, 0)
                    : new Vec2F(-Constants.CUSTOMER_ACCEL_X, 0), Shape.AsDynamicShape());
            if (Shape.Position.X + Shape.Extent.X > platform.Position.X + platform.Extent.X) {
                TurnAround();
            } else if (Shape.Position.X < platform.Position.X) {
                TurnAround();
            }
        }

        /// <summary>
        /// Flips the direction the customer is walking in.
        /// Used to turn when a customer reaches the end of a platform.
        /// </summary>
        private void TurnAround() {
            var shape = Shape.AsDynamicShape();
            var xDir = shape.Direction.X;
            shape.Direction = new Vec2F(xDir * -1, 0);
            direction = shape.Direction.X < 0 ? CustomerDirection.Left : CustomerDirection.Right;
        }

        /// <summary>
        /// Returns a boolean value determining whether the customer should be dropped off
        /// at the platform or not
        /// </summary>
        /// <param name="platform">Platform to check</param>
        /// <returns>True if it should be dropped off, false if not</returns>
        public bool IsDestination(Platform platform) {
            if (NextLevel) {
                return false;
            }
            return DestinationPlatformId == platform.Id || DestinationPlatformId == '^';
        }

        /// <summary>
        /// Returns a new instance of Customer containing the same values as the fields in the one
        /// the function is called on.
        /// </summary>
        /// <returns>New customer instance</returns>
        public Customer Copy() {
            return new Customer(Shape, Image) {
                Name = Name,
                PlatformId = PlatformId,
                NextLevel = NextLevel,
                DestinationPlatformId = DestinationPlatformId,
                TimeToDeliver = TimeToDeliver,
                WhenToSpawn = WhenToSpawn,
                Points = Points                  
            };
        }

        public bool Collision(Shape shape) {
            var collisionData = CollisionDetection.Aabb(Shape.AsDynamicShape(), shape);
            if (!collisionData.Collision) {
                return false;
            }
            DeleteEntity();
            return true;
        }

    }
}