using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class WorldMetadataGenerator
{
    private const string _levelFolder = "Levels";

    [MenuItem("Tools/Generate Level Metadata")]
    public static void GenerateLevelData()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, _levelFolder);

        if (!Directory.Exists(folderPath))
            return;

        string[] files = Directory.GetFiles(folderPath);
        List<ScriptableWorld> worlds = new List<ScriptableWorld>();
        foreach (string file in files)
        {
            var ext = Path.GetExtension(file);
            if (ext != ".json")
                continue;

            var name = Path.GetFileNameWithoutExtension(file);
            var levelVals = SaveDataManager.ParseLevelName(name);
            if (levelVals[0] == -1)
                continue;

            if (levelVals[0] > worlds.Count)
            {
                ScriptableWorld world = ScriptableObject.CreateInstance<ScriptableWorld>();
                worlds.Add(world);

                string worldPath = $"Assets/Scripts/ScriptableLevelMap/Worlds/World_{levelVals[0]}.asset";
                AssetDatabase.CreateAsset(world, worldPath);
            }

            ScriptableLevel level = ScriptableObject.CreateInstance<ScriptableLevel>();
            level.SetSceneName(name);
            level.SetPumpkinCount(LevelSaveManager.GetPumpkinCount(name));

            string levelPath = $"Assets/Scripts/ScriptableLevelMap/Levels/{name}.asset";
            AssetDatabase.CreateAsset(level, levelPath);

            worlds[levelVals[0] -1].AddLevel(level);
            AssetDatabase.SaveAssetIfDirty(worlds[levelVals[0] - 1]);
        }

        string prefabPath = "Assets/Prefabs/Managers/GameManager.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError("Prefab not found!");
            return;
        }

        GameManager gameManager = prefab.GetComponent<GameManager>();
        if (gameManager != null)
        {
            gameManager.SetWorldList(worlds);
        }

        AssetDatabase.SaveAssets();
    }
}
#endif