using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput = 0f, _movementSmoothing = .1f;
    private bool _facingRight = true, _inverted = false;
    private readonly int _speed = 6;
    private Vector2 _currentVelocity = Vector2.zero;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Rigidbody2D _playerRB;

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        // Control the sprite for the character
        if (_horizontalInput > 0 && !_facingRight)
        {
            _facingRight = true;
            // _animator?.SetBool("facingRight", true);
        }
        else if (_horizontalInput < 0 && _facingRight)
        {
            _facingRight = false;
            // _animator?.SetBool("facingRight", false);
        }

        if ( Input.GetButtonDown("Flip"))
        {
            _animator.SetTrigger("Flip");
        }
    }

    private void FixedUpdate()
    {
        Move(_horizontalInput * _speed * Time.fixedDeltaTime);
    }

    private void Move(float xSpeed)
    {
        Vector2 targetVelocity = new Vector2(xSpeed * 60f, _playerRB.velocity.y);
        // And then smoothing it out and applying it to the character
        _playerRB.velocity = Vector2.SmoothDamp(_playerRB.velocity, targetVelocity, ref _currentVelocity, _movementSmoothing);
    }

    public void InvertGravity()
    {
        _playerRB.gravityScale *= -1;
    }

    public void InvertCharacter()
    {
        Debug.Log("flip");
        _inverted = !_inverted;
        GameManager.Instance.FlipGravity();
        InvertGravity();
        transform.Rotate(Vector3.forward, 180f);
        transform.position += (_inverted ? -1 : 1) * Vector3.up;
    }
}