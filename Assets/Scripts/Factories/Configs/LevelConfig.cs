public enum LevelStatus { Closed, Opened, Completed }

[System.Serializable]
public class LevelConfig
{
    public int StageIndex;
    public int Number;
    public LevelStatus Status;
    public LevelDifficulty Difficulty;

    public LevelConfig(int stageIndex, int levelNumber, LevelStatus status, LevelDifficulty difficulty)
    {
        StageIndex = stageIndex;
        Number = levelNumber;
        Status = status;
        Difficulty = difficulty;
    }
}
