using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundary : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        var controller = collision.GetComponent<PlayerController>();
        if (controller != null )
        {
            controller.StartDeath();
        }
    }
}
