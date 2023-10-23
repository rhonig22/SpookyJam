using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            SceneTransition.Instance.EndLevelTransition();
        }
    }
}
