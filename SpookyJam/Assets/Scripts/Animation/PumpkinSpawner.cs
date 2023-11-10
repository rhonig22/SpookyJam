using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_pumpkinPrefab;
    private float m_spawnAreaXBound = 5.5f;
    private float m_spawnAreaTopBound = -2.75f;
    private float m_spawnAreaBottomBound = -4.25f;

    private void Start()
    {
        SpawnPumpkins(DataManager.Instance.TotalPumpkinCount);
    }

    private void SpawnPumpkins(int totalPumpkinCount)
    {
        for (int i = 0; i < totalPumpkinCount; i++)
        {
            var pumpkin = Instantiate(m_pumpkinPrefab);
            float x = Random.Range(-1*m_spawnAreaXBound, m_spawnAreaXBound);
            float y = Random.Range(m_spawnAreaBottomBound, m_spawnAreaTopBound);
            pumpkin.transform.position = new Vector3(x, y, 0);
        }
    }
}
