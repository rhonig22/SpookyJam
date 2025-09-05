using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private int _wakeCounter = 10;

    public void SleepOrPounce()
    {
        var rand = Random.Range(0, 2);
        if (rand == 0)
            _animator.SetTrigger("Sleep");
        else
            _animator.SetTrigger("Pounce");
    }

    public void SleepOrWake()
    {
        var rand = Random.Range(0, _wakeCounter+1);
        if (rand == 0)
        {
            _animator.SetTrigger("Wake");
            _wakeCounter = 10;
        }
        else
            _wakeCounter--;
    }

    public void EndPounce()
    {
        transform.position -= Vector3.right;
    }
}
