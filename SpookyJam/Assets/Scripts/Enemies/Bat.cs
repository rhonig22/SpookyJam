using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    [SerializeField] private GameObject _endpoint1;
    [SerializeField] private GameObject _endpoint2;

    private readonly float speed = 5f;
    private bool _towardsPoint1 = true;

    private void Update()
    {
        var moveDirection = (_towardsPoint1 ? _endpoint1.transform.position : _endpoint2.transform.position) - transform.position;
        if (moveDirection.magnitude <= 0.1f)
            _towardsPoint1 = !_towardsPoint1;

        moveDirection = moveDirection.normalized * Time.deltaTime * speed;
        transform.position += moveDirection;
    }
}
