using System.Collections.Generic;

namespace Exam_2018.LevelCreation {
    public interface ILevelLoader {
        Dictionary<string, List<string>> ReadFileContents(string filepath);
    }
}