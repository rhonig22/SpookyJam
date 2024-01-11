using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public int TotalPumpkinCount { get; private set; } = 0;
    public int CurrentPumpkinCount { get; private set; } = 0;
    public int CurrentPumpkinMax { get; private set; } = 0;

    private int[] _levelsLoaded = { 1, 0, 0 };

    [SerializeField] private string _collectibleName = "Pumpkin";

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

    private void Start()
    {
        ResetPumpkins();
    }

    public void OnLevelWasLoaded(int level)
    {
        ResetPumpkins();
    }

    private void ResetPumpkins()
    {
        CurrentPumpkinCount = 0;
        CurrentPumpkinMax = GameObject.FindGameObjectsWithTag(_collectibleName).Length;
    }

    public void LevelFinished()
    {
        TotalPumpkinCount += CurrentPumpkinCount;
    }

    public void PickupCollectible(string collectibleName)
    {
        if (collectibleName == _collectibleName)
        {
            CurrentPumpkinCount++;
        }
    }

    public int[] GetLevelsLoaded()
    {
        return _levelsLoaded;
    }

    public void CompleteWorld(int world)
    {
        _levelsLoaded[world + 1] = 1;
    }
}
