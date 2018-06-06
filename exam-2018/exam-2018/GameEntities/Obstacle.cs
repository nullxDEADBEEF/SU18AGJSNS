using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;

namespace exam_2018.GameEntities {
    public class Obstacle : Entity {
        public Obstacle(StationaryShape shape, IBaseImage image) : base(shape, image) { }

        public bool Collision(DynamicShape shape) {
            return CollisionDetection.Aabb(shape, Shape).Collision;
        }
    }
}