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
    [SerializeField] private SpriteRenderer _doorRenderer;
    [SerializeField] private Sprite[] _doorSprites = new Sprite[0];
    private bool _locked = true;

    protected enum LevelDoorSprites
    {
        Locked = 0,
        Unlocked = 1,
        Completed = 2
    }

    private void Start()
    {
        _levelNameText.text = _thisDoor.GetWorld() + "." + _thisDoor.GetLevel();
        var pumpReq = _thisDoor.GetPumpkinReq();
        _pumpkinReqText.text = pumpReq + "";
        if (pumpReq == 0)
        {
            UnlockDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_locked && collision != null && collision.gameObject.GetComponent<PlayerInteractor>() != null)
        {
            if (SaveDataManager.Instance.GetTotalPumpkinCount() >= _thisDoor.GetPumpkinReq())
            {
                UnlockDoor();
            }
        }
    }

    private void UnlockDoor()
    {
        _locked = false;
        _pumpkinReqText.text = "";
        _doorRenderer.sprite = _doorSprites[(int)(LevelDoorSprites.Unlocked)];
    }
}