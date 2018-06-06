namespace Exam_2018 {
    /// <summary>
    /// Contains constants used in different parts of the program
    /// </summary>
    public class Constants {
        //level data category constants
        public const string NAME = "Name";
        public const string IMAGES = "Images";
        public const string PLATFORMS = "Platforms";
        public const string CUSTOMERS = "Customers";
        public const string MAP = "Map";
        
        //global constants
        public const float EXTENT_X = 1 / 40f;
        public const float EXTENT_Y = 1 / 30f;
        
        //physics constants
        public const float GRAVITY_X = 0.0f;
        public const float GRAVITY_Y = -0.0001f;
        public const float MAXSPEED_X = 0.002f;
        public const float MAXSPEED_Y = 0.005f;
        
        //customer constants
        public const float CUSTOMER_ACCEL_X = 0.0001f;
        public const float CUSTOMER_ACCEL_Y = 0;
        
        //player related constants
        public const float TAXI_ACCEL_X = 0.0002f;
        public const float TAXI_ACCEL_Y = 0.00035f;
        public const float CRASHSPEED = 0.0045f;
    }
}