using System;
using System.Collections.Generic;

[Serializable]
public class LevelList
{
    public List<WorldData> Worlds;
}

[Serializable]
public class WorldData
{
    public List<LevelData> Levels;
    public bool Unlocked;
    public bool Completed;
    public int Requirement;

    public WorldData()
    {
        Unlocked = true;
        Completed = false;
        Requirement = 0;
        Levels = new List<LevelData>();
    }

    public void AddLevel(int pumpkins)
    {
        Levels.Add(new LevelData(pumpkins));
    }
}

[Serializable]
public class LevelData
{
    public bool[] PumpkinsFound;

    public LevelData(int pumpkins)
    {
        PumpkinsFound = new bool[pumpkins];
    }
}
