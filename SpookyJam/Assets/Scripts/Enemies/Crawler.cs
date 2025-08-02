using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using UnityEngine;

public class Crawler : Enemy, ILevelEntity
{
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
        Collider2D col = Physics2D.OverlapPoint(transform.position - transform.right, mask);
        if (col != null)
            transform.Rotate(new Vector3(0, 0, -90));
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
