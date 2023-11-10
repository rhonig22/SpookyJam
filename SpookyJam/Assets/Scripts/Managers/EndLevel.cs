using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private AudioSource _AudioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            SceneTransition.Instance.EndLevelTransition();
            _AudioSource.Play();
            DataManager.Instance.LevelFinished();
        }
    }
}
