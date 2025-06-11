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
    protected bool _collected = false;
    protected bool _hidden = false;

    public int GetIndex()
    {
        return _index;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.GetComponent<PlayerController>();
        if (controller != null && !_hidden && !_collected)
        {
            PickupCollectible();
        }
    }

    protected virtual void PickupCollectible()
    {
        PumpkinManager.Instance.PickupCollectible(_index);
        SoundManager.Instance.PlaySound(_pumpkinSound, transform.position, 1f);
        _animator.SetTrigger("PickedUp");
        _particleSystem.Play();
        _collected = true;
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
        LevelEntity levelEntity = new LevelEntity();
        return GetLevelEntity(levelEntity);
    }

    public LevelEntity GetLevelEntity(LevelEntity levelEntity)
    {
        levelEntity.Index = _index;
        return levelEntity;
    }

    public void SetLevelEntity(LevelEntity levelEntity)
    {
        _index = levelEntity.Index;
    }
}
