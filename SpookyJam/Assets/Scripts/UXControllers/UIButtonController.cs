using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image _highlightImage;
    [SerializeField] Color _highlightColor;
    [SerializeField] Color _baseColor;

    public void OnSelect(BaseEventData eventData)
    {
        _highlightImage.color = _highlightColor;
    }

    public void OnDeselect(BaseEventData eventData)
    { 
        _highlightImage.color = _baseColor;
    }
}
