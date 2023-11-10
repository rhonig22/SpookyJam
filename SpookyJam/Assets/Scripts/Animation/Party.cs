using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _waitTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartPartyJump());
    }

    IEnumerator StartPartyJump()
    {

        yield return new WaitForSeconds(_waitTime);
        _animator.SetTrigger("Party");
    }
}
