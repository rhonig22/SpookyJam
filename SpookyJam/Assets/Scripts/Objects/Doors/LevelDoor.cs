using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoor : Door
{
    [SerializeField] private int _world;
    [SerializeField] private int _level;
    [SerializeField] private int _pumpkinRequirement;
    [SerializeField] private AudioClip _lockedSound;
    protected override void OpenDoor()
    {
        if (SaveDataManager.Instance.IsLevelUnlocked(_world - 1, _level - 1))
        {
            SceneTransition.Instance.EndLevelTransition(() =>
            {
                GameManager.Instance.SetWorld(_world);
                GameManager.Instance.SetEntrance(_entranceNumber);
                GameManager.Instance.LoadLevel(_level);
            });
        }
        else
        {
            SoundManager.Instance.PlaySound(_lockedSound, transform.position, 1f);
        }
    }

    public int GetWorld() { return _world; }
    public int GetLevel() { return _level; }
    public int GetPumpkinReq() { return _pumpkinRequirement; }
}
