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
    private readonly string _levelScene = "Level";
    private int CurrentLevel = 0;
    private int CurrentWorld = 0;

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
        if (world+1 < SaveDataManager.Instance.GetWorldCount())
            LoadLevelMenu();
        else
            LoadEndScreen();
    }

    public void LoadNextLevel()
    {
        CurrentLevel++;
        var nextLevel = _levelScene + "_" + CurrentWorld + "_" + CurrentLevel;
        if (CurrentLevel <= SaveDataManager.Instance.GetLevelCountForWorld(CurrentWorld))
            SceneManager.LoadScene(nextLevel);
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
        SceneManager.LoadScene("LevelMenu");
    }
}