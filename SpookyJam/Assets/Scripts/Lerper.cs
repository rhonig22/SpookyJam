using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lerper : MonoBehaviour
{
    private readonly float _lerpSpeed = .5f;
    private float _maxTime, _timeVal = 0;
    private bool _isStarted = false;
    public UnityEvent TargetReached = new UnityEvent();
    private Vector3 _initialPosition, _goalPosition;

    private void Update()
    {
        if (_isStarted)
        {
            _timeVal = Mathf.MoveTowards(_timeVal, _maxTime, _lerpSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(_initialPosition, _goalPosition, _timeVal);


            if (transform.position == _goalPosition)
            {
                TargetReached.Invoke();
                TargetReached.RemoveAllListeners();
                _isStarted = false;
            }
        }
    }

    public void StartLerping(Vector2 goalPosition, float maxTime)
    {
        _goalPosition = goalPosition;
        _initialPosition = transform.position;
        _maxTime = maxTime;
        _timeVal = 0;
        _isStarted = true;
    }

    public void PauseLerp()
    {
        _isStarted = false;
    }
}
