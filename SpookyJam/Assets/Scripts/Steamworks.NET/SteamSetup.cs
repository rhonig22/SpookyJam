using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.LightTransport;

public class SteamSetup : MonoBehaviour
{
    private const string DEATH_STAT = "Deaths_";
    private const string WIN_STAT = "Wins_";
    private const string FINISH_DEMO_STAT = "FinishedDemo";
    private const string PUMPKINS_STAT = "Pumpkins_Total";
    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    protected Callback<UserStatsReceived_t> m_UserStatsReceived;

    // Start is called before the first frame update
    void Start()
    {
        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);
            var init = SteamAPI.Init();
            Debug.Log("initialized: " + init);
        }
    }

    private void Update()
    {
        if (SteamManager.Initialized)
        {
            SteamAPI.RunCallbacks();
        }
    }

    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
            m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        }
    }

    public static void IncrementDeaths(int world, int level)
    {
        if (!SteamManager.Initialized)
            return;

        string statName = DEATH_STAT + world + "_" + level;
        IncrementStat(statName);
    }

    public static void IncrementWins(int world, int level)
    {
        if (!SteamManager.Initialized)
            return;

        string statName = WIN_STAT + world + "_" + level;
        IncrementStat(statName);
    }

    public static void FinishDemo()
    {
        if (!SteamManager.Initialized)
            return;

        IncrementStat(FINISH_DEMO_STAT);
    }

    public static void SetPumpkins(int pumpkins)
    {
        if (!SteamManager.Initialized)
            return;

        SetStat(PUMPKINS_STAT, pumpkins);
    }

    private static void IncrementStat(string statName)
    {
        int current;
        if (SteamUserStats.GetStat(statName, out current))
        {
            current++;
            SteamUserStats.SetStat(statName, current);
            SteamUserStats.StoreStats();
            Debug.Log($"Updated {statName} to {current}");
        }
        else
        {
            Debug.LogWarning($"Failed to get stat: {statName}");
        }
    }

    private static void SetStat(string statName, int value)
    {
        SteamUserStats.SetStat(statName, value);
        SteamUserStats.StoreStats();
        Debug.Log($"Updated {statName} to {value}");
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");
        }
        else
        {
            Debug.Log("Steam Overlay has been closed");
        }
    }

    // This method is called by Steamworks.NET when stats are received
    private void OnUserStatsReceived(UserStatsReceived_t result)
    {
        // Check if the retrieval was successful
        if (result.m_eResult == EResult.k_EResultOK)
        {
            Debug.Log("User stats have been received from Steam.");
            // Stats are now ready. Call your method to get and use them.
        }
        else
        {
            Debug.LogError("Failed to receive user stats from Steam. Result: " + result.m_eResult);
        }
    }
}
