using System;
using DIKUArcade.Entities;
using exam_2018.GameEntities;
using Microsoft.Win32;
using NUnit.Framework;


namespace Tests.GameEntitiesTests {
    [TestFixture]
    public class PlatformTest {
        private Platform testPlatform;
        private float acceptableDeviance;

        [SetUp]
        public void Init() {
            testPlatform = new Platform('r');
            acceptableDeviance = 0.00000001f;
        }

        [Test]
        public void SizeOfDefaultBoundingBoxIsAsExpected() {
            var shape = testPlatform.GetBoundingBox();
            
            
            Assert.IsTrue((shape.Position.X < acceptableDeviance &&
                           shape.Position.Y < acceptableDeviance));
        }

        [TestCase(1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        [TestCase(-1, -1)]
        public void IncreaseBoundingBoxExtentInGivenDirectionGivesExpectedBoundingBox(
            float x, float y) {
            
            var shape = new StationaryShape(0,0,x,y);
            testPlatform.AddPlatform(shape, null);

            var deltaX = Math.Abs(testPlatform.GetBoundingBox().Extent.X) - Math.Abs(x);
            var deltaY = Math.Abs(testPlatform.GetBoundingBox().Extent.Y) - Math.Abs(y);
            
            Assert.IsTrue(deltaX < acceptableDeviance && deltaY < acceptableDeviance);
        }
        
        [TestCase(1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        [TestCase(-1, -1)]
        public void AddingPlatformToBoundingBoxGivesExpectedBoundingBoxPosition(float x, float y) {
            
            var shape = new StationaryShape(x, y, 0, 0);
            testPlatform.AddPlatform(shape, null);

            var deltaX = Math.Abs(testPlatform.GetBoundingBox().Position.X) - Math.Abs(x);
            var deltaY = Math.Abs(testPlatform.GetBoundingBox().Position.Y) - Math.Abs(y);
            
            Assert.IsTrue(deltaX < acceptableDeviance && deltaY < acceptableDeviance);
        }
    }
}