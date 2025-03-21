using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class SerializableLevel
{
    public List<SerializableTileLayer> SerializableTileLayers;
    public List<LevelEntity> SerializableEntities;
    public string Name;
    public int Level;
    public int World;

    public SerializableLevel()
    {
        SerializableTileLayers = new List<SerializableTileLayer>();
        SerializableEntities = new List<LevelEntity>();
    }
}

[Serializable]
public class SerializableTileLayer
{
    public List<Vector3Int> Positions;
    public TileLayerType TileType;
}

[Serializable]
public class LevelEntity
{
    public Vector3 Position;
    public Quaternion Rotation;
    public LevelEntityType EntityType;
}

public enum TileLayerType
{
    Background,
    Foreground,
    Inverter
}

public enum LevelEntityType
{
    Player,
    Tentacle,
    Bat,
    Sign,
    EndPortal,
    Pumpkin
}