using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenUxManager : MonoBehaviour
{

    public void StartButtonPressed()
    {
        GameManager.Instance.LoadLevelMenu();
    }

    public void SettingsPressed()
    {
        GameManager.Instance.LoadSettings();
    }

    public void LoadWorldPressed(int world)
    {
        GameManager.Instance.LoadWorld(world);
    }
}
