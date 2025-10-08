using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

    private PlayerInput _playerInput;
    private InputAction _pauseAction;
    private bool _isPaused = false;

    void Awake()
    {
        SetupPauseMenu();
    }

    public void SetupPauseMenu()
    {
        var player = GameObject.FindWithTag("Ghost");
        if (player == null) return;

        _playerInput = player.GetComponent<PlayerInput>();
        _pauseAction = _playerInput.actions["Pause"];
        _pauseAction.performed += TogglePause;
        _pauseAction.Enable();
    }

    void OnDisable()
    {
        _pauseAction.Disable();
        _pauseAction.performed -= TogglePause;
        if (_isPaused)
            TimeManager.Instance.Pause(false);
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        _isPaused = !_isPaused;
        _pauseMenu.SetActive(_isPaused);
        TimeManager.Instance.Pause(_isPaused);

        if (_playerInput == null)
            return;

        if (_isPaused)
        {
            _playerInput.SwitchCurrentActionMap("UI");
        }
        else
        {
            _playerInput.SwitchCurrentActionMap("Player");
        }
    }
}
