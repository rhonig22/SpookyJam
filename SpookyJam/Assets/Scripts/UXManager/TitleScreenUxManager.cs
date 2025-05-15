using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenUxManager : MonoBehaviour
{

    public void StartButtonPressed()
    {
        GameManager.Instance.LoadWorldMenu();
    }

    public void SettingsPressed()
    {
        GameManager.Instance.LoadSettings();
    }

    public void LoadWorldPressed(int world)
    {
        GameManager.Instance.LoadWorld(world);
    }

    public void MainMenuClicked()
    {
        GameManager.Instance.LoadTitleScreen();
    }

    public void BackToWorldMenu()
    {
        GameManager.Instance.LoadWorldMenu();
    }
}
