using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverworldUxManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textBox;

    private void Update()
    {
        _textBox.text = SaveDataManager.Instance.GetTotalPumpkinCount() + "";
    }
}
