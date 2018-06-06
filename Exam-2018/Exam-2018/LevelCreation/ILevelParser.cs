using System.Collections.Generic;
using Exam_2018.GameEntities;

namespace Exam_2018.LevelCreation {
    public interface ILevelParser {
        Dictionary<char, string> ParseImages(List<string> imageStrings);
        List<Customer> ParseCustomerStrings(List<string> customerStrings);
        Player ParsePlayer(List<string> mapStrings);
        HashSet<char> ParsePlatformChars(List<string> platformStrings);
        string ParseLevelName(List<string> nameString);
    }
}