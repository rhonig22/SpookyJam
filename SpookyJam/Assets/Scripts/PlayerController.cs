using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput = 0f, _movementSmoothing = .05f;
    private bool _facingRight = true;
    private readonly int _speed = 8;
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
            _animator?.SetBool("facingRight", true);
        }
        else if (_horizontalInput < 0 && _facingRight)
        {
            _facingRight = false;
            _animator?.SetBool("facingRight", false);
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
        _playerRB.velocity = Vector2.SmoothDamp(_playerRB.velocity, targetVelocity, ref _currentVelocity, Math.Abs(_currentVelocity.x) < Math.Abs(targetVelocity.x) ? _movementSmoothing : 0);
    }
}