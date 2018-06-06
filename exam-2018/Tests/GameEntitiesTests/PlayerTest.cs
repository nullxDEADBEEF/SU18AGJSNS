using DIKUArcade;
using DIKUArcade.Entities;
using exam_2018.GameEntities;
using NUnit.Framework;

namespace Tests.GameEntitiesTests {
    [TestFixture]
    public class PlayerTest {
        private Window win;
        private Platform testPlatform;
        private Customer testCustomer;
        private int score;

        [SetUp]
        public void Init() {
            win = new Window("player test", 1,1);
            testCustomer = new Customer(new DynamicShape(0.1f, 0.11f, 0.1f, 0.1f), null);
            testCustomer.Name = "Alice";
            testCustomer.PlatformId = 'R';
            testCustomer.Points = 100;
            testPlatform = new Platform('R');
            testPlatform.AddPlatform(new StationaryShape(0, 0, 1.0f, 0.2f), testCustomer.Image);
            score = 0;
        }

        [Test]
        public void TestCorrectDropOffGives100Points() {
            var customer = testCustomer;
            if (customer == null) {
                return;
            }

            if (!customer.IsDestination(testPlatform)) {
                return;
            }

            score += customer.Points;
            Assert.AreSame(score, 100);
        }
    }
}