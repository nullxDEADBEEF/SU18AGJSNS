using System.Collections.Generic;

namespace exam_2018.LevelCreation {
    public interface ILevelLoader {
        Dictionary<string, List<string>> ReadFileContents(string filepath);
    }
}