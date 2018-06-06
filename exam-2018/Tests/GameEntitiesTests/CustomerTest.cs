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
    public class CustomerTest {
        private Window win;
        private Customer testCustomer;
        private Platform testPlatform;

        [SetUp]
        public void Init() {
            win = new Window("customer test", 1,1);
            testCustomer = new Customer(
                new DynamicShape(0.1f, Constants.EXTENT_Y, Constants.EXTENT_X,
                    Constants.EXTENT_Y), null) {
                Name = "Bob",
                PlatformId = 'x',
                NextLevel = true,
                DestinationPlatformId = '^',
                TimeToDeliver = 55,
                WhenToSpawn = 23,
                Points = 999
            };

            testPlatform = new Platform('R');
            testPlatform.AddPlatform(
                new StationaryShape(0,0,1.0f,Constants.EXTENT_Y), testCustomer.Image);
        }

        [Test]
        public void CopyReturnsExpectedCopy() {
            var expectedName = testCustomer.Name;
            var expectedPlatformId = testCustomer.PlatformId;
            var expectedNextLevel = testCustomer.NextLevel;
            var expectedDestinationPlatformId = testCustomer.DestinationPlatformId;
            var expectedTimeToDeliver = testCustomer.TimeToDeliver;
            var expectedWhenToSpawn = testCustomer.WhenToSpawn;
            var expectedPoints = testCustomer.Points;

            var copiedCustomer = testCustomer.Copy();
            
            Assert.IsTrue(
                (expectedName == copiedCustomer.Name &&
                 expectedPlatformId == copiedCustomer.PlatformId &&
                 expectedNextLevel == copiedCustomer.NextLevel &&
                 expectedDestinationPlatformId == copiedCustomer.DestinationPlatformId &&
                 expectedTimeToDeliver == copiedCustomer.TimeToDeliver &&
                 expectedWhenToSpawn == copiedCustomer.WhenToSpawn &&
                 expectedPoints == copiedCustomer.Points)
                );
        }

        [Test]
        public void IsDestinationReturnsCorrectBoolean() {
            var shape = testCustomer.Shape;
            var customer1 = new Customer(shape, null) {
                DestinationPlatformId = 'R' //expect true since customer1 has same id as platform
            };
            
            var customer2 = new Customer(shape, null) {
                DestinationPlatformId = '^' //expect true since customer2 has the wildcard id
            };
            
            //expect false since customer3 does not have same id as platform
            var customer3 = new Customer(shape, null) {
                DestinationPlatformId = 'x' 
            };

            //expect false because NextLevel = true
            var customer4 = new Customer(shape, null) {DestinationPlatformId = 'R'};
            customer4.NextLevel = true;
            
            //expect false because NextLevel = true
            var customer5 = new Customer(shape, null) {DestinationPlatformId = '^'};
            customer5.NextLevel = true;
            //expect false because NextLevel = true
            var customer6 = new Customer(shape, null) {DestinationPlatformId = 'x'};
            customer6.NextLevel = true;
            
            Assert.IsTrue(
                (customer1.IsDestination(testPlatform) &&
                 customer2.IsDestination(testPlatform) &&
                 customer3.IsDestination(testPlatform) == false &&
                 customer4.IsDestination(testPlatform) == false &&
                 customer5.IsDestination(testPlatform) == false &&
                 customer6.IsDestination(testPlatform) == false
                 )
                );
        }
        
        [Test]
        public void CustomerCollisionMarksCustomerForDeletion() {
            var player = new Player(null, null);
            player.Shape.Position = //set to same y value as customer to ensure collision
                new Vec2F(
                    testCustomer.Shape.Position.X + 4 * Constants.EXTENT_X, 
                    testCustomer.Shape.Position.Y);
            for (int i = 0; i < 100; i++) {
                Force.ApplyForce(new Vec2F(0.01f, 0), testCustomer.Shape.AsDynamicShape());
                testCustomer.Shape.Move();
                testCustomer.Collision(player.Shape.AsDynamicShape());
            }

            Assert.AreEqual(true, testCustomer.IsDeleted());
        }
    }
}