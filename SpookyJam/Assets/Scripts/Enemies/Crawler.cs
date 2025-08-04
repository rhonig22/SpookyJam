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
        Collider2D collideFront = Physics2D.OverlapPoint(transform.position - transform.right, mask);
        Collider2D collideDown = Physics2D.OverlapPoint(transform.position - transform.up, mask);
        if (collideFront != null)
            _animator.SetTrigger("Clockwise");
        else if (collideDown == null)
            RotateCounterClockwise();
    }

    public void RotateClockwise()
    {
        transform.Rotate(new Vector3(0, 0, -90));
    }

    public void RotateCounterClockwise()
    {
        transform.Rotate(new Vector3(0, 0, 90));
        _animator.SetTrigger("CounterClockwise");
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
