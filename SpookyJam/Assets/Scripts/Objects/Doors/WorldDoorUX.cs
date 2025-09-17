using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldDoorUX : MonoBehaviour
{
    [SerializeField] private WorldDoor _thisDoor;
    [SerializeField] private TextMeshProUGUI _levelNameText;
    [SerializeField] private TextMeshProUGUI _pumpkinReqText;
    [SerializeField] private GameObject _lock;
    [SerializeField] private Color _unlockColor;
    private bool _locked = true;

    private void Start()
    {
        _levelNameText.text = _thisDoor.GetWorld() + "";
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
    }
}
