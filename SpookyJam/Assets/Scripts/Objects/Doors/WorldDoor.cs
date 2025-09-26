using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDoor : Door
{
    [SerializeField] private int _world;
    protected override void OpenDoor()
    {
        if (SaveDataManager.Instance.IsWorldUnlocked(_world - 1))
        {
            SceneTransition.Instance.EndLevelTransition(() => {
                GameManager.Instance.LoadWorldHallway(_world, _entranceNumber);
            });
        }
    }

    public int GetWorld() { return _world; }
}
