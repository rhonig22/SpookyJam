using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput = 0f;
    private readonly float _floatGravityMultiplier = 4f, _movementSmoothing = .1f, _maxFloatFall = 4f;
    private bool _facingRight = true, _inverted = false, _grounded = false, _isShrinking = false, _isDead = false, _isFloating = false;
    private readonly int _speed = 6;
    private Vector2 _currentVelocity = Vector2.zero;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Rigidbody2D _playerRB;
    [SerializeField] private AudioClip _invertClip;
    [SerializeField] private AudioClip _deathClip;

    // Update is called once per frame
    void Update()
    {
        if ( _isDead)
        {
            return;
        }

        if (Input.GetButtonDown("Float"))
        {
            StartFloat();
        }

        if (Input.GetButtonUp("Float"))
        {
            EndFloat();
        }

        _horizontalInput = Input.GetAxisRaw("Horizontal");

        if (_isShrinking)
        {
            return;
        }

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

        if (_grounded && Input.GetButtonDown("Flip"))
        {
            StartShrink();
        }
    }

    private void FixedUpdate()
    {
        if (_isShrinking)
        {
            return;
        }

        Move(_horizontalInput * _speed * Time.fixedDeltaTime);
        CapVelocity();
    }

    private void Move(float xSpeed)
    {
        Vector2 targetVelocity = new Vector2(xSpeed * 60f, _playerRB.velocity.y);
        // And then smoothing it out and applying it to the character
        _playerRB.velocity = Vector2.SmoothDamp(_playerRB.velocity, targetVelocity, ref _currentVelocity, _movementSmoothing);
    }

    private void CapVelocity()
    {
        var currentYVelocity = _playerRB.velocity.y;
        if (_isFloating && Mathf.Abs(currentYVelocity) > _maxFloatFall)
        {
            _playerRB.velocity = new Vector2(_playerRB.velocity.x, _maxFloatFall * currentYVelocity / Mathf.Abs(currentYVelocity));
        }
    }

    public void InvertGravity()
    {
        _inverted = !_inverted;
        _playerRB.gravityScale *= -1;
    }

    public void FlipCharacter()
    {
        transform.Rotate(Vector3.forward, 180f);
    }

    public void InvertCharacter()
    {
        GameManager.Instance.FlipGravity();
        InvertGravity();
        FlipCharacter();
        transform.position += (_inverted ? -1 : 1) * Vector3.up;
    }

    public void StartShrink()
    {
        _animator.SetTrigger("Flip");
        _isShrinking = true;
        _playerRB.velocity = Vector2.zero;
        _audioSource.clip = _invertClip;
        _audioSource.Play();
    }

    public void EndShrink()
    {
        _isShrinking = false;
    }

    public void StartFloat()
    {
        _playerRB.gravityScale /= _floatGravityMultiplier;
        _isFloating = true;
        _animator.SetBool("Float", true);
    }

    public void EndFloat()
    {
        _playerRB.gravityScale *= _floatGravityMultiplier;
        _isFloating = false;
        _animator.SetBool("Float", false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckGrounding(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _grounded = false;
    }

    private void CheckGrounding(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            _grounded |= Vector2.Angle(normal, transform.up) < 90;
        }
    }

    public void StartDeath()
    {
        _isDead = true;
        _animator.SetTrigger("Death");
        _audioSource.clip = _deathClip;
        _audioSource.Play();
        CameraController.Instance.ShakeCamera();
    }

    public void FinishDeath()
    {
        _playerRB.simulated = false;
        SceneTransition.Instance.RestartLevelTransition();
    }
}