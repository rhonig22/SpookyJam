using UnityEngine;

public class Candle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _litCandle;
    [SerializeField] private Sprite _unlitCandle;
    private bool _isLit = false;

    public void LightCandle()
    {
        _isLit = true;
        _spriteRenderer.sprite = _litCandle;
    }

    public bool GetIsLit()
    {
        return _isLit;
    }
}
