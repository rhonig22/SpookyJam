using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] private CinemachineVirtualCamera _mainCamera;
    private CinemachineBasicMultiChannelPerlin _followNoisePerlin;
    private readonly float _shakeAmplitude = 5f, _shakeFrequency = 2f, _shakeTime = .5f;
    private float _shakeTimeElapsed = 0;
    private float _currentZoom = 0;
    private bool _isShaking = false;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentZoom = _mainCamera.m_Lens.OrthographicSize;
        _followNoisePerlin = _mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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
        return levelCamera;
    }

    public void SetLevelCamera(LevelCamera levelCamera)
    {
        _mainCamera.transform.position = levelCamera.Position;
        _mainCamera.m_Lens.OrthographicSize = levelCamera.LensOrtho;
    }
}
