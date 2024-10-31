using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PumpkinManager : MonoBehaviour
{
    public static PumpkinManager Instance;
    private bool[] _currentPumpkinsFound;

    [SerializeField] private string _collectibleName = "Pumpkin";
    private GameObject[] _pumpkinList;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SceneManager.sceneLoaded += LevelLoaded;
    }

    private void LevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level_"))
            ResetPumpkins();
    }

    public int GetPumpkinCount()
    {
        return 0;
    }

    public int GetPumpkinsFound()
    {
        int count = 0;
        for (int i = 0; i < _currentPumpkinsFound.Length; i++)
        {
            if (_currentPumpkinsFound[i])
                count++;
        }

        return count;
    }

    public int GetMaxPumpkins()
    {
        return _currentPumpkinsFound.Length;
    }

    private void ResetPumpkins()
    {
        _pumpkinList = GameObject.FindGameObjectsWithTag(_collectibleName);
        var levelName = GameManager.Instance.CurrentWorld + "-" + GameManager.Instance.CurrentLevel;
        var levelData = SaveDataManager.Instance.GetLevelData(levelName);
        if (levelData.PumpkinsFound == null)
        {
            levelData.PumpkinsFound = new bool[_pumpkinList.Length];
            SaveDataManager.Instance.UpdatePumpkins();
        }

        _currentPumpkinsFound = (bool[]) levelData.PumpkinsFound.Clone();
        for (int i = 0; i < _pumpkinList.Length; i++)
        {
            var collectible = _pumpkinList[i].GetComponent<BaseCollectible>();
            if (levelData.PumpkinsFound[collectible.GetIndex()])
                _pumpkinList[i].SetActive(false);
        }
    }

    public void LevelFinished()
    {
        var levelName = GameManager.Instance.CurrentWorld + "-" + GameManager.Instance.CurrentLevel;
        var levelData = SaveDataManager.Instance.GetLevelData(levelName);
        for (int i = 0; i < levelData.PumpkinsFound.Length; i++)
        {
            levelData.PumpkinsFound[i] = _currentPumpkinsFound[i];
        }

        SaveDataManager.Instance.UpdatePumpkins();
    }

    public void PickupCollectible(int index)
    {
        _currentPumpkinsFound[index] = true;
    }
}
