using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWorldDoor : Door
{
    /// <summary>
    /// The 1-based number of the world this hallway exists in, used to check current world levels completed and unlock next world
    /// </summary>
    [SerializeField] private int _world;
    [SerializeField] private int _completedLevelsRequirement;
    [SerializeField] private bool _hasLock;
    [SerializeField] private GameObject _candlePrefab;
    [SerializeField] private Transform[] _candleLocations;
    private List<Candle> _candles = new List<Candle>();
    private bool _allCandlesLit = false;

    private void Start()
    {
        if (!_hasLock)
            return;

        for (var i  = 0; i < _completedLevelsRequirement; i++)
        {
            var candle = GameObject.Instantiate(_candlePrefab);
            candle.transform.position = _candleLocations[i].position;
            candle.transform.parent = _candleLocations[i];
            _candles.Add(candle.GetComponent<Candle>());
        }
    }

    protected override void OpenDoor()
    {
        if (!_hasLock || SaveDataManager.Instance.IsWorldUnlocked(_world))
        {
            SceneTransition.Instance.EndLevelTransition(() => {
                GameManager.Instance.SetEntrance(_entranceNumber);
                GameManager.Instance.LoadOverworld();
            });
        }
    }

    public void HideCandles()
    {
        foreach (var candle in _candles)
        {
            candle.gameObject.SetActive(false);
        }
    }

    public void LightCandles(int count)
    {
        StartCoroutine(LightCandlesWithDelay(count));
    }

    private IEnumerator LightCandlesWithDelay(int count)
    {
        for (var i = 0;i < count;i++)
        {
            _candles[i].LightCandle();
            yield return new WaitForSeconds(.3f);
        }

        if (count == _completedLevelsRequirement)
            _allCandlesLit = true;
    }

    public int GetWorld() { return _world; }
    public bool HasLock() { return _hasLock; }
    public bool AreAllCandlesLit() { return _allCandlesLit; }
    public int CompletedLevelsRequirement() { return _completedLevelsRequirement; }
}
