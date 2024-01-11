using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;
    public int CurrentLevel = 0;
    public int CurrentWorld = 0;
    public readonly int[] LevelCount = {3,3,0};

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void FinishWorld(int world)
    {
        DataManager.Instance.CompleteWorld(world);
        if (LevelCount[world+1] > 0)
            LoadLevelMenu();
        else
            LoadEndScreen();
    }

    public void LoadNextLevel()
    {
        CurrentLevel++;
        var nextLevel = "Level_" + (CurrentWorld + 1) + "_" + CurrentLevel;
        if (CurrentLevel <= LevelCount[CurrentWorld])
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
