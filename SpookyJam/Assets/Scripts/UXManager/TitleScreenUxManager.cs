using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenUxManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _levelContainers = new GameObject[0];

    private void Start()
    {
        if (_levelContainers.Length > 0)
        {
            int[] levels = DataManager.Instance.GetLevelsLoaded();
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i] == 1 && i < _levelContainers.Length)
                    _levelContainers[i].SetActive(true);
            }
        }
    }

    public void StartButtonPressed()
    {
        LevelLoader.Instance.LoadLevelMenu();
    }

    public void LoadWorldPressed(int world)
    {
        LevelLoader.Instance.LoadWorld(world);
    }
}
