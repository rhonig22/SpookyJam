using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ReverseTiles : MonoBehaviour
{
    [SerializeField] private Tilemap _foregroundTileMap;
    [SerializeField] private Tilemap _reverseTileMap;
    [SerializeField] private TileBase _reverseTile;

    private void Start()
    {
        CreateReverseTileMap();
    }

    public void CreateReverseTileMap()
    {
        var positions = TileClusterFinder.GetAllTilePositions(_foregroundTileMap);
        foreach (var position in positions)
        {
            var up = position + Vector3Int.up;
            var down = position + Vector3Int.down;
            var left = position + Vector3Int.left;
            var right = position + Vector3Int.right;
            if (!_foregroundTileMap.HasTile(up) && !_reverseTileMap.HasTile(up))
                _reverseTileMap.SetTile(up, _reverseTile);
            if (!_foregroundTileMap.HasTile(down) && !_reverseTileMap.HasTile(down))
                _reverseTileMap.SetTile(down, _reverseTile);
            if (!_foregroundTileMap.HasTile(left) && !_reverseTileMap.HasTile(left))
                _reverseTileMap.SetTile(left, _reverseTile);
            if (!_foregroundTileMap.HasTile(right) && !_reverseTileMap.HasTile(right))
                _reverseTileMap.SetTile(right, _reverseTile);
        }
    }
}
