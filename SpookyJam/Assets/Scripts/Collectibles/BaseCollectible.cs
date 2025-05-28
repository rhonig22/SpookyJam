using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollectible : MonoBehaviour, ILevelEntity
{
    [SerializeField] private AudioClip _pumpkinSound;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private SpriteRenderer _shadow;
    [SerializeField] private int _index = 0;
    private bool _collected = false;
    private bool _hidden = false;

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
        if (!_hidden && !_collected)
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
        SetCollectedState();
    }

    public void SetCollectedState()
    {
        transform.localScale = Vector3.one;
        _sprite.enabled = false;
        _shadow.enabled = true;
        _collected = true;
    }

    public LevelEntity GetLevelEntity()
    {
        LevelEntity collectibleEntity = new LevelEntity();
        collectibleEntity.Index = _index;
        return collectibleEntity;
    }

    public void SetLevelEntity(LevelEntity levelEntity)
    {
        _index = levelEntity.Index;
    }
}
