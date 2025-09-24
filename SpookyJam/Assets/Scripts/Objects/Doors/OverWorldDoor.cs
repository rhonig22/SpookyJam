using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWorldDoor : Door
{
    /// <summary>
    /// The 1-based number of the world this hallway exists in, used to check current world levels completed and unlock next world
    /// </summary>
    [SerializeField] private int _world;
    [SerializeField] private int _completedLevelsRequirement;
    [SerializeField] private bool _hasLock;
    protected override void OpenDoor()
    {
        if (!_hasLock || SaveDataManager.Instance.IsWorldUnlocked(_world))
        {
            SceneTransition.Instance.EndLevelTransition();
            GameManager.Instance.SetEntrance(_entranceNumber);
            GameManager.Instance.LoadOverworld();
        }
    }

    public int GetWorld() { return _world; }
    public bool HasLock() { return _hasLock; }
    public int CompletedLevelsRequirement() { return _completedLevelsRequirement; }
}
