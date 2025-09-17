using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoor : Door
{
    [SerializeField] private int _world;
    [SerializeField] private int _level;
    [SerializeField] private int _pumpkinRequirement;
    protected override void OpenDoor()
    {
        if (SaveDataManager.Instance.IsLevelUnlocked(_world - 1, _level - 1))
        {
            SceneTransition.Instance.EndLevelTransition();
            GameManager.Instance.SetWorld(_world);
            GameManager.Instance.LoadLevel(_level);
        }
    }

    public int GetWorld() { return _world; }
    public int GetLevel() { return _level; }
    public int GetPumpkinReq() { return _pumpkinRequirement; }
}
