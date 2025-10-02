using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private const float _offsetPanAmount = -2f, _offsetPanTime = .5f, _speedThreshold = 3.25f, _pixelsPerUnit = 16;
    [SerializeField] private CinemachineCamera _mainCamera;
    [SerializeField] private CinemachineCamera _focusCamera;
    [SerializeField] private CinemachineCamera _secondaryCamera;
    private CinemachinePositionComposer _transposer;
    private CinemachineBasicMultiChannelPerlin _followNoisePerlin;
    private readonly float _shakeAmplitude = 5f, _shakeFrequency = 2f;
    private float _shakeTimeElapsed = 0, _currentZoom = 0, _shakeTime = .5f;
    private bool _isShaking = false;
    public UnityEvent CameraValuesChanged = new UnityEvent();
    private Vector3 _currentPos;

    private void Awake()
    {
        Instance = this;
        _currentZoom = _mainCamera.Lens.OrthographicSize;
        _followNoisePerlin = _mainCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        _transposer = _mainCamera.GetComponent<CinemachinePositionComposer>();
    }

    private void Update()
    {
        if (_mainCamera != null && _mainCamera.transform.position != _currentPos)
        {
            _currentPos = _mainCamera.transform.position;
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

    private void FixedUpdate()
    {
        Vector3 pos = _mainCamera.transform.position;
        _mainCamera.transform.position = new Vector3(
            Mathf.Round(pos.x * _pixelsPerUnit) / _pixelsPerUnit,
            Mathf.Round(pos.y * _pixelsPerUnit) / _pixelsPerUnit,
            pos.z
        );
    }

    public void ShakeCamera(float shakeTime)
    {
        _shakeTime = shakeTime;
        _followNoisePerlin.AmplitudeGain = _shakeAmplitude;
        _followNoisePerlin.FrequencyGain = _shakeFrequency;
        _shakeTimeElapsed = 0;
        _isShaking = true;
    }

    private void StopShake()
    {
        _followNoisePerlin.AmplitudeGain = 0;
        _followNoisePerlin.FrequencyGain = 0;
        _isShaking = false;
    }

    public LevelCamera GetLevelCamera()
    {
        LevelCamera levelCamera = new LevelCamera();
        levelCamera.Position = _mainCamera.transform.position;
        levelCamera.LensOrtho = _mainCamera.Lens.OrthographicSize;
        levelCamera.hardLimitX = _transposer.Composition.HardLimits.Size.x;
        levelCamera.hardLimitY = _transposer.Composition.HardLimits.Size.y;
        levelCamera.deadZoneHeight = _transposer.Composition.DeadZone.Size.y;
        levelCamera.deadZoneWidth = _transposer.Composition.DeadZone.Size.x;
        levelCamera.xDamping = _transposer.Damping.x;
        levelCamera.yDamping = _transposer.Damping.y;
        levelCamera.screenX = _transposer.Composition.ScreenPosition.x;
        levelCamera.screenY = _transposer.Composition.ScreenPosition.y;
        levelCamera.useLookAhead = _transposer.Lookahead.Enabled;
        levelCamera.lookAheadTime = _transposer.Lookahead.Time;
        levelCamera.lookAheadSmoothing = _transposer.Lookahead.Smoothing;
        levelCamera.lookAheadIgnoreY = _transposer.Lookahead.IgnoreY;
        return levelCamera;
    }

    public SecondaryCamera GetSecondaryCamera()
    {
        SecondaryCamera secondaryCamera = new SecondaryCamera();
        secondaryCamera.Position = _secondaryCamera.transform.position;
        secondaryCamera.LensOrtho = _secondaryCamera.Lens.OrthographicSize;
        return secondaryCamera;
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
            _focusCamera.Follow = player.transform;
        }

        _mainCamera.transform.position = levelCamera.Position;
        _mainCamera.Lens.OrthographicSize = levelCamera.LensOrtho;
        _transposer.Composition.HardLimits.Size.x = levelCamera.hardLimitX;
        _transposer.Composition.HardLimits.Size.y = levelCamera.hardLimitY;
        _transposer.Composition.DeadZone.Size.y = levelCamera.deadZoneHeight;
        _transposer.Composition.DeadZone.Size.x = levelCamera.deadZoneWidth;
        _transposer.Damping.x = levelCamera.xDamping;
        _transposer.Damping.y = levelCamera.yDamping;
        _transposer.Composition.ScreenPosition.x = levelCamera.screenX;
        _transposer.Composition.ScreenPosition.y = levelCamera.screenY;
        _transposer.Lookahead.Enabled = levelCamera.useLookAhead;
        _transposer.Lookahead.IgnoreY = levelCamera.lookAheadIgnoreY;
        _transposer.Lookahead.Smoothing = levelCamera.lookAheadSmoothing;
        _transposer.Lookahead.Time = levelCamera.lookAheadTime;
        _mainCamera.enabled = true;
        _mainCamera.OnTargetObjectWarped(_mainCamera.Follow, Vector3.zero);

        CameraValuesChanged.Invoke();
    }

    public void SetSecondaryCamera(SecondaryCamera secondaryCamera)
    {
        _secondaryCamera.transform.position = secondaryCamera.Position;
        _secondaryCamera.Lens.OrthographicSize = secondaryCamera.LensOrtho;
    }

    public void SetFocusCamera()
    {
        _focusCamera.Priority = 100;
    }

    public void SwitchToSecondaryCam()
    {
        _secondaryCamera.Priority = 10;
        _mainCamera.Priority = 0;
    }

    public void SwitchToMainCam()
    {
        _mainCamera.Priority = 10;
        _secondaryCamera.Priority = 0;
    }
}
