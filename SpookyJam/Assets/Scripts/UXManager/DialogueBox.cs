using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textBox;
    private string _fullText, _currentText = "";
    private float _addLetterTime = .03f, _currentTime = 0, _dismissTime = 1f;
    public UnityEvent DialogueFinished = new UnityEvent();

    private void Update()
    {
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
