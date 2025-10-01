using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _playButton;
    [SerializeField] private PauseManager _manager;

    private void OnEnable()
    {
        StartCoroutine(SetFirstSelected());
    }

    private IEnumerator SetFirstSelected()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_playButton);
    }

    public void PlayClicked()
    {
        _manager.TogglePause(new InputAction.CallbackContext());
    }

    public void LevelSelectClicked()
    {
        GameManager.Instance.StartGame();
    }

    public void MenuClicked()
    {
        GameManager.Instance.LoadTitleScreen();
    }

    public void SettingsClicked()
    {
        GameManager.Instance.LoadSettings();
    }

    public void QuitClicked()
    {
        Application.Quit();
    }
}
