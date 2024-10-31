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
}

[Serializable]
public class LevelData
{
    public bool[] PumpkinsFound;
}
