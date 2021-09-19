using Enums;

[System.Serializable]
public class LevelConfig
{
    public int StageIndex;
    public int Number;
    public LevelStatus Status;

    public LevelConfig(int stageIndex, int levelNumber, LevelStatus status)
    {
        StageIndex = stageIndex;
        Number = levelNumber;
        Status = status;
    }
}
