using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image _buttonImage;
    [SerializeField] TextMeshProUGUI _buttonText;
    [SerializeField] Color _highlightTextColor;
    [SerializeField] Color _baseTextColor;
    [SerializeField] Sprite _highlightSprite;
    [SerializeField] Sprite _baseSprite;
    [SerializeField] AudioClip _clickSound;

    public void OnSelect(BaseEventData eventData)
    {
        _buttonText.color = _highlightTextColor;
        _buttonImage.sprite = _highlightSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _buttonText.color = _baseTextColor;
        _buttonImage.sprite = _baseSprite;
    }

    public void ClickSound()
    {
        SoundManager.Instance.PlaySound(_clickSound, transform.position);
    }
}
