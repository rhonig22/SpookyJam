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
    private const string _loadableLevel = "LoadableLevel";
    [SerializeField] private List<ScriptableWorld> _worldList;
    public int CurrentLevel { get; private set; } = 0;
    public int CurrentWorld { get; private set; } = 0;
    public string CurrentLevelName { get; private set; } = "";

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

    // called second
    private void LevelLoaded(Scene scene, LoadSceneMode mode)
    {
        var levels = SaveDataManager.ParseLevelName(scene.name);
        if (levels[0] != -1)
        {
            CurrentWorld = levels[0];
            CurrentLevel = levels[1];
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == _worldMenu || sceneName == _settingsScene)
                LoadTitleScreen();
            else if (sceneName == _levelMenu)
                LoadWorldMenu();
            else if (sceneName == _titleScene)
                Application.Quit();
            else if (sceneName == _loadableLevel)
                LoadWorld(CurrentWorld);
        }
        else if (Input.GetButtonDown("Restart"))
        {
            SceneTransition.Instance.RestartLevelTransition();
        }
    }

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
        return _worldList[CurrentWorld-1].GetLevelCount();
    }

    public void LoadSettings()
    {
        LoadScene(_settingsScene);
    }

    public void LoadTitleScreen()
    {
        LoadScene(_titleScene);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void FinishWorld(int world)
    {
        SaveDataManager.Instance.CompleteWorld(world - 1);
        /*if (world < _worldList.Count)
            LoadLevelMenu();
        else
            LoadEndScreen();*/
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

        LoadWorld(CurrentWorld);
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
            SceneManager.LoadScene(_loadableLevel);
            SaveDataManager.Instance.StartLevel(CurrentWorld-1, CurrentLevel-1, scriptableLevel.GetPumpkinCount());
        }
    }

    public void LoadWorld(int world)
    {
        CurrentWorld = world;
        CurrentLevel = 0;
        SceneManager.LoadScene(_levelMenu);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadEndScreen()
    {
        SceneManager.LoadScene("EndTitle");
    }

    public void LoadWorldMenu()
    {
        SceneManager.LoadScene(_worldMenu);
    }
}