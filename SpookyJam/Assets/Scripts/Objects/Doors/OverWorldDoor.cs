using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWorldDoor : Door
{
    [SerializeField] private int _world;
    protected override void OpenDoor()
    {
        if (SaveDataManager.Instance.IsWorldUnlocked(_world - 1))
        {
            SceneTransition.Instance.EndLevelTransition();
            GameManager.Instance.SetEntrance(_entranceNumber);
            GameManager.Instance.LoadOverworld();
        }
    }

    public int GetWorld() { return _world; }
}
