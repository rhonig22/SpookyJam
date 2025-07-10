using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public LevelCamera Camera;

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
    public Vector3 Endpoint1;
    public Vector3 Endpoint2;
    public Vector3 EntityPoint;
    public bool TowardsPoint1;
    public string Message;
    public int Index;
    public string Tag;
}

[Serializable]
public class LevelCamera
{
    public Vector3 Position;
    public float LensOrtho;
    public float xDamping;
    public float yDamping;
    public float screenX;
    public float screenY;
    public float deadZoneWidth;
    public float deadZoneHeight;
    public float softZoneWidth;
    public float softZoneHeight;
    public float biasX;
    public float biasY;
    public float lookAheadTime;
    public float lookAheadSmoothing;
    public bool lookAheadIgnoreY;
}

public enum TileLayerType
{
    Background,
    Foreground,
    Inverter
}

public enum LevelEntityType
{
    Ghost,
    Tentacle,
    Bat,
    Sign,
    EndPortal,
    Pumpkin,
    Daisy,
    Tombstone
}