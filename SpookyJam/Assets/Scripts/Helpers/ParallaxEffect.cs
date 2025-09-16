using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] CinemachineCamera _camera;
    [SerializeField] float _parallaxEffect;
    private readonly float _camSize = 7;
    private float _startPos, _length;

    private void Start()
    {
        if (CameraController.Instance != null)
            CameraController.Instance.CameraValuesChanged.AddListener(() => ResetParallax());

        ResetParallax();
    }

    private void ResetParallax()
    {
        if (_camera == null)
            return;

        _startPos = _camera.transform.position.x;
        transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, transform.position.z);
        var newCamSize = _camera.Lens.OrthographicSize;
        if (newCamSize != _camSize)
            transform.localScale = transform.localScale * (newCamSize / _camSize);
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_camera == null)
            return;

        var distance = (_camera.transform.position.x - _startPos) * _parallaxEffect;
        var move = _camera.transform.position.x * (1 - _parallaxEffect);
        transform.position = new Vector3(_startPos + distance, _camera.transform.position.y, transform.position.z);

        if (move > _startPos + _length)
        {
            _startPos += _length;
        }
        else if (move < _startPos - _length)
        {
            _startPos -= _length;
        }

        SnapToPixelGrid();
    }

    private void SnapToPixelGrid()
    {
        float ppu = 16f;
        Vector3 pos = transform.position;
        transform.position = new Vector3(
            Mathf.Round(pos.x * ppu) / ppu,
            Mathf.Round(pos.y * ppu) / ppu,
            pos.z
        );
    }
}
