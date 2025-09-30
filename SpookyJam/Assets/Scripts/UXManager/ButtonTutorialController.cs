using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines.Interpolators;
using UnityEngine.Windows;

public class ButtonTutorialController : MonoBehaviour, ILevelEntity
{
    [SerializeField] private SpriteRenderer _buttons;
    [SerializeField] private SpriteRenderer _arrow;
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _showFlip;
    private float _currentVal = 0;
    private float _endVal = 0;
    private float _speed = 4f;

    private void StartAnimation(bool isKeyboard)
    {
        _animator.Play("Default");
        if (_showFlip)
        {
            if (isKeyboard)
                _animator.SetTrigger("Space");
            else
                _animator.SetTrigger("XButton");
        }
        else
        {

            if (isKeyboard)
                _animator.SetTrigger("Alt");
            else
                _animator.SetTrigger("AButton");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentVal == _endVal)
            return;
        _currentVal = Mathf.Lerp(_currentVal, _endVal, _speed*Time.deltaTime);
        var color = _buttons.color;
        color.a = _currentVal;
        _buttons.color = color;
        _arrow.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<PlayerController>() != null)
        {
            var playerInput = collision.gameObject.GetComponent<PlayerInput>();
            if (playerInput.currentControlScheme == null)
                return;
            else if (playerInput.currentControlScheme == "Keyboard&Mouse")
                StartAnimation(true);
            else
                StartAnimation(false);

            _endVal = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<PlayerController>() != null)
        {
            _endVal = 0;
        }
    }
    public LevelEntity GetLevelEntity()
    {
        LevelEntity levelEntity = new LevelEntity();
        return GetLevelEntity(levelEntity);
    }

    public LevelEntity GetLevelEntity(LevelEntity levelEntity)
    {
        return levelEntity;
    }

    public void SetLevelEntity(LevelEntity levelEntity)
    {
    }
}
