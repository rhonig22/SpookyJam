using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private const float _offsetPanAmount = -2f, _offsetPanTime = .5f, _speedThreshold = 3.25f;
    [SerializeField] private CinemachineVirtualCamera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera _focusCamera;
    private CinemachineFramingTransposer _transposer;
    private CinemachineBasicMultiChannelPerlin _followNoisePerlin;
    private readonly float _shakeAmplitude = 5f, _shakeFrequency = 2f, _shakeTime = .5f;
    private float _shakeTimeElapsed = 0, _currentZoom = 0;
    private bool _isShaking = false, _lerpedDamping = false;
    public UnityEvent CameraValuesChanged = new UnityEvent();
    private Vector3 _currentPos;

    private void Awake()
    {
        Instance = this;
        _currentZoom = _mainCamera.m_Lens.OrthographicSize;
        _followNoisePerlin = _mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _transposer = _mainCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update()
    {
        if (_mainCamera != null && _mainCamera.transform.position != _currentPos)
        {
            _currentPos = _mainCamera.transform.position;
            Debug.Log("Current cam pos: " + _currentPos);
        }

        if (_isShaking)
        {
            _shakeTimeElapsed += Time.deltaTime;
            if (_shakeTimeElapsed > _shakeTime)
            {
                StopShake();
            }
        }
    }

    public void ShakeCamera()
    {
        _followNoisePerlin.m_AmplitudeGain = _shakeAmplitude;
        _followNoisePerlin.m_FrequencyGain = _shakeFrequency;
        _shakeTimeElapsed = 0;
        _isShaking = true;
    }

    private void StopShake()
    {
        _followNoisePerlin.m_AmplitudeGain = 0;
        _followNoisePerlin.m_FrequencyGain = 0;
        _isShaking = false;
    }

    public LevelCamera GetLevelCamera()
    {
        LevelCamera levelCamera = new LevelCamera();
        levelCamera.Position = _mainCamera.transform.position;
        levelCamera.LensOrtho = _mainCamera.m_Lens.OrthographicSize;
        levelCamera.biasX = _transposer.m_BiasX;
        levelCamera.biasY = _transposer.m_BiasY;
        levelCamera.deadZoneHeight = _transposer.m_DeadZoneHeight;
        levelCamera.deadZoneWidth = _transposer.m_DeadZoneWidth;
        levelCamera.softZoneHeight = _transposer.m_SoftZoneHeight;
        levelCamera.softZoneWidth = _transposer.m_SoftZoneWidth;
        levelCamera.xDamping = _transposer.m_XDamping;
        levelCamera.yDamping = _transposer.m_YDamping;
        levelCamera.screenX = _transposer.m_ScreenX;
        levelCamera.screenY = _transposer.m_ScreenY;
        levelCamera.lookAheadTime = _transposer.m_LookaheadTime;
        levelCamera.lookAheadSmoothing = _transposer.m_LookaheadSmoothing;
        levelCamera.lookAheadIgnoreY = _transposer.m_LookaheadIgnoreY;
        return levelCamera;
    }

    public void DisableVCam()
    {
        _mainCamera.enabled = false;
    }

    public void SetLevelCamera(LevelCamera levelCamera)
    {
        GameObject player = GameObject.FindGameObjectWithTag(LevelEntityType.Ghost.ToString());
        if (player != null)
        {
            _mainCamera.Follow = player.transform;
            _focusCamera.LookAt = player.transform;
        }

        _mainCamera.transform.position = levelCamera.Position;
        _mainCamera.m_Lens.OrthographicSize = levelCamera.LensOrtho;
        _transposer.m_BiasX = levelCamera.biasX;
        _transposer.m_BiasY = levelCamera.biasY;
        _transposer.m_DeadZoneHeight = levelCamera.deadZoneHeight;
        _transposer.m_DeadZoneWidth = levelCamera.deadZoneWidth;
        _transposer.m_SoftZoneHeight = levelCamera.softZoneHeight;
        _transposer.m_SoftZoneWidth = levelCamera.softZoneWidth;
        _transposer.m_XDamping = levelCamera.xDamping;
        _transposer.m_YDamping = levelCamera.yDamping;
        _transposer.m_ScreenX = levelCamera.screenX;
        _transposer.m_ScreenY = levelCamera.screenY;
        _transposer.m_LookaheadIgnoreY = levelCamera.lookAheadIgnoreY;
        _transposer.m_LookaheadSmoothing = levelCamera.lookAheadSmoothing;
        _transposer.m_LookaheadTime = levelCamera.lookAheadTime;
        _mainCamera.enabled = true;
        _mainCamera.OnTargetObjectWarped(_mainCamera.Follow, Vector3.zero);

        CameraValuesChanged.Invoke();
        Debug.Log("Setting Camera position: " + _mainCamera.transform.position);
    }

    public void SetFocusCamera()
    {
        _focusCamera.Priority = 100;
    }
}
