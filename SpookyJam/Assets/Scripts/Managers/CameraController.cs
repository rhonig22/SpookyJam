using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private const float _offsetPanAmount = -2f, _offsetPanTime = .5f, _speedThreshold = 3.25f;
    [SerializeField] private CinemachineVirtualCamera _mainCamera;
    private CinemachineFramingTransposer _transposer;
    private CinemachineBasicMultiChannelPerlin _followNoisePerlin;
    private Rigidbody2D _playerRb;
    private readonly float _shakeAmplitude = 5f, _shakeFrequency = 2f, _shakeTime = .5f;
    private float _shakeTimeElapsed = 0, _currentZoom = 0;
    private bool _isShaking = false, _isLerpingDamping = false, _lerpedDamping = false;

    private void Awake()
    {
        Instance = this;
        _currentZoom = _mainCamera.m_Lens.OrthographicSize;
        _followNoisePerlin = _mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _transposer = _mainCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag(LevelEntityType.Ghost.ToString());
        if (player != null)
            _playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isShaking)
        {
            _shakeTimeElapsed += Time.deltaTime;
            if (_shakeTimeElapsed > _shakeTime)
            {
                StopShake();
            }
        }

        if (_playerRb != null)
        {
            float verticalVelocity = _playerRb.velocity.y;
            if (Mathf.Abs(verticalVelocity) > _speedThreshold && !_isLerpingDamping && !_lerpedDamping)
                StartDampingY(true);

            if (Mathf.Abs(verticalVelocity) < _speedThreshold && !_isLerpingDamping && _lerpedDamping)
            {
                _transposer.m_TrackedObjectOffset.y = 0;
                _lerpedDamping = false;
            }
        }
    }

    private void StartDampingY(bool isFalling)
    {
        _isLerpingDamping = true;
        StartCoroutine(LerpYOffset(isFalling));
    }

    private IEnumerator LerpYOffset(bool isFalling)
    {
        float verticalVelocity = _playerRb.velocity.y;
        float startDamp = _transposer.m_TrackedObjectOffset.y;
        float endDamp = 0;
        if (isFalling)
        {
            endDamp = _offsetPanAmount;
        }
        
        float elapsedTime = 0f;
        while (elapsedTime < _offsetPanTime)
        {
            elapsedTime += Time.deltaTime;
            float targetOffsetY = Mathf.Lerp(
                startDamp,
                endDamp,
                elapsedTime / _offsetPanTime
            );

            _transposer.m_TrackedObjectOffset.y = targetOffsetY;
            yield return null;
        }

        _isLerpingDamping = false;
        _lerpedDamping = isFalling;
    }

    public void InvertScreenY()
    {
        _transposer.m_ScreenY = 1 - _transposer.m_ScreenY;
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
        return levelCamera;
    }

    public void SetLevelCamera(LevelCamera levelCamera)
    {
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

        GameObject player = GameObject.FindGameObjectWithTag(LevelEntityType.Ghost.ToString());
        if (player != null)
        {
            _playerRb = player.GetComponent<Rigidbody2D>();
            _mainCamera.Follow = player.transform;
        }
    }
}
