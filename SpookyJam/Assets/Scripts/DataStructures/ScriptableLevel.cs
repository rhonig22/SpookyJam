using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Level", menuName = "Scriptables/ New Level")]
public class ScriptableLevel : ScriptableObject
{
    [SerializeField] private string _sceneName;
    [SerializeField] private int _pumpkinCount;

    public void SetSceneName(string sceneName)
    {
        _sceneName = sceneName;
    }

    public string GetSceneName()
    {
        return _sceneName;
    }

    public void SetPumpkinCount(int pumpkinCount)
    {
        _pumpkinCount = pumpkinCount;
    }

    public int GetPumpkinCount()
    {
        return _pumpkinCount;
    }

}
