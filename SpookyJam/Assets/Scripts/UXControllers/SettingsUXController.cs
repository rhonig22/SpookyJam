using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsUXController : MonoBehaviour
{
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _soundSlider;
    [SerializeField] GameObject _confirmationMessage;
    [SerializeField] GameObject _confirmNoButton;
    [SerializeField] GameObject _menuButton;

    private void Start()
    {
        _musicSlider.value = MusicManager.Instance.GetCurrentVolume();
        _soundSlider.value = SoundManager.Instance.GetCurrentVolume();
    }

    public void MainMenuClicked()
    {
        GameManager.Instance.ReturnToGame();
    }

    public void SetMusicVolume()
    {
        MusicManager.Instance.ChangeMasterVolume(_musicSlider.value);
    }
    public float GetMusicVolume()
    {
        return MusicManager.Instance.GetCurrentVolume();
    }
    public void SetSoundFXVolume()
    {
        SoundManager.Instance.ChangeMasterVolume(_soundSlider.value);
    }

    public void ClearDataButtonClicked()
    {
        _confirmationMessage.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_confirmNoButton);
    }

    public void ConfirmClearData()
    {
        SaveDataManager.Instance.ClearData();
        _confirmationMessage.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_menuButton);
    }

    public void CancelClearData()
    {
        _confirmationMessage.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_menuButton);
    }
}
