using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Graphics;
using DIKUArcade.Utilities;

namespace Exam_2018.LevelCreation {
    
    /// <summary>
    /// Enum used to describe the direction the taxi is facing.
    /// </summary>
    public enum TaxiOrientation {
        Left=0,
        Right
    }
    
    /// <summary>
    /// Enum used to describe in which direction the taxi is moving.
    /// </summary>
    public enum TaxiDirection{
        None=0,
        Left,
        LeftAndUp,
        Up=3,
        RightAndUp=1,
        Right
    }

    /// <summary>
    /// Enum used to descibre in which direction a customer is moving.
    /// </summary>
    public enum CustomerDirection {
        Left=0,
        Right
    }
    
    /// <summary>
    /// ImageContainer is responsible for loading all image assets used in the SpaceTaxi game.
    /// </summary>
    public class ImageContainer {
        private static ImageContainer instance;
        private Dictionary<string, Image> imageDictionary;
        private IBaseImage[,] playerStrides;
        private IBaseImage[] customerStrides;

        public static ImageContainer GetInstance() {
            return ImageContainer.instance ?? (ImageContainer.instance = new ImageContainer());
        }

        private ImageContainer() {
            imageDictionary = new Dictionary<string, Image>();
            MakeImages();
            playerStrides = MakePlayerStrides();
            customerStrides = MakeCustomerStrides();
        }

        /// <summary>
        /// Generates image objects for all of the images in the Assets/Images/ folder
        /// for the project.
        /// </summary>
        private void MakeImages() {
            var directory = 
                new DirectoryInfo(Path.Combine(FileIO.GetProjectPath(), "Assets", "Images"));
            var imageFiles = directory.GetFiles();

            foreach (FileInfo imageFile in imageFiles) {
                if (imageFile.Name.EndsWith(".png") && !imageFile.Name.StartsWith("Taxi") &&
                    !imageFile.Name.StartsWith("Customer")) {
                    imageDictionary.Add(imageFile.Name, new Image(imageFile.FullName));
                }
            }
        }

        /// <summary>
        /// Returns an array of IBaseImage objects of the player representing all possible movements
        /// of the player
        /// </summary>
        /// <returns>IBaseImage array</returns>
        private IBaseImage[,] MakePlayerStrides() {
            var images = new IBaseImage[2,4];
            images[0,0] = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            images[1,0] = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));
            images[0,1] = new ImageStride(80, ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Back.png")));
            images[0,2] = new ImageStride(80, ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back.png")));
            images[0,3] = new ImageStride(80, ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom.png")));
            images[1,1] = new ImageStride(80, ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Back_Right.png")));
            images[1,2] = new ImageStride(80, ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back_Right.png")));
            images[1,3] = new ImageStride(80, ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Right.png")));
            return images;
        }

        /// <summary>
        /// Returns an IBaseImage for the customer class to use.
        /// </summary>
        /// <returns>IBaseImage of customer</returns>
        private IBaseImage[] MakeCustomerStrides() {
            var images = new IBaseImage[2];
            images[0] = new ImageStride(80, ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "CustomerWalkLeft.png")));
            images[1] = new ImageStride(80, ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "CustomerWalkRight.png")));
            return images;
        }

        /// <summary>
        /// Returns an IBaseImage of the player taxi depending on the TaxiOrientation and
        /// TaxiDirection parameters given
        /// </summary>
        /// <param name="orientation"></param>
        /// <param name="direction"></param>
        /// <returns>IBaseImage of player taxi</returns>
        public IBaseImage GetPlayerStride(TaxiOrientation orientation, TaxiDirection direction) {
            return playerStrides[(int) orientation, (int) direction];
        }

        /// <summary>
        /// Returns a walking animation for the customer instances.
        /// </summary>
        /// <returns>Walking animation of customer depending on the CustomerDirection</returns>
        public IBaseImage GetCustomerStride(CustomerDirection direction) {
            return customerStrides[(int) direction];
        }


        /// <summary>
        /// Returns the image object corresponding to the given string parameter, if said string is
        /// in the ImageContainer's image dictionary.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Image from dictionary</returns>
        public Image GetImageByName(string filename) {
            if (!imageDictionary.ContainsKey(filename)) {
                throw new Exception(
                    "Image with name '" + filename + "' does not exist in dictionary.");
            }
            return imageDictionary[filename];
        }
    }
}