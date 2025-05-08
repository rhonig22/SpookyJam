using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] private CinemachineVirtualCamera _mainCamera;
    private CinemachineFramingTransposer _transposer;
    private CinemachineBasicMultiChannelPerlin _followNoisePerlin;
    private readonly float _shakeAmplitude = 5f, _shakeFrequency = 2f, _shakeTime = .5f;
    private float _shakeTimeElapsed = 0;
    private float _currentZoom = 0;
    private bool _isShaking = false;

    private void Awake()
    {
        Instance = this;
        _currentZoom = _mainCamera.m_Lens.OrthographicSize;
        _followNoisePerlin = _mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _transposer = _mainCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
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
        _mainCamera.Follow = player.transform;
    }
}
