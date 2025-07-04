using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseConditionalTrigger : MonoBehaviour, ILevelEntity
{
    [SerializeField] protected string _conditionTag;
    private List<BaseCondition> _conditions = new List<BaseCondition>();
    protected bool _triggered = false;

    private void Start()
    {
        GetConditions();
    }

    // Update is called once per frame
    void Update()
    {
        int conditionCount = 0;
        foreach (BaseCondition condition in _conditions)
            if (condition.IsTriggered) conditionCount++;

        if (!_triggered && conditionCount == _conditions.Count)
            TriggerConditional();
    }

    protected void GetConditions()
    {
        var foundEntities = GameObject.FindGameObjectsWithTag(_conditionTag);
        foreach (var entity in foundEntities)
        {
            _conditions.Add(entity.GetComponent<BaseCondition>());
        }
    }

    protected virtual void TriggerConditional()
    {
        _triggered = true;
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
