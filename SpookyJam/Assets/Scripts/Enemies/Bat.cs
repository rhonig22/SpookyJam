using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy, ILevelEntity
{
    [SerializeField] private GameObject _endpoint1;
    [SerializeField] private GameObject _endpoint2;
    [SerializeField] private GameObject _bat;
    [SerializeField] private bool _towardsPoint1 = true;

    private readonly float speed = 5f;
    
    private void Update()
    {
        var moveDirection = (_towardsPoint1 ? _endpoint1.transform.position : _endpoint2.transform.position) - _bat.transform.position;
        if (moveDirection.magnitude <= 0.1f)
            _towardsPoint1 = !_towardsPoint1;

        moveDirection = moveDirection.normalized * Time.deltaTime * speed;
        _bat.transform.position += moveDirection;
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
