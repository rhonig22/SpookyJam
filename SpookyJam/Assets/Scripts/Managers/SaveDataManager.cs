using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager Instance { get; private set; }
    private const string _playerDataKey = "PlayerData";
    private const string _levelScene = "Level";
    private PlayerData _playerData;
    [SerializeField] private LevelList _levelList;
    [SerializeField] private SaveMetaData _saveMetaData;
    [SerializeField] private int _currentSave = -1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SetUpDataManager();
    }

    public static int[] ParseLevelName(string levelName)
    {
        var vals = levelName.Split('_');
        int[] level = new int[2] { -1, -1 };
        if (vals[0] == _levelScene && vals.Length == 3)
        {
            level[0] = int.Parse(vals[1]);
            level[1] = int.Parse(vals[2]);
        }

        return level;
    }

    private void SetUpDataManager()
    {
        if (PlayerPrefs.HasKey(_playerDataKey))
        {
            _playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(_playerDataKey));
        }
        else
        {
            InitializePlayerData();
        }

        LoadMetaData();
        if (_currentSave != -1)
            LoadLevelData();
    }

    public PlayerData GetPlayerData()
    {
        return _playerData;
    }

    public void SetPlayerData(PlayerData playerData)
    {
        PlayerPrefs.SetString(_playerDataKey, JsonUtility.ToJson(playerData));
        PlayerPrefs.Save();
    }

    public void SetPlayerLocation(string sceneName, int entranceNumber)
    {
        _levelList.PlayerLocation = new PlayerLocation() { Scene = sceneName, Entrance = entranceNumber };
        SaveLevelData();
    }

    public PlayerLocation GetPlayerLocation()
    {
        return _levelList.PlayerLocation;
    }

    public void InitializePlayerData()
    {
        PlayerData playerData = new PlayerData()
        {
            SoundFxVolume = 1,
            MusicVolume = 1,
            PlayerName = ""
        };
        SetPlayerData(playerData);
        _playerData = playerData;
    }

    public LevelData GetLevelData(string levelName)
    {
        var vals = levelName.Split('-');
        var world = int.Parse(vals[0]) - 1;
        var level = 0;
        if (vals.Length > 1)
            level = int.Parse(vals[1]) - 1;

        if (world < _levelList.Worlds.Count && level < _levelList.Worlds[world].Levels.Count)
            return _levelList.Worlds[world].Levels[level];
        else
            return new LevelData(0);
    }


    public WorldData GetWorldData(int world)
    {
        if (world < _levelList.Worlds.Count)
            return _levelList.Worlds[world];
        else
            return null;
    }

    public void UpdatePumpkins()
    {
        SaveLevelData();
        UpdateSaveMetaData();
    }

    #region File manipulation
    private string GetMetadataPath()
    {
        return Application.persistentDataPath + "/saves.fun";
    }
    private string GetLevelDataPath(int saveNumber)
    {
        return Application.persistentDataPath + "/save" + saveNumber + ".fun";
    }

    private void SaveMetaData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetMetadataPath();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, _saveMetaData);
        stream.Close();
    }

    private void LoadMetaData()
    {
        string path = GetMetadataPath();
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            _saveMetaData = formatter.Deserialize(stream) as SaveMetaData;
            stream.Close();
        }
        else
        {
            InitializeMetaData();
        }
    }

    private void SaveLevelData()
    {
        if (_currentSave == -1 ) return;

        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetLevelDataPath(_currentSave);
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, _levelList);
        stream.Close();
    }

    private void LoadLevelData()
    {
        if (_currentSave == -1 ) return;

        string path = GetLevelDataPath(_currentSave);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            _levelList = formatter.Deserialize(stream) as LevelList;
            stream.Close();
        }
        else
        {
            InitializeLevelData();
        }
    }

    private void ClearLevelData(int saveNumber)
    {
        string path = GetLevelDataPath(saveNumber);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private void ClearMetaData()
    {
        string path = GetMetadataPath();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    #endregion

    #region Save File Actions
    public void StartNewGame(int saveNumber)
    {
        _saveMetaData.Saves[saveNumber].FileStarted = true;
        SaveMetaData();
    }

    public SaveFile GetSaveMetaData(int saveNumber)
    {
        return _saveMetaData.Saves[saveNumber];
    }

    public void UpdateSaveMetaData()
    {
        if (_currentSave == -1) return;
        int pumpkinCount = GetTotalPumpkinCount();
        int levelCount = GetTotalLevelCount();
        var saveData = _saveMetaData.Saves[_currentSave];
        saveData.PumpkinCount = pumpkinCount;
        saveData.LevelCount = levelCount;
        SaveMetaData();
    }
    #endregion

    public void StartLevel(int world, int level, int pumpkins)
    {
        if (_levelList.Worlds[world].Levels.Count <= level)
            _levelList.Worlds[world].AddLevel(pumpkins);
    }

    public void OpenSave(int saveNumber)
    {
        _currentSave = saveNumber;
        if (!_saveMetaData.Saves[_currentSave].FileStarted)
            StartNewGame(saveNumber);

        LoadLevelData();
    }

    public void UnlockWorld(int world)
    {
        var worldData = GetWorldData(world);
        worldData.Unlocked = true;
        SaveLevelData();
    }

    public void CompleteWorld(int world)
    {
        var worldData = GetWorldData(world);
        worldData.Completed = true;
        CheckNextWorld(world);
        SaveLevelData();
    }

    public bool IsWorldUnlocked(int world)
    {
        return GetWorldData(world).Unlocked;
    }

    public bool IsWorldCompleted(int world)
    {
        return GetWorldData(world).Completed;
    }

    public void UnlockLevel(int world, int level)
    {
        var worldData = GetWorldData(world);
        worldData.Levels[level].Unlocked = true;
        SaveLevelData();
    }

    public void CompleteLevel(int world, int level)
    {
        var worldData = GetWorldData(world);
        worldData.Levels[level].Completed = true;
        SaveLevelData();
        UpdateSaveMetaData();
    }

    public bool IsLevelUnlocked(int world, int level)
    {
        return GetWorldData(world).Levels[level].Unlocked;
    }

    public bool IsLevelCompleted(int world, int level)
    {
        return GetWorldData(world).Levels[level].Completed;
    }

    public bool IsLevelHundredPercented(int world, int level)
    {
        var levelData = GetWorldData(world).Levels[level];
        bool completed = levelData.Completed;
        bool allPumpkinsFound = true;
        for (int i = 0; i < levelData.PumpkinsFound.Length; i++)
        {
            allPumpkinsFound = allPumpkinsFound && levelData.PumpkinsFound[i];
        }

        return completed && allPumpkinsFound;
    }

    public int GetTotalPumpkinCount()
    {
        var pumpkinCount = 0;
        for (int i = 0; i < _levelList.Worlds.Count; i++)
        {
            var world = _levelList.Worlds[i];
            for (int j = 0; j < world.Levels.Count; j++)
            {
                var level = world.Levels[j];
                for (int k = 0; k < level.PumpkinsFound.Length; k++)
                {
                    if (level.PumpkinsFound[k])
                        pumpkinCount++;
                }
            }
        }

        return pumpkinCount;
    }

    public int GetTotalLevelCount()
    {
        var levelCount = 0;
        for (int i = 0; i < _levelList.Worlds.Count; i++)
        {
            var world = _levelList.Worlds[i];
            for (int j = 0; j < world.Levels.Count; j++)
            {
                var level = world.Levels[j];
                if (level.Completed)
                    levelCount++;
            }
        }

        return levelCount;
    }

    private void CheckNextWorld(int world)
    {
        if (GameManager.Instance.HasNextWorld(world) && (world + 1 == _levelList.Worlds.Count))
        {
            _levelList.Worlds.Add(new WorldData());
        }
    }

    public void InitializeMetaData()
    {
        SaveMetaData saveMetaData = new SaveMetaData();
        _saveMetaData = saveMetaData;
        SaveMetaData();
    }

    public void InitializeLevelData()
    {
        LevelList levelList = new LevelList()
        {
            Worlds = new List<WorldData>()
            {
            }
        };

        int worlds = GameManager.Instance.GetWorldCount();
        for (int i = 0; i < worlds; i++)
        {
            var worldData = new WorldData();
            int levels = GameManager.Instance.GetLevelCount(i);
            for (int j = 0; j < levels; j++)
            {
                worldData.AddLevel(GameManager.Instance.GetPumpkinsInLevel(i, j));
            }

            levelList.Worlds.Add(worldData);
        }

        // unlock the first level
        levelList.Worlds[0].Unlocked = true;
        levelList.Worlds[0].Levels[0].Unlocked = true;

        _levelList = levelList;
        SaveLevelData();
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        for (int i = 0; i < _saveMetaData.Saves.Count; i++)
            ClearLevelData(i);
        ClearMetaData();
        SetUpDataManager();
    }
}
