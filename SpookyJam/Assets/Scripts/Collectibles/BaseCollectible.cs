using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollectible : MonoBehaviour
{
    [SerializeField] private AudioClip _pumpkinSound;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private int _index = 0;
    private bool _collected = false;

    public int GetIndex()
    {
        return _index;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            PickupCollectible();
        }
    }

    private void PickupCollectible() {
        if (!_collected)
        {
            PumpkinManager.Instance.PickupCollectible(_index);
            SoundManager.Instance.PlaySound(_pumpkinSound, transform.position, 1f);
            _animator.SetTrigger("PickedUp");
            _particleSystem.Play();
            _collected = true;
        }
    }

    private void Finish()
    {
        Destroy(gameObject);
    }
}
