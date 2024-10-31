using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUxManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textBox;

    private void Update()
    {
        _textBox.text = PumpkinManager.Instance.GetPumpkinsFound() + " / " + PumpkinManager.Instance.GetMaxPumpkins();
    }
}
