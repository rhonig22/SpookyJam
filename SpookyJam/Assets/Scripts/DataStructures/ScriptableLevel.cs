using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Level", menuName = "Scriptables/ New Level")]
public class ScriptableLevel : ScriptableObject
{
    [SerializeField] private string _sceneName;
    [SerializeField] private int _pumpkinCount;

    public string GetSceneName()
    {
        return _sceneName;
    }

    public int GetPumpkinCount()
    {
        return _pumpkinCount;
    }

}
