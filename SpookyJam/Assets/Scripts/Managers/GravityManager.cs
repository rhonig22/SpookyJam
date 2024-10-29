using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance;
    public bool Inverted { get; private set; } = false;

    [SerializeField] private GameObject _player;
    private GameObject[] _blocks, _invisiBlocks;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _blocks = GameObject.FindGameObjectsWithTag("Block");
        _invisiBlocks = GameObject.FindGameObjectsWithTag("InvisiBlock");

        foreach (var invisiBlock in _invisiBlocks)
        {
            invisiBlock.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void FlipGravity()
    {
        Inverted = !Inverted;

        foreach (var block in _blocks)
        {
            block.GetComponent<BoxCollider2D>().enabled = !Inverted;
        }

        foreach (var invisiBlock in _invisiBlocks)
        {
            invisiBlock.GetComponent<BoxCollider2D>().enabled = Inverted;
        }
    }
}
