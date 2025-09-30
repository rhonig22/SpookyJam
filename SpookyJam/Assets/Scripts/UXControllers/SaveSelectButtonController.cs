using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSelectButtonController : MonoBehaviour
{
    [SerializeField] GameObject _counterPanel;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _nameShadow;
    [SerializeField] TextMeshProUGUI _levelCount;
    [SerializeField] TextMeshProUGUI _levelCountShadow;
    [SerializeField] TextMeshProUGUI _pumpkinCount;
    [SerializeField] TextMeshProUGUI _pumpkinCountShadow;
    [SerializeField] int _saveNumber;

    private void Start()
    {
        var saveMeta = SaveDataManager.Instance.GetSaveMetaData(_saveNumber);
        if (!saveMeta.FileStarted)
        {
            _counterPanel.SetActive(false);
            return;
        }

        _name.text = (_saveNumber + 1).ToString();
        _nameShadow.text = _name.text;
        _levelCount.text = saveMeta.LevelCount.ToString();
        _levelCountShadow.text = _levelCount.text;
        _pumpkinCount.text = saveMeta.PumpkinCount.ToString();
        _pumpkinCountShadow.text = _pumpkinCount.text;
    }

    public void SaveSelected()
    {
        SaveDataManager.Instance.OpenSave(_saveNumber);
        GameManager.Instance.StartGame();
    }
}
