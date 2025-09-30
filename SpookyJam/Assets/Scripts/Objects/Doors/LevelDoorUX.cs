using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class LevelDoorUX : MonoBehaviour
{
    [SerializeField] private LevelDoor _thisDoor;
    [SerializeField] private TextMeshProUGUI _levelNameText;
    [SerializeField] private TextMeshProUGUI _pumpkinReqText;
    [SerializeField] private Animator _doorAnimator;
    [SerializeField] private GameObject _doorShine;
    [SerializeField] private AudioClip _fireSound;
    [SerializeField] private AudioClip _unlockSound;
    private LevelDoorStates _doorState = LevelDoorStates.Locked;

    protected enum LevelDoorStates
    {
        Locked = 0,
        Unlocked = 1,
        Completed = 2
    }

    private void Start()
    {
        _levelNameText.text = "" + _thisDoor.GetLevel();
        var pumpReq = _thisDoor.GetPumpkinReq();
        _pumpkinReqText.text = pumpReq + "";
        var level = _thisDoor.GetLevel();
        var world = _thisDoor.GetWorld();
        if (pumpReq == 0 || SaveDataManager.Instance.IsLevelUnlocked(world - 1, level - 1))
        {
            SetDoorUnlocked();
        }

        if (SaveDataManager.Instance.IsLevelHundredPercented(world - 1, level - 1))
        {
            _doorShine.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<PlayerInteractor>() != null)
        {
            if (_doorState == LevelDoorStates.Locked && SaveDataManager.Instance.GetTotalPumpkinCount() >= _thisDoor.GetPumpkinReq())
            {
                UnlockDoor();
            }
            else if (_doorState == LevelDoorStates.Unlocked && SaveDataManager.Instance.IsLevelCompleted(_thisDoor.GetWorld() - 1, _thisDoor.GetLevel() - 1))
            {
                CompleteDoor();
            }
        }
    }

    private void CompleteDoor()
    {
        _doorState = LevelDoorStates.Completed;
        SoundManager.Instance.PlaySound(_fireSound, transform.position, 1f);
        _doorAnimator.SetTrigger("Completed");
    }

    private void UnlockDoor()
    {
        _doorState = LevelDoorStates.Unlocked;
        _pumpkinReqText.text = "";
        SoundManager.Instance.PlaySound(_unlockSound, transform.position, 1f);
        _doorAnimator.SetTrigger("Unlocked");
        SaveDataManager.Instance.UnlockLevel(_thisDoor.GetWorld() - 1, _thisDoor.GetLevel() - 1);
    }

    private void SetDoorUnlocked()
    {
        _doorAnimator.SetBool("IsUnlocked", true);
        _doorState = LevelDoorStates.Unlocked;
        _pumpkinReqText.text = "";
    }
}