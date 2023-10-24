using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;
    public int CurrentLevel = 0;
    public readonly int LevelCount = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextLevel()
    {
        CurrentLevel++;
        var nextLevel = "Level_" + CurrentLevel;
        if (CurrentLevel <= LevelCount)
            SceneManager.LoadScene(nextLevel);
        else
            LoadEndScreen();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadEndScreen()
    {
        SceneManager.LoadScene("EndTitle");
    }
}
