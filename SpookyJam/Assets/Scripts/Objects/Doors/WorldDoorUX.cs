using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldDoorUX : MonoBehaviour
{
    [SerializeField] private WorldDoor _thisDoor;
    [SerializeField] private TextMeshProUGUI _worldNameText;
    [SerializeField] private TextMeshProUGUI _shadowNameText;
    [SerializeField] private SpriteRenderer _worldDoorSprite;


    private void Start()
    {
        int world = _thisDoor.GetWorld();
        _worldNameText.text = _thisDoor.GetWorld() + "";
        _shadowNameText.text = _worldNameText.text;
    }
}
