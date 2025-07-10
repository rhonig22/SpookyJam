using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class WebDataLoader : MonoBehaviour
{
    public static void Load<T>(MonoBehaviour context, string filename, Action<T> onSuccess, Action<string> onFail = null)
    {
        context.StartCoroutine(LoadCoroutine(filename, onSuccess, onFail));
    }

    private static IEnumerator LoadCoroutine<T>(string filename, Action<T> onSuccess, Action<string> onFail)
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, filename);
        UnityWebRequest request = UnityWebRequest.Get(path);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            try
            {
                T data = JsonUtility.FromJson<T>(json);
                onSuccess?.Invoke(data);
            }
            catch (Exception e)
            {
                Debug.LogError("JSON parsing error: " + e.Message);
                onFail?.Invoke("Failed to parse JSON");
            }
        }
        else
        {
            Debug.LogError("Failed to load file: " + request.error);
            onFail?.Invoke(request.error);
        }
    }
}