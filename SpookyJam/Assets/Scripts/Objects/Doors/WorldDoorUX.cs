using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.LightTransport;

public class WorldDoorUX : MonoBehaviour
{
    [SerializeField] private WorldDoor _thisDoor;
    [SerializeField] private TextMeshProUGUI _worldNameText;
    [SerializeField] private TextMeshProUGUI _shadowNameText;
    [SerializeField] private SpriteRenderer _worldDoorSprite;
    [SerializeField] private Sprite _unlockedWorldDoor;
    private bool _locked = true;


    private void Start()
    {
        int world = _thisDoor.GetWorld();
        _worldNameText.text = _thisDoor.GetWorld() + "";
        _shadowNameText.text = _worldNameText.text;

        if (SaveDataManager.Instance.IsWorldUnlocked(world - 1))
        {
            UnlockDoor();
        }
    }

    private void UnlockDoor()
    {
        _locked = false;
        _worldDoorSprite.sprite = _unlockedWorldDoor;
    }
}
