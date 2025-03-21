using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class TileClusterFinder : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    private Dictionary<Vector3Int, bool> _visitedTiles = new Dictionary<Vector3Int, bool>();
    public List<TileCluster> Clusters { get; private set; } = new List<TileCluster>();

    public void FindTileClusters()
    {
        List<List<Vector3Int>> clusters = new List<List<Vector3Int>>();

        // Initialize the visited dictionary
        foreach (Vector3Int pos in GetAllTilePositions(_tilemap))
        {
            _visitedTiles[pos] = false;
        }

        var keys = _visitedTiles.Keys.ToList<Vector3Int>();
        foreach (var pos in keys)
        {
            if (!_visitedTiles[pos] && _tilemap.HasTile(pos))
            {
                // Find a cluster using BFS or DFS
                List<Vector3Int> cluster = new List<Vector3Int>();
                FindClusterDFS(pos, cluster);
                clusters.Add(cluster);
            }
        }

        foreach (var cluster in clusters)
        {
            Clusters.Add(ConvertCellPositionsToTileCLuster(cluster));
        }
    }

    public static List<Vector3Int> GetAllTilePositions(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        List<Vector3Int> tilePositions = new List<Vector3Int>();

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                tilePositions.Add(pos);
            }
        }
        return tilePositions;
    }

    void FindClusterDFS(Vector3Int start, List<Vector3Int> cluster)
    {
        Stack<Vector3Int> stack = new Stack<Vector3Int>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            Vector3Int pos = stack.Pop();
            if (_visitedTiles[pos]) continue;

            if (_visitedTiles.ContainsKey(pos))
                _visitedTiles[pos] = true;

            cluster.Add(pos);

            foreach (Vector3Int neighbor in GetNeighbors(pos))
            {
                if (_tilemap.HasTile(neighbor) && !_visitedTiles[neighbor])
                {
                    stack.Push(neighbor);
                }
            }
        }
    }

    List<Vector3Int> GetNeighbors(Vector3Int pos)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>
        {
            pos + Vector3Int.up,
            pos + Vector3Int.down,
            pos + Vector3Int.left,
            pos + Vector3Int.right
        };

        return neighbors;
    }

    TileCluster ConvertCellPositionsToTileCLuster(List<Vector3Int> cells)
    {
        var cluster = new TileCluster();
        foreach (var cell in cells)
        {
            Vector3 worldPosition = _tilemap.GetCellCenterWorld(cell);
            cluster.AddTile(worldPosition, _tilemap.layoutGrid.cellSize);
        }

        return cluster;
    }
}
