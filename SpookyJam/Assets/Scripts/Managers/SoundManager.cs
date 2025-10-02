using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private float _volume;
    private readonly float _maxVolume = .6f;
    [SerializeField] private AudioSource _soundEffectsSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _volume = SoundManager.Instance.GetCurrentVolume();
    }

    private void Start()
    {
        _volume = SaveDataManager.Instance.GetPlayerData().SoundFxVolume;
    }

    public AudioSource PlaySound(AudioClip clip, Vector3 position, float pitch = 1)
    {
        AudioSource audioSource = Instantiate(_soundEffectsSource, position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = _volume * _maxVolume;
        audioSource.pitch = pitch;
        audioSource.Play();
        float clipLength = clip.length;
        DontDestroyOnLoad(audioSource.gameObject);
        Destroy(audioSource.gameObject, clipLength);
        return audioSource;
    }

    public void ChangeMasterVolume(float volume)
    {
        _volume = volume;
        var playerData = SaveDataManager.Instance.GetPlayerData();
        playerData.SoundFxVolume = volume;
        SaveDataManager.Instance.SetPlayerData(playerData);
    }

    public float GetCurrentVolume() { return _volume; }
}
