using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy, ILevelEntity
{
    [SerializeField] private GameObject _endpoint1;
    [SerializeField] private GameObject _endpoint2;
    [SerializeField] private GameObject _bat;
    [SerializeField] private bool _towardsPoint1 = true;

    private readonly float speed = 3f;
    
    private void FixedUpdate()
    {
        var goalPosition = (_towardsPoint1 ? _endpoint1.transform.position : _endpoint2.transform.position);
        var moveDistance = goalPosition - _bat.transform.position;

        _bat.transform.position = Vector3.Lerp(_bat.transform.position, goalPosition, speed * Time.fixedDeltaTime / moveDistance.magnitude);

        if (_bat.transform.position == goalPosition)
        {
            _towardsPoint1 = !_towardsPoint1;
        }
    }

    public LevelEntity GetLevelEntity()
    {
        LevelEntity levelEntity = new LevelEntity();
        return GetLevelEntity(levelEntity);
    }

    public LevelEntity GetLevelEntity(LevelEntity levelEntity)
    {
        levelEntity.Endpoint1 = _endpoint1.transform.position;
        levelEntity.Endpoint2 = _endpoint2.transform.position;
        levelEntity.EntityPoint = _bat.transform.position;
        levelEntity.TowardsPoint1 = _towardsPoint1;
        return levelEntity;
    }

    public void SetLevelEntity(LevelEntity levelEntity)
    {
        _endpoint1.transform.position = levelEntity.Endpoint1;
        _endpoint2.transform.position = levelEntity.Endpoint2;
        _bat.transform.position = levelEntity.EntityPoint;
        _towardsPoint1 = levelEntity.TowardsPoint1;
    }
}
