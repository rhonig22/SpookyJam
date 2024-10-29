using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private readonly float _maxVolume = .3f;
    private float _volume = .3f;
    [SerializeField] private AudioSource _musicSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _volume = SaveDataManager.Instance.GetPlayerData().MusicVolume * _maxVolume;
        _musicSource.volume = _volume;
    }

    public void StartMusic()
    {
        _musicSource.volume = _volume;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void ChangeMasterVolume(float volume)
    {
        _volume = volume * _maxVolume;
        _musicSource.volume = _volume;
        var playerData = SaveDataManager.Instance.GetPlayerData();
        playerData.MusicVolume = volume;
        SaveDataManager.Instance.SetPlayerData(playerData);
    }

    public float GetCurrentVolume() {
        return _volume / _maxVolume;
    }

    public void PlayMusicClip(AudioClip clip, bool playOnce = false)
    {
        _musicSource.volume = _volume;
        if (playOnce)
        {
            _musicSource.Stop();
            _musicSource.PlayOneShot(clip);
        }
        else
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }
    }

    public void StopMusicClip() {
        _musicSource.Stop();
    }
    public AudioSource getMusicSource()
    {  
        return _musicSource; 
    }    
}
