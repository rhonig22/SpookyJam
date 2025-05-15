using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager Instance { get; private set; }
    private const string _playerDataKey = "PlayerData";
    private const string _levelScene = "Level";
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
    }

    private string GetLevelDataPath()
    {
        return Application.persistentDataPath + "/levels.fun";
    }

    private void SaveLevelData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetLevelDataPath();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, _levelList);
        stream.Close();
    }

    private void LoadLevelData()
    {
        string path = GetLevelDataPath();
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

    private void ClearLevelData()
    {
        string path = GetLevelDataPath();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public void StartLevel(int world, int level, int pumpkins)
    {
        if (_levelList.Worlds[world].Levels.Count <= level)
            _levelList.Worlds[world].AddLevel(pumpkins);
    }

    public void CompleteWorld(int world)
    {
        var worldData = GetWorldData(world);
        worldData.Completed = true;
        CheckNextWorld(world);
        SaveLevelData();
    }

    public void CompleteLevel(int world, int level)
    {
        var worldData = GetWorldData(world);
        worldData.Levels[level].Completed = true;
        SaveLevelData();
    }

    private void CheckNextWorld(int world)
    {
        if (GameManager.Instance.HasNextWorld(world) && world + 1 == _levelList.Worlds.Count)
        {
            _levelList.Worlds.Add(new WorldData());
        }
    }

    public void InitializeLevelData()
    {
        LevelList levelList = new LevelList()
        {
            Worlds = new List<WorldData>()
            {
                new WorldData()
            }
        };

        _levelList = levelList;
        SaveLevelData();
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        ClearLevelData();
        SetUpDataManager();
    }
}
