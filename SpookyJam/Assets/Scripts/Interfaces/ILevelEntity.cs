using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelEntity
{
    public LevelEntity GetLevelEntity();
    public LevelEntity GetLevelEntity(LevelEntity levelEntity);
    public void SetLevelEntity(LevelEntity levelEntity);
}
