using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private const string _titleScene = "Title";
    private const string _settingsScene = "SettingsScene";
    private const string _levelMenu = "LevelMenu";
    private const string _worldMenu = "WorldMenu";
    private const string _levelScene = "Level";
    private const string _worldHallwayScene = "World";
    private const string _loadableLevel = "LoadableLevel";
    private const string _overworld = "Overworld";
    [SerializeField] private List<ScriptableWorld> _worldList;
    [SerializeField] private bool _isMenuSystem = false;
    public int CurrentLevel { get; private set; } = 0;
    public int CurrentWorld { get; private set; } = 0;
    public int CurrentEntrance { get; private set; } = -1;
    public string CurrentLevelName { get; private set; } = "";
    public bool IsNewGame { get; private set; } = false;
    private string _lastScene = "";
    private string _currentScene = "";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += LevelLoaded;
    }

    private void ClearGameStates()
    {
        CurrentEntrance = -1;
        CurrentLevel = 0;
        CurrentWorld = 0;
        CurrentLevelName = string.Empty;
    }

    // called second
    private void LevelLoaded(Scene scene, LoadSceneMode mode)
    {
        var sceneName = scene.name;
        var levels = SaveDataManager.ParseLevelName(sceneName);
        if (levels[0] != -1)
        {
            CurrentWorld = levels[0];
            CurrentLevel = levels[1];
        }

        _lastScene = _currentScene;
        _currentScene = sceneName;

        //TODO Remove the World_3 condition post-demo
        if (CurrentEntrance != -1 && (sceneName == _overworld || (sceneName.StartsWith(_worldHallwayScene) && sceneName != "World_3")))
            SaveDataManager.Instance.SetPlayerLocation(sceneName, CurrentEntrance);
    }

    private bool IsPlayableScene()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        return sceneName == _overworld || sceneName == _loadableLevel || sceneName.StartsWith(_worldHallwayScene) || sceneName.StartsWith(_levelScene);
    }

    #region Scriptable World Info Access
    public void SetWorldList(List<ScriptableWorld> worldList)
    {
        _worldList = worldList;
    }

    public int GetWorldCount()
    {
        return _worldList.Count; 
    }

    public int GetCurrentWorldLevelCount()
    {
        return GetLevelCount(CurrentWorld - 1);
    }

    public int GetLevelCount(int world)
    {
        return _worldList[world].GetLevelCount();
    }

    public int GetPumpkinsInLevel(int world, int level)
    {
        return _worldList[world].GetLevel(level).GetPumpkinCount();
    }
    #endregion

    public void LoadSettings()
    {
        LoadScene(_settingsScene);
    }

    public void LoadTitleScreen()
    {
        ClearGameStates();
        LoadScene(_titleScene);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void FinishWorld(int world)
    {
        SaveDataManager.Instance.CompleteWorld(world - 1);
    }

    public bool HasNextWorld(int world)
    {
        if (world + 1 < _worldList.Count)
            return true;

        return false;
    }

    public int GetPumpkinCount(int level)
    {
        var scriptableWorld = _worldList[CurrentWorld - 1];
        if (level >= scriptableWorld.GetLevelCount())
            return 0;

        var scriptableLevel = scriptableWorld.GetLevel(level);
        return scriptableLevel.GetPumpkinCount();
    }

    public int GetPumpkinCountForWorld(int world)
    {
        if (world >= _worldList.Count)
            return 0;

        var scriptableWorld = _worldList[world];
        var levels = scriptableWorld.GetLevelCount();
        var count = 0;
        for (int i = 0; i < levels; i++)
        {
            var level = scriptableWorld.GetLevel(i);
            count += level.GetPumpkinCount();
        }

        return count;
    }

    public void FinishLevel()
    {
        SaveDataManager.Instance.CompleteLevel(CurrentWorld-1, CurrentLevel - 1);
        // subtract 1 from world and level to be 0-based
        if (CurrentLevel == _worldList[CurrentWorld - 1].GetLevelCount())
        {
            FinishWorld(CurrentWorld);
        }

        if (_isMenuSystem)
            LoadLevelMenuForWorld(CurrentWorld);
        else
            LoadWorldHallway(CurrentWorld, CurrentEntrance);
    }

    public void LoadLevel(int level)
    {
        CurrentLevel = level;
        // subtract 1 from world and level to be 0-based
        if (level <= _worldList[CurrentWorld-1].GetLevelCount())
        {
            var scriptableLevel = _worldList[CurrentWorld - 1].GetLevel(CurrentLevel - 1);
            var nextLevel = scriptableLevel.GetSceneName();
            CurrentLevelName = nextLevel;
            SaveDataManager.Instance.SetPlayerLocation(SceneManager.GetActiveScene().name, CurrentEntrance);
            SceneManager.LoadScene(_loadableLevel);
            SaveDataManager.Instance.StartLevel(CurrentWorld-1, CurrentLevel-1, scriptableLevel.GetPumpkinCount());
        }
    }

    public void SetWorld(int world)
    {
        CurrentWorld = world;
    }

    public void SetEntrance(int entrance)
    {
        CurrentEntrance = entrance;
    }

    public void LoadWorldHallway(int world, int entranceNumber)
    {
        CurrentEntrance = entranceNumber;
        SceneManager.LoadScene(_worldHallwayScene + "_" + world);
    }

    public void LoadLevelMenuForWorld(int world)
    {
        CurrentWorld = world;
        CurrentLevel = 0;
        SceneManager.LoadScene(_levelMenu);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadOverworld()
    {
        SceneManager.LoadScene(_overworld);
    }

    public void LoadEndScreen()
    {
        SceneManager.LoadScene("EndTitle");
    }

    public void LoadWorldMenu()
    {
        SceneManager.LoadScene(_worldMenu);
    }

    public void LoadSaveSelect()
    {
        SceneManager.LoadScene("SaveSelectScreen");
    }

    public void StartGame()
    {
        var playerLocation = SaveDataManager.Instance.GetPlayerLocation();
        if (playerLocation == null)
        {
            IsNewGame = true;
            SceneManager.LoadScene(_overworld);
        }
        else
        {
            CurrentEntrance = playerLocation.Entrance;
            SceneManager.LoadScene(playerLocation.Scene);
        }
    }

    public void ClearNewGame()
    {
        IsNewGame = false; 
    }

    public void ReturnToGame()
    {
        if (_lastScene == _titleScene || _lastScene == string.Empty)
            LoadTitleScreen();
        else
            StartGame();
    }
}