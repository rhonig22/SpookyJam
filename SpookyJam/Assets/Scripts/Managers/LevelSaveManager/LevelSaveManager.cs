using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class LevelSaveManager : MonoBehaviour
{
    [SerializeField] ScriptableEntityMap _entityMap;
    [SerializeField] CameraController _cameraController;
    [SerializeField] Tilemap _backgoundTileMap;
    [SerializeField] Tilemap _foregroundTileMap;
    [SerializeField] Tilemap _inverterTileMap;
    [SerializeField] TileBase _backgoundTile;
    [SerializeField] TileBase _foregroundTile;
    [SerializeField] TileBase _inverterTile;
    [SerializeField] ReverseTiles _reverseTiles;
    [SerializeField] string _levelToLoad;
    [SerializeField] bool _isLevelLoader;

    // Start is called before the first frame update
    void Start()
    {
        if (!_isLevelLoader)
            SaveLevel();
        else if (_levelToLoad == null || _levelToLoad.Length == 0)
            LoadLevel(GameManager.Instance.CurrentLevelName);
        else
            LoadLevel(_levelToLoad);
    }

    public static int GetPumpkinCount(string levelName)
    {
        SerializableLevel level = LoadFromFile(levelName);

        var count = 0;
        foreach (var entity in level.SerializableEntities)
        {
            if (entity.EntityType == LevelEntityType.Pumpkin)
                count++;
        }

        return count;
    }

    public void SaveLevel()
    {
        SerializableLevel level = new SerializableLevel();

        SerializableTileLayer background = new SerializableTileLayer();
        background.TileType = TileLayerType.Background;
        background.Positions = TileClusterFinder.GetAllTilePositions(_backgoundTileMap);
        level.SerializableTileLayers.Add(background);

        SerializableTileLayer foreground = new SerializableTileLayer();
        foreground.TileType = TileLayerType.Foreground;
        foreground.Positions = TileClusterFinder.GetAllTilePositions(_foregroundTileMap);
        level.SerializableTileLayers.Add(foreground);

        SerializableTileLayer inverter = new SerializableTileLayer();
        inverter.TileType = TileLayerType.Inverter;
        inverter.Positions = TileClusterFinder.GetAllTilePositions(_inverterTileMap);
        level.SerializableTileLayers.Add(inverter);

        foreach (LevelEntityType type in Enum.GetValues(typeof(LevelEntityType)))
        {
            var entities = GetEntitiesFromType(type);
            level.SerializableEntities.AddRange(entities);
        }

        level.Camera = _cameraController.GetLevelCamera();
        SetLevelNameAndNumbers(level);
        SaveToFile(level);
    }

    public void LoadLevel(string levelName)
    {
        SerializableLevel level = LoadFromFile(levelName);
        foreach (var layer in level.SerializableTileLayers)
        {
            Tilemap map;
            TileBase tile;
            switch (layer.TileType)
            {
                case TileLayerType.Background:
                    map = _backgoundTileMap;
                    tile = _backgoundTile;
                    break;
                case TileLayerType.Foreground:
                    map = _foregroundTileMap;
                    tile = _foregroundTile;
                    break;
                case TileLayerType.Inverter:
                    map = _inverterTileMap;
                    _inverterTileMap.gameObject.SetActive(true);
                    tile = _inverterTile;
                    break;
                default:
                    map = null;
                    tile = null;
                    break;
            }

            if (map == null)
                continue;

            PlaceTilePositions(map, tile, layer);
        }

        _reverseTiles.CreateReverseTileMap();

        foreach (var entity in  level.SerializableEntities)
        {
            InstantiateLevelEntity(entity);
        }

        _cameraController.SetLevelCamera(level.Camera);
        PumpkinManager.Instance.ResetPumpkins();
    }

    private void PlaceTilePositions(Tilemap map, TileBase tile, SerializableTileLayer layer)
    {
        foreach (Vector3Int pos in layer.Positions)
        {
            map.SetTile(pos, tile);
        }
    }

    private void InstantiateLevelEntity(LevelEntity entity)
    {
        var prefab = _entityMap.GetPrefabForEntityType(entity.EntityType);
        var entityInstance = Instantiate(prefab, entity.Position, entity.Rotation);
        foreach (MonoBehaviour component in entityInstance.GetComponents<MonoBehaviour>())
        {
            if (component is ILevelEntity levelEntity)
            {
                levelEntity.SetLevelEntity(entity);
            }
        }
    }

    private List<LevelEntity> GetEntitiesFromType(LevelEntityType type)
    {
        var entities = new List<LevelEntity>();
        var foundEntities = GameObject.FindGameObjectsWithTag(type.ToString());
        foreach (var entityObject in foundEntities) {
            LevelEntity entity = new LevelEntity();
            foreach (MonoBehaviour component in entityObject.GetComponents<MonoBehaviour>())
            {
                if (component is ILevelEntity levelEntity)
                {
                    entity = levelEntity.GetLevelEntity();
                }
            }

            entity.EntityType = type;
            entity.Position = entityObject.transform.position;
            entity.Rotation = entityObject.transform.rotation;
            entities.Add(entity);
        }

        return entities;
    }

    private void SetLevelNameAndNumbers(SerializableLevel level)
    {
        var levelName = SceneManager.GetActiveScene().name;
        var vals = levelName.Split('_');
        if (vals[0] != "Level" || vals.Length != 3)
        {
            return;
        }

        level.Name = levelName;
        level.Level = int.Parse(vals[2]);
        level.World = int.Parse(vals[1]);
    }

    private static string GetSerializedLevelPath(string levelName)
    {
        string filePath = Path.Combine(UnityEngine.Application.streamingAssetsPath, "Levels/" + levelName + ".json");
        Debug.Log($"File Path: {filePath}");
        return filePath;
    }

    private void SaveToFile(SerializableLevel level)
    {
        string path = GetSerializedLevelPath(level.Name);
        string json = JsonUtility.ToJson(level, true);
        File.WriteAllText(path, json);
    }

    private static SerializableLevel LoadFromFile(string levelName)
    {
        string filePath = GetSerializedLevelPath(levelName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SerializableLevel levelData = JsonUtility.FromJson<SerializableLevel>(json);
            Debug.Log($"{filePath} loaded!");
            return levelData;
        }
        else
        {
            Debug.Log($"No level file found for {filePath}");
            return null;
        }
    }
}
