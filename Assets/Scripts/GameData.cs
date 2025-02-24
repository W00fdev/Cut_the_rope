using System;

[Serializable]
public class GameData : ISavable
{
    public LevelsData LevelsData;

    public GameData(string saveKey)
    {
        LevelsData = new LevelsData();
        SaveKey = saveKey;
    }

    public string SaveKey { get; }
}