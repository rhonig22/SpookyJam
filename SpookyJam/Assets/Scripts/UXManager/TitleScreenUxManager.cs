using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenUxManager : MonoBehaviour
{
    public void StartButtonPressed()
    {
        LevelLoader.Instance.LoadNextLevel();
    }
}
