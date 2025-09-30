using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverWorldDoorUX : MonoBehaviour
{
    [SerializeField] private OverWorldDoor _thisDoor;
    [SerializeField] private SpriteRenderer _worldDoorSprite;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _rumbleSound;
    private bool _locked = true;
    private bool _shouldUnlock = false;


    private void Start()
    {
        int world = _thisDoor.GetWorld();
        if (!_thisDoor.HasLock() || SaveDataManager.Instance.IsWorldUnlocked(_thisDoor.GetWorld()))
        {
            _animator.SetBool("IsUnlocked", true);
            _locked = false;
            _thisDoor.HideCandles();
        }
    }

    private void Update()
    {
        if (_locked && _thisDoor.AreAllCandlesLit() && _shouldUnlock)
        {
            UnlockDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<PlayerInteractor>() != null)
        {
            if (_locked && CanUnlockDoor())
            {
                _shouldUnlock = true;
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

        _thisDoor.LightCandles(Mathf.Min(completedCount, levelRequirement));
        return completedCount >= levelRequirement;
    }

    private void UnlockDoor()
    {
        _thisDoor.HideCandles();
        _locked = false;
        SoundManager.Instance.PlaySound(_rumbleSound, transform.position, 1f);
        CameraController.Instance.ShakeCamera(2f);
        SaveDataManager.Instance.UnlockWorld(_thisDoor.GetWorld());
        _animator.SetTrigger("Unlocked");
    }
}
