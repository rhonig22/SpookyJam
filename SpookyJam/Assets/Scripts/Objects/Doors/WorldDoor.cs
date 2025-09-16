using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDoor : Door
{
    [SerializeField] private int _world;
    [SerializeField] private int _pumpkinRequirement;
    protected override void OpenDoor()
    {
        if (SaveDataManager.Instance.GetTotalPumpkinCount() >= _pumpkinRequirement)
        {
            SceneTransition.Instance.EndLevelTransition();
            GameManager.Instance.LoadWorldHallway(_world);
        }
    }

    public int GetWorld() { return _world; }
    public int GetPumpkinReq() { return _pumpkinRequirement; }
}
