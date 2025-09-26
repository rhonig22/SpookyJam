using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] AudioClip _endClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player != null && !player.IsDead && !player.IsEnding)
        {
            player.EndLevel();
            StartCoroutine(EndLevelTransition());
            SoundManager.Instance.PlaySound(_endClip, transform.position, 1f);
            PumpkinManager.Instance.LevelFinished();
        }
    }

    private IEnumerator EndLevelTransition()
    {
        yield return new WaitForSeconds(.75f);
        SceneTransition.Instance.EndLevelTransition(() => GameManager.Instance.FinishLevel());
    }
}
