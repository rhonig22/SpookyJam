using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonController : MonoBehaviour
{
    [SerializeField] private int _level;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _requirementText;
    [SerializeField] private GameObject _showLevel;
    [SerializeField] private GameObject _showLock;
    [SerializeField] private GameObject _pumpkins;
    [SerializeField] private GameObject _pumpkinIndicatorPrefab;
    [SerializeField] private Image _image;
    [SerializeField] private Color _completeColor;
    [SerializeField] private Color _lockColor;
    [SerializeField] private Button _button;

    // Start is called before the first frame update
    void Start()
    {
        _levelText.text = "World " + _level;
        var worldData = SaveDataManager.Instance.GetWorldData(_level - 1);
        if (worldData != null && worldData.Unlocked)
        {
            _showLevel.SetActive(true);
            _showLock.SetActive(false);
            _button.enabled = true;
            GeneratePumpkins(worldData);

            if (worldData.Completed)
            {
                _image.color = _completeColor;
            }
        }
        else
        {
            _showLevel.SetActive(false);
            _showLock.SetActive(true);
            _image.color = _lockColor;
            _button.enabled = false;
        }
    }

    private void GeneratePumpkins(WorldData worldData)
    {
        var xPos = -60;
        var yPos = 20;
        var step = 40;
        var xThresh = 75;
        for (var i = 0; i < worldData.Levels.Count; i++)
        {
            var level = worldData.Levels[i];
            for (var j = 0; j < level.PumpkinsFound.Length; j++)
            {
                var pumpkin = Instantiate(_pumpkinIndicatorPrefab, _pumpkins.transform);
                if (!level.PumpkinsFound[j])
                {
                    pumpkin.GetComponent<Image>().color = Color.black;
                }
                
                pumpkin.transform.localPosition = new Vector3(xPos, yPos, 0);
                xPos += step;
                if (xPos > xThresh)
                {
                    xPos = -60;
                    yPos -= step;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        GameManager.Instance.LoadWorld(_level);
    }
}
