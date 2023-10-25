using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartPartyJump());
    }

    IEnumerator StartPartyJump()
    {

        yield return new WaitForSeconds(2);
        _animator.SetTrigger("Party");
    }
}
