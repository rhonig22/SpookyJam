using System.Collections;
using UnityEngine;

public class StartCinematic : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _player;
    [SerializeField] AudioClip _clip;

    private void Start()
    {
        if (GameManager.Instance.IsNewGame)
        {
            _spriteRenderer.enabled = true;
            _player.SetActive(false);
            StartCoroutine(TriggerAnimation());
        }
    }

    private IEnumerator TriggerAnimation()
    {
        yield return new WaitForSeconds(1f);
        _animator.SetTrigger("StartCinematic");
        // SoundManager.Instance.PlaySound(_clip, transform.position);
    }

    public void FinishStartingCinematic()
    {
        _player.SetActive(true);
        GameManager.Instance.ClearNewGame();
        _spriteRenderer.enabled = false;
    }
}
