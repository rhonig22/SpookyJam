using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New World", menuName = "Scriptables/ New World")]
public class ScriptableWorld : ScriptableObject
{
    [SerializeField] private List<ScriptableLevel> _levels;

    public int GetLevelCount()
    {
        return _levels.Count;
    }

    public ScriptableLevel GetLevel(int level)
    {
        if (_levels != null && level < _levels.Count)
            return _levels[level];
        else
            return null;
    }
}