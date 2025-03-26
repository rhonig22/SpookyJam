using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Level Entity", menuName = "Scriptables/ Level Entity")]
public class ScriptableLevelEntity : ScriptableObject
{
    [SerializeField] LevelEntityType _entityType;
    [SerializeField] GameObject _prefab;

    public LevelEntityType GetEntityType() 
    { 
        return _entityType;
    }

    public GameObject GetPrefab()
    {
        return _prefab;
    }
}
