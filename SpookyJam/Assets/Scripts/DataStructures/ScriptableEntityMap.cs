using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Entity Map", menuName = "Scriptables/ Entity Map")]
public class ScriptableEntityMap : ScriptableObject
{
    [SerializeField] List<ScriptableLevelEntity> _levelEntityList = new List<ScriptableLevelEntity>();

    public GameObject GetPrefabForEntityType(LevelEntityType entityType)
    {
        foreach (var levelEntity in _levelEntityList)
        {
            if (levelEntity.GetEntityType() == entityType)
                return levelEntity.GetPrefab();
        }

        return null;
    }
}
