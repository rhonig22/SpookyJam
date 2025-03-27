using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelEntity
{
    public LevelEntity GetLevelEntity();
    public void SetLevelEntity(LevelEntity levelEntity);
}
