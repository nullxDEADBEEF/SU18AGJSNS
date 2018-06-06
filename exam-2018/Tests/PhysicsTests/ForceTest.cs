using System;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using exam_2018;
using exam_2018.Physics;
using NUnit.Framework;


namespace Tests.PhysicsTests {
    [TestFixture]
    public class ForceTest {
        private DynamicShape testShape;
        private float AcceptableDeviance;
        [SetUp]
        public void Init() {
            testShape = new DynamicShape(0,0,0,0); //default position for testing will be 0,0
            AcceptableDeviance = 0.00001f;
        }

        [Test]
        public void ApplyGravityAppliesCorrectAcceleration() {
            Force.ApplyGravity(testShape);
            testShape.Move();
            //measure movement caused by ApplyGravity
            var dX = Math.Abs(testShape.Position.X) - Math.Abs(Constants.GRAVITY_X);
            var dY = Math.Abs(testShape.Position.Y) - Math.Abs(Constants.GRAVITY_Y);
            Assert.IsTrue(((dX < AcceptableDeviance)&&(dY < AcceptableDeviance)));
        }

        [TestCase(0, 0.5f)]
        [TestCase(0.5f, 0)]
        [TestCase(0, -0.5f)]
        [TestCase(-0.5f, 0)]
        [TestCase(-0.5f, 0.5f)]
        [TestCase(0.5f, 0.5f)]
        [TestCase(0.5f, -0.5f)]
        [TestCase(-0.5f, -0.5f)]
        public void ApplyForceAppliesCorrectAcceleration(float x, float y) {
            var forceApplied = new Vec2F(x, y);
            Force.ApplyForce(forceApplied, testShape);
            //measure movement caused by ApplyGravity
            var dX = Math.Abs(testShape.Position.X) - Math.Abs(forceApplied.X);
            var dY = Math.Abs(testShape.Position.Y) - Math.Abs(forceApplied.Y);
            Assert.IsTrue(((dX < AcceptableDeviance)&&(dY < AcceptableDeviance)));
            
        }
    }
}