using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image _highlightImage;

    public void OnSelect(BaseEventData eventData)
    {
        _highlightImage.enabled = true;
    }

    public void OnDeselect(BaseEventData eventData)
    { 
        _highlightImage.enabled = false;
    }
}
