using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _dialogueBox;
    [SerializeField] TextMeshProUGUI _textBox;
    private string _fullText, _currentText = "";
    private float _addLetterTime = .03f, _currentTime = 0, _dismissTime = 1f;
    private bool _showText = false;
    public UnityEvent DialogueFinished = new UnityEvent();

    private void Update()
    {
        if (!_showText)
        {
            return;
        }

        if (_fullText != null && _fullText != string.Empty)
        {
            if (_currentTime > _addLetterTime)
            {
                AddLetter();
                _currentTime = 0;
            }
            else
            {
                _currentTime += Time.deltaTime;
            }
        }
        else
        {
            if (_currentTime >= _dismissTime)
            {
                DialogueFinished.Invoke();
                DialogueFinished.RemoveAllListeners();
            }
            else
            {
                _currentTime += Time.deltaTime;
            }

        }
    }

    public void OpenDialogue()
    {
        _showText = false;
        _dialogueBox.SetActive(true);
        _animator.SetTrigger("Open");
    }

    private void FinishOpen()
    {
        _showText = true;
    }

    public void CloseDialogue()
    {
        _animator.SetTrigger("Close");
    }

    private void FinishClose()
    {
        _dialogueBox.SetActive(false);
    }

    public void SetText(string text)
    {
        _fullText = text;
        _currentTime = 0;
        _currentText = "";
        _textBox.text = _currentText;
    }

    private void AddLetter()
    {
        _currentText += _fullText[0];
        _fullText = _fullText.Remove(0, 1);
        _textBox.text = _currentText;
    }
}
