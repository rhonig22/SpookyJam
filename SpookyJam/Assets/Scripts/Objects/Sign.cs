using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Sign : MonoBehaviour, ILevelEntity
{
    [SerializeField] private DialogueBox _dialogueBox;
    [SerializeField] private string _signText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            _dialogueBox?.SetText(_signText);
            _dialogueBox?.OpenDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            StartCoroutine(CloseSign());
        }
    }

    IEnumerator CloseSign()
    {
        yield return new WaitForSeconds(.5f);
        _dialogueBox?.CloseDialogue();
    }

    public LevelEntity GetLevelEntity()
    {
        LevelEntity messageEntity = new LevelEntity();
        messageEntity.Message = _signText;
        return messageEntity;
    }

    public void SetLevelEntity(LevelEntity levelEntity)
    {
        _signText = levelEntity.Message;
    }
}
