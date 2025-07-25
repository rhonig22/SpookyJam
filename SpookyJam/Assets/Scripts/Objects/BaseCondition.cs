using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCondition : MonoBehaviour, ILevelEntity
{
    [SerializeField] private string _conditionTag;
    [SerializeField] private Animator _animator;
    public bool IsTriggered { get; private set; } = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsTriggered && collision.CompareTag("Ghost"))
        {
            IsTriggered = true;
            _animator.SetTrigger("Triggered");
        }
    }

    public LevelEntity GetLevelEntity()
    {
        LevelEntity levelEntity = new LevelEntity();
        return GetLevelEntity(levelEntity);
    }

    public LevelEntity GetLevelEntity(LevelEntity levelEntity)
    {
        levelEntity.Tag = _conditionTag;
        return levelEntity;
    }

    public void SetLevelEntity(LevelEntity levelEntity)
    {
        _conditionTag = levelEntity.Tag;
    }
}
