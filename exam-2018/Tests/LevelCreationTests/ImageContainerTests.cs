using System.Collections.Generic;
using DIKUArcade;
using exam_2018.LevelCreation;
using NUnit.Framework;

namespace Tests.LevelCreationTests {
    [TestFixture]
    public class ImageContainerTests {
        private Window win;
        private ImageContainer imageContainer;

        [SetUp]
        public void Init() {
            win = new Window("ImageContainerTest", 200, AspectRatio.R1X1);
            imageContainer = ImageContainer.GetInstance();
        }

        /// <summary>
        /// Tests if the GetPlayerStride() method is able to return an IBaseImage for
        /// any possible combination of the TaxiOrientation and TaxiDirection enums
        /// </summary>
        /// <param name="orientation"></param>
        /// <param name="direction"></param>
        [TestCase(TaxiOrientation.Left,TaxiDirection.Left)]
        [TestCase(TaxiOrientation.Left,TaxiDirection.LeftAndUp)]
        [TestCase(TaxiOrientation.Left,TaxiDirection.Up)]
        [TestCase(TaxiOrientation.Right,TaxiDirection.Up)]
        [TestCase(TaxiOrientation.Right,TaxiDirection.Right)]
        [TestCase(TaxiOrientation.Right,TaxiDirection.RightAndUp)]
        public void GetPlayerStridesReturnsImageStrideForAllPossibleDirections(
            TaxiOrientation orientation, TaxiDirection direction) {
            
            //if test doesn't fail, the images were successfully retrieved from the ImageContainer
            var strides = imageContainer.GetPlayerStride(orientation, direction);
        }

        /// <summary>
        /// Tests if GetCustomerStride returns an image for both CustomerDirection enum values
        /// </summary>
        /// <param name="direction"></param>
        [TestCase(CustomerDirection.Left)]
        [TestCase(CustomerDirection.Right)]
        public void GetCustomerStridesBothDirections(CustomerDirection direction) {
            var strides = imageContainer.GetCustomerStride(direction);
        }

        
        /// <summary>
        /// Tests if the ImageContainer will return an image known to exist in the
        /// Assets/Images/ folder in the project. Not all images are tested, but all of the
        /// images used in the short-n-sweet.txt level are included.
        /// </summary>
        [Test]
        public void GetImageByNameReturnsIfIMageExists() {
            //all images used in short-n-sweet.txt
            var images = new List<string>() {
                "white-square.png",
                "ironstone-square.png",
                "neptune-square.png",
                "green-square.png",
                "yellow-stick.png",
                "purple-circle.png",
                "green-upper-left.png",
                "green-upper-right.png",
                "green-lower-left.png",
                "green-lower-right.png",
                "ironstone-upper-left.png",
                "ironstone-upper-right.png",
                "ironstone-lower-left.png",
                "ironstone-lower-right.png",
                "neptune-upper-left.png",
                "neptune-upper-right.png",
                "neptune-lower-left.png",
                "neptune-lower-right.png",
                "white-upper-left.png",
                "white-upper-right.png",
                "white-lower-left.png",
                "white-lower-right.png",
            };
            foreach (string imagename in images) {
                imageContainer.GetImageByName(imagename);
            }
            
        }
}
}