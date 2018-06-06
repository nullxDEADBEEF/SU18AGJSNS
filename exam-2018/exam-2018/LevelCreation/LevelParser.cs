using System;
using System.Collections.Generic;
using System.Text;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using exam_2018.GameEntities;

namespace exam_2018.LevelCreation {
    
    /// <summary>
    /// Parses chunks of Space Taxi level data.
    /// </summary>
    public class LevelParser : ILevelParser {

        /// <summary>
        /// Links each image file used to its char
        /// </summary>
        /// <param name="imageStrings">
        /// Strings containing only Space Taxi level information about image files
        /// </param>
        /// <returns>Dictionary containing all used image file and char combinations</returns>
        public Dictionary<char, string> ParseImages(List<string> imageStrings) {
            var charImageNameDictionary = new Dictionary<char, string>();
            foreach (string imageString in imageStrings) {
                var imageChar = imageString[0];
                var imageName = imageString.Substring(3);
                charImageNameDictionary.Add(imageChar, imageName);
            }
            return charImageNameDictionary;
        }
        
        /// <summary>
        /// Parses Space Taxi level information about customers in a level
        /// </summary>
        /// <param name="customerStrings">Strings containing only customer information</param>
        /// <returns>List of customers based on level data</returns>
        public List<Customer> ParseCustomerStrings(List<string> customerStrings) {
            var customerList = new List<Customer>();
            
            var customerData = new string[6];
            foreach (string customerString in customerStrings) {
                var customerSubstring = customerString.Substring("Customer: ".Length);
                customerData = customerSubstring.Split(' ');

                Customer customer = new Customer(
                    new DynamicShape(
                        new Vec2F(), new Vec2F(Constants.EXTENT_X, Constants.EXTENT_Y)), null) {
                    Name = customerData[0],
                    WhenToSpawn = Int32.Parse(customerData[1]),
                    PlatformId = customerData[2][0],
                    TimeToDeliver = Int32.Parse(customerData[4]),
                    Points = Int32.Parse(customerData[5])
                };
                if (customerData[3].Length > 1) {
                    customer.DestinationPlatformId = 
                        customerData[3][0] == '^' ? customerData[3][1] : customerData[3][0];
                    customer.NextLevel = true;
                    customerList.Add(customer);
                    
                } else if (customerData[3][0] == '^') {
                    customer.DestinationPlatformId = '^';
                    customer.NextLevel = true;
                    customerList.Add(customer);
                    
                } else {
                    customer.DestinationPlatformId = customerData[3][0];
                    customer.NextLevel = false;
                    customerList.Add(customer);
                }
                
            }

            return customerList;
        }
        
        /// <summary>
        /// DEPRECATED AND UNIMPLEMENTED
        /// Find player spawning location in a Space Taxi level map and return player
        /// </summary>
        /// <param name="mapStrings">Strings containing only a Space Taxi level map</param>
        /// <returns>Player object based on map data</returns>
        public Player ParsePlayer(List<string> mapStrings) {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Creates set of characters representing platforms in a Space Taxi level
        /// </summary>
        /// <param name="platformStrings">String(s) containing only platform information</param>
        /// <returns>HashSet containing platform characters</returns>
        public HashSet<char> ParsePlatformChars(List<string> platformStrings) {
            var platforms = new HashSet<char>();
            foreach (string platformstring in platformStrings) {
                var charArray = platformstring.Substring("platforms:".Length).Trim();
                foreach (char character in charArray) {
                    if (character == ',') {
                        continue;
                    }
                        platforms.Add(character);
                }
            }

            return platforms;
        }
        
        /// <summary>
        /// Returns the name of a Space Taxi level
        /// </summary>
        /// <param name="nameStrings">String(s) containing the name of a Space Taxi level</param>
        /// <returns>Name of level as string</returns>
        public string ParseLevelName(List<string> nameStrings) {
            StringBuilder nameBuilder = new StringBuilder();
            foreach (string nameString in nameStrings) {
                nameBuilder.Append(nameString.Substring("Name: ".Length));
            }

            return nameBuilder.ToString();
        }
    }
}