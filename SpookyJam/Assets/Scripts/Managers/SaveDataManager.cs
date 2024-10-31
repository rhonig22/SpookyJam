using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager Instance;
    private readonly string _playerDataKey = "PlayerData";
    private readonly string _levelDataKey = "LevelData";
    private PlayerData _playerData;
    private LevelList _levelList;

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

        if (PlayerPrefs.HasKey(_levelDataKey))
        {
            _levelList = JsonUtility.FromJson<LevelList>(PlayerPrefs.GetString(_levelDataKey));
        }
        else
        {
            InitializeLevelData();
        }
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

        return _levelList.Worlds[world].Levels[level];
    }


    public WorldData GetWorldData(int world)
    {
        return _levelList.Worlds[world];
    }

    public int GetWorldCount()
    {
        return _levelList.Worlds.Count;
    }

    public int GetLevelCountForWorld(int world)
    {
        return _levelList.Worlds[world].Levels.Count;
    }

    public void UpdatePumpkins()
    {
        SetLevelList();
    }

    private void SetLevelList()
    {
        PlayerPrefs.SetString(_levelDataKey, JsonUtility.ToJson(_levelList));
        PlayerPrefs.Save();
    }

    public void CompleteWorld(int world)
    {
        var worldData = GetWorldData(world);
        worldData.Completed = true;
        CheckUnlocks();
        SetLevelList();
    }

    private void CheckUnlocks()
    {
        var totalComplete = 0;
        for (var i = 0; i < _levelList.Worlds.Count; i++)
            if (_levelList.Worlds[i].Completed)
                totalComplete++;

        for (var i = 0; i < _levelList.Worlds.Count; i++)
        {
            var world = _levelList.Worlds[i];
            if (world.Requirement <= totalComplete)
                world.Unlocked = true;
        }
    }

    public void InitializeLevelData()
    {
        LevelList levelList = new LevelList()
        {
            Worlds = new List<WorldData>()
            {
                new WorldData() {
                    Unlocked = true, 
                    Completed = false, 
                    Requirement = 0,
                    Levels = new List<LevelData> {
                        new LevelData() { },
                        new LevelData() { },
                        new LevelData() { } ,
                    }
                },
                new WorldData() {
                    Unlocked = false,
                    Completed = false,
                    Requirement = 1,
                    Levels = new List<LevelData> {
                        new LevelData() { },
                        new LevelData() { },
                        new LevelData() { } ,
                    }
                },
                new WorldData() {
                    Unlocked = false,
                    Completed = false,
                    Requirement = 2,
                    Levels = new List<LevelData> {
                        new LevelData() { },
                        new LevelData() { },
                        new LevelData() { } ,
                    }
                },
                new WorldData() {
                    Unlocked = false,
                    Completed = false,
                    Requirement = 3,
                    Levels = new List<LevelData> {
                        new LevelData() { },
                        new LevelData() { },
                        new LevelData() { } ,
                    }
                },
            }
        };

        _levelList = levelList;
        SetLevelList();
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        SetUpDataManager();
    }
}
