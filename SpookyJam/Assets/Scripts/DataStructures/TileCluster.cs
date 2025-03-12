using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileCluster
{
    public float Top { get; private set; } = float.MinValue;
    public float Bottom { get; private set; } = float.MaxValue;
    public float Left { get; private set; } = float.MaxValue;
    public float Right { get; private set; } = float.MinValue;
    private List<Vector3> tiles = new List<Vector3>();

    public void AddTile(Vector3 tile, Vector3 tileSize)
    {
        tiles.Add(tile);
        var width = tileSize.x / 2;
        var height = tileSize.y / 2;
        Right = Math.Max(tile.x + width, Right);
        Left = Math.Min(tile.x - width, Left);
        Top = Math.Max(tile.y + height, Top);
        Bottom = Math.Min(tile.y - height, Bottom);
    }

    public float getHeight() { return Top - Bottom; }
    public float getWidth() { return Right - Left; }
    public Vector3 getCenter() { return new Vector3(Left + getWidth()/2, Bottom + getHeight()/2, 0); }
}
