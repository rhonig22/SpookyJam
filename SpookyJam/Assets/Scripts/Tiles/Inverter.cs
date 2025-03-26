using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : MonoBehaviour
{
    [SerializeField] private TileClusterFinder _finder;
    [SerializeField] private GameObject _particlePrefab;

    private void Start()
    {
        _finder.FindTileClusters();
        CreateParticlesFromClusters();
    }

    private void CreateParticlesFromClusters()
    {
        foreach (var cluster in _finder.Clusters)
        {
            var particles = Instantiate(_particlePrefab);
            particles.transform.position = cluster.getCenter();
            var inverterParticles = particles.GetComponent<InverterParticles>();
            inverterParticles.Height = cluster.getHeight();
            inverterParticles.Width = cluster.getWidth();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.GetComponentInParent<PlayerController>();

        if (controller != null)
        {
            controller.InvertGravity();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var controller = collision.GetComponentInParent<PlayerController>();

        if (controller != null)
        {
            controller.InvertGravity();
        }
    }
}
