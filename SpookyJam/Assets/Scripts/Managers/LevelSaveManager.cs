using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelSaveManager : MonoBehaviour
{
    [SerializeField] Tilemap _backgoundTiles;
    [SerializeField] Tilemap _foregroundTiles;
    [SerializeField] Tilemap _inverterTiles;

    // Start is called before the first frame update
    void Start()
    {
        SaveLevel();
    }

    public void SaveLevel()
    {
        SerializableLevel level = new SerializableLevel();

        SerializableTileLayer background = new SerializableTileLayer();
        background.TileType = TileLayerType.Background;
        background.Positions = TileClusterFinder.GetAllTilePositions(_backgoundTiles);
        level.SerializableTileLayers.Add(background);

        SerializableTileLayer foreground = new SerializableTileLayer();
        foreground.TileType = TileLayerType.Foreground;
        foreground.Positions = TileClusterFinder.GetAllTilePositions(_foregroundTiles);
        level.SerializableTileLayers.Add(foreground);

        SerializableTileLayer inverter = new SerializableTileLayer();
        inverter.TileType = TileLayerType.Inverter;
        inverter.Positions = TileClusterFinder.GetAllTilePositions(_inverterTiles);
        level.SerializableTileLayers.Add(inverter);

        foreach (LevelEntityType type in Enum.GetValues(typeof(LevelEntityType)))
        {
            var entities = GetEntitiesFromType(type);
            level.SerializableEntities.AddRange(entities);
        }

        SaveToFile(level);
    }

    private List<LevelEntity> GetEntitiesFromType(LevelEntityType type)
    {
        var entities = new List<LevelEntity>();
        var foundEntities = GameObject.FindGameObjectsWithTag(type.ToString());
        foreach (var entityObject in foundEntities) {
            LevelEntity entity = new LevelEntity();
            entity.EntityType = type;
            entity.Position = entityObject.transform.position;
            entity.Rotation = entityObject.transform.rotation;
            entities.Add(entity);
        }

        return entities;
    }

    private string GetSerializedLevelPath()
    {
        var filePath = Path.Combine(Application.persistentDataPath, "level.json");
        Debug.Log($"File Path: {filePath}");
        return filePath;
    }

    private void SaveToFile(SerializableLevel level)
    {
        string path = GetSerializedLevelPath();
        string json = JsonUtility.ToJson(level, true);
        File.WriteAllText(path, json);
    }
}
