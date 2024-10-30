using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] AudioClip _endClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            SceneTransition.Instance.EndLevelTransition();
            SoundManager.Instance.PlaySound(_endClip, transform.position, 1f);
            DataManager.Instance.LevelFinished();
        }
    }
}
