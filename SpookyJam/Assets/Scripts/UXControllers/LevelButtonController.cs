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
        _levelText.text = "Level " + (_level + 1);
        var worldData = SaveDataManager.Instance.GetWorldData(GameManager.Instance.CurrentWorld - 1);
        if (_level - 1 < worldData.Levels.Count && (_level == 0 || worldData.Levels[_level - 1].Completed))
        {
            var levelData = _level < worldData.Levels.Count ? worldData.Levels[_level] : null;
            _showLevel.SetActive(true);
            _showLock.SetActive(false);
            _button.enabled = true;
            GeneratePumpkins(levelData);

            if (levelData != null && levelData.Completed)
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

    public void SetLevel(int level)
    {
        _level = level;
    }

    private void GeneratePumpkins(LevelData levelData)
    {
        var xPos = -60;
        var yPos = 0;
        var step = 40;
        var xThresh = 75;
        var count = 0;
        if (levelData != null)
        {
            for (var j = 0; j < levelData.PumpkinsFound.Length; j++)
            {
                var pumpkin = Instantiate(_pumpkinIndicatorPrefab, _pumpkins.transform);
                if (!levelData.PumpkinsFound[j])
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

                count++;
            }
        }
        else
        {
            var pumpkinsCount = GameManager.Instance.GetPumpkinCount(_level);
            GenerateBlankPumpkins(pumpkinsCount, xPos, yPos, step, xThresh);
        }
    }

    private void GenerateBlankPumpkins(int pumpkinsCount, int xPos, int yPos, int step, int xThresh)
    {
        for (var j = 0; j < pumpkinsCount; j++)
        {
            var pumpkin = Instantiate(_pumpkinIndicatorPrefab, _pumpkins.transform);
            pumpkin.GetComponent<Image>().color = Color.black;

            pumpkin.transform.localPosition = new Vector3(xPos, yPos, 0);
            xPos += step;
            if (xPos > xThresh)
            {
                xPos = -60;
                yPos -= step;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        // offset by one for Level Number
        GameManager.Instance.LoadLevel(_level + 1);
    }
}
