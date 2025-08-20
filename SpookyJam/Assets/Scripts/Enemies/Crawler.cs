using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using UnityEngine;

public class Crawler : Enemy, ILevelEntity
{
    [SerializeField] Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveFrame()
    {
        transform.position -= transform.right;
        LayerMask mask = LayerMask.GetMask("Tiles");
        var front = transform.position - transform.right;
        var down = transform.position - transform.up;
        if (!GridManager.Instance.VisibleTilemapContainsPoint(down))
            RotateCounterClockwise();
        else if (GridManager.Instance.VisibleTilemapContainsPoint(front))
            _animator.SetTrigger("Clockwise");
    }

    public void RotateClockwise()
    {
        transform.Rotate(new Vector3(0, 0, -90));
    }

    public void RotateCounterClockwise()
    {
        _animator.SetTrigger("CounterClockwise");
        transform.Rotate(new Vector3(0, 0, 90));
    }

    public LevelEntity GetLevelEntity()
    {
        LevelEntity levelEntity = new LevelEntity();
        return GetLevelEntity(levelEntity);
    }

    public LevelEntity GetLevelEntity(LevelEntity levelEntity)
    {
        return levelEntity;
    }

    public void SetLevelEntity(LevelEntity levelEntity)
    {
    }
}
