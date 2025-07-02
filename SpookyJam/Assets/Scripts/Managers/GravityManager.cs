using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance;
    public bool Inverted { get; private set; } = false;

    [SerializeField] private TilemapCollider2D _blocks;
    [SerializeField] private TilemapCollider2D _inverseBlocks;


    private void Awake()
    {
        Instance = this;
    }

    public void FlipGravity()
    {
        Inverted = !Inverted;
        _blocks.enabled = !Inverted;
        _inverseBlocks.enabled = Inverted;
    }

    public bool TilemapContainsPoint(Vector3 point)
    {
        var map = Inverted ? _inverseBlocks : _blocks;
        var collider = map.GetComponent<CompositeCollider2D>();
        return collider.OverlapPoint(point);
    }
}
