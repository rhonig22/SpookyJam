using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDoor : Door
{
    [SerializeField] private int _world;
    protected override void OpenDoor()
    {
        GameManager.Instance.LoadWorld(_world);
    }
}
