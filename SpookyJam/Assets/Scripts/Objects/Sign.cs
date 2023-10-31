using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] private DialogueBox _dialogueBox;
    [SerializeField] private string _signText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            _dialogueBox?.gameObject.SetActive(true);
            _dialogueBox?.SetText(_signText);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            _dialogueBox?.gameObject.SetActive(false);
        }
    }
}
