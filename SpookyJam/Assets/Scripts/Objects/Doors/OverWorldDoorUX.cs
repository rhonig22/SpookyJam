using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.LightTransport;

public class OverWorldDoorUX : MonoBehaviour
{
    [SerializeField] private OverWorldDoor _thisDoor;
    [SerializeField] private SpriteRenderer _worldDoorSprite;
    [SerializeField] private Animator _animator;
    private bool _locked = true;


    private void Start()
    {
        int world = _thisDoor.GetWorld();
        if (!_thisDoor.HasLock() || SaveDataManager.Instance.IsWorldUnlocked(_thisDoor.GetWorld()))
        {
            _animator.SetBool("IsUnlocked", true);
            _locked = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<PlayerInteractor>() != null)
        {
            if (_locked && CanUnlockDoor())
            {
                UnlockDoor();
            }
        }
    }

    private bool CanUnlockDoor()
    {
        var world = _thisDoor.GetWorld();
        var levelRequirement = _thisDoor.CompletedLevelsRequirement();
        var worldData = SaveDataManager.Instance.GetWorldData(world - 1);
        var completedCount = 0;
        foreach (var level in worldData.Levels)
        {
            if (level.Completed)
                completedCount++;
        }

        return completedCount >= levelRequirement;
    }

    private void UnlockDoor()
    {
        _locked = false;
        SaveDataManager.Instance.UnlockWorld(_thisDoor.GetWorld());
        _animator.SetTrigger("Unlocked");
    }
}
