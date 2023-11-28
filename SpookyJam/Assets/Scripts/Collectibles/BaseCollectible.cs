using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollectible : MonoBehaviour
{
    [SerializeField] private string _collectibleName = "Pumpkin";
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particleSystem;
    private bool _collected = false;

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
            DataManager.Instance.PickupCollectible(_collectibleName);
            _audioSource.Play();
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
