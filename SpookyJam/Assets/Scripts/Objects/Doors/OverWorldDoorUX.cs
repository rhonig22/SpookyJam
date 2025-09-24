using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.LightTransport;

public class OverWorldDoorUX : MonoBehaviour
{
    [SerializeField] private OverWorldDoor _thisDoor;
    [SerializeField] private SpriteRenderer _worldDoorSprite;
    private bool _locked = true;


    private void Start()
    {
        int world = _thisDoor.GetWorld();
    }

    private void UnlockDoor()
    {
    }
}
