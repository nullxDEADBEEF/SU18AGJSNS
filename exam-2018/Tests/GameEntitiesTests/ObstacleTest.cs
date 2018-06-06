using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using exam_2018;
using exam_2018.GameEntities;
using exam_2018.Physics;
using exam_2018;
using NUnit.Framework;


namespace Tests.GameEntitiesTests {
    [TestFixture]
    public class ObstacleTest {
        private Window win;
        private Obstacle obstacle;
        private Player player;
        
        [SetUp]
        public void Init() {
            win = new Window("test", 1, 1);
            obstacle = new Obstacle(
                new StationaryShape(0.5f,0.2f,Constants.EXTENT_X, Constants.EXTENT_Y), null);
            player = new Player(
                new DynamicShape(0f, 0.2f, Constants.EXTENT_X, Constants.EXTENT_Y), null);
            
        }

        [Test]
        public void TestObstacleCollision() {
            int i = 0;
            while(i < 50) {
                Force.ApplyForce(new Vec2F(0.1f, 0), player.Shape.AsDynamicShape());
                player.Shape.Move();
                if (obstacle.Collision(player.Shape.AsDynamicShape())) {
                    break;
                }

                i++;
            }
            Assert.IsTrue(i < 50);
        }

}
}