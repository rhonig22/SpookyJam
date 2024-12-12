using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private readonly string _titleScene = "Title";
    private readonly string _settingsScene = "SettingsScene";
    private readonly string _levelMenu = "LevelMenu";
    private readonly string _levelScene = "Level";
    [SerializeField] private List<ScriptableWorld> _worldList;
    public int CurrentLevel { get; private set; } = 0;
    public int CurrentWorld { get; private set; } = 0;

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
        var vals = scene.name.Split('_');
        if (vals[0] == _levelScene && vals.Length == 3)
        {
            CurrentWorld = int.Parse(vals[1]);
            CurrentLevel = int.Parse(vals[2]);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == _levelMenu || sceneName == _settingsScene)
                LoadTitleScreen();
            else if (sceneName == _titleScene)
                Application.Quit();
            else
                LoadLevelMenu();
        }
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
        LoadLevelMenu();
    }

    public bool HasNextWorld(int world)
    {
        if (world + 1 < _worldList.Count)
            return true;

        return false;
    }

    public int GetPumpkinCount(int world)
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

    public void LoadNextLevel()
    {
        CurrentLevel++;
        // subtract 1 from world and level to be 0-based
        if (CurrentLevel <= _worldList[CurrentWorld-1].GetLevelCount())
        {
            var scriptableLevel = _worldList[CurrentWorld - 1].GetLevel(CurrentLevel - 1);
            var nextLevel = scriptableLevel.GetSceneName();
            SceneManager.LoadScene(nextLevel);
            SaveDataManager.Instance.StartLevel(CurrentWorld-1, CurrentLevel-1, scriptableLevel.GetPumpkinCount());
        }
        else
            FinishWorld(CurrentWorld);
    }

    public void LoadWorld(int world)
    {
        CurrentWorld = world;
        CurrentLevel = 0;
        LoadNextLevel();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadEndScreen()
    {
        SceneManager.LoadScene("EndTitle");
    }

    public void LoadLevelMenu()
    {
        SceneManager.LoadScene(_levelMenu);
    }
}