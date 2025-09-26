using UnityEngine;

public class Candle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _litCandle;
    [SerializeField] private Sprite _unlitCandle;
    [SerializeField] private AudioClip _candleSound;
    private bool _isLit = false;

    public void LightCandle()
    {
        if (_isLit)
            return;

        _isLit = true;
        _spriteRenderer.sprite = _litCandle;
        SoundManager.Instance.PlaySound(_candleSound, transform.position, 1f);
    }

    public bool GetIsLit()
    {
        return _isLit;
    }
}
