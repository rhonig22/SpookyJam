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

    public WorldData()
    {
        Unlocked = true;
        Completed = false;
        Levels = new List<LevelData>();
    }

    public void AddLevel(int pumpkins)
    {
        Levels.Add(new LevelData(pumpkins));
    }

    public PumpkinCount GetPumpkinCount()
    { 
        int count = 0;
        int total = 0;
        foreach (var level in Levels)
        {
            for (int i = 0; i < level.PumpkinsFound.Length; i++)
            {
                count += level.PumpkinsFound[i] ? 1 : 0;
                total++;
            }
        }

        return new PumpkinCount { Found = count, Total = total };
    }
}

[Serializable]
public class LevelData
{
    public bool[] PumpkinsFound;
    public bool Completed;

    public LevelData(int pumpkins)
    {
        PumpkinsFound = new bool[pumpkins];
        Completed= false;
    }
}

[Serializable]
public class PumpkinCount
{
    public int Found;
    public int Total;
}