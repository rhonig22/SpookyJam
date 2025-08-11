using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public bool Inverted { get; private set; } = false;

    [SerializeField] private TilemapCollider2D _blocks;
    [SerializeField] private TilemapCollider2D _inverseBlocks;
    [SerializeField] private Tilemap _foregroundTiles;
    [SerializeField] private Tilemap _metalTiles;

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

    public bool VisibleTilemapContainsPoint(Vector3 point)
    {
        Vector3Int cellPos = _foregroundTiles.WorldToCell(point);
        TileBase tile = _foregroundTiles.GetTile(cellPos);
        TileBase metalTile = _metalTiles.GetTile(cellPos);
        return tile != null || metalTile != null;
    }
}
