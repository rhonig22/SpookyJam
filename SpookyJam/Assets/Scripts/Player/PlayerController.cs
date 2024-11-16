using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput = 0f;
    private readonly float _floatGravityMultiplier = 4f, _movementSmoothing = .1f, _maxFloatFall = 3.5f,
        _topSpeed = 10f, _timeToTopSpeed = .2f, _degradeInertiaMultiplier = 6f;
    private bool _facingRight = true, _inverted = false, _grounded = false, _isShrinking = false, _isDead = false, _isFloating = false;
    private Vector2 _currentVelocity = Vector2.zero;
    [SerializeField] private Animator _animator;
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
        _animator.SetFloat("xMovement", _horizontalInput);

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

        Move(_horizontalInput);
        CapVelocity();
    }

    private void Move(float horizontalInput)
    {
        _playerRB.drag = 0;
        Vector3 targetVelocity = new Vector2(_topSpeed * horizontalInput, 0);
        Vector2 diffVelocity = new Vector2(targetVelocity.x - _playerRB.velocity.x, 0);
        if (targetVelocity.x == 0)
            diffVelocity.x *= _degradeInertiaMultiplier;
        _playerRB.AddForce(_playerRB.mass * diffVelocity / _timeToTopSpeed);
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
        GravityManager.Instance.FlipGravity();
        InvertGravity();
        FlipCharacter();
        transform.position += (_inverted ? -1 : 1) * Vector3.up;
    }

    public void StartShrink()
    {
        _animator.SetTrigger("Flip");
        _isShrinking = true;
        _playerRB.velocity = Vector2.zero;
        SoundManager.Instance.PlaySound(_invertClip, transform.position, 1.2f);
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
        if (!_isDead)
        {
            _isDead = true;
            _playerRB.gravityScale = 0;
            _playerRB.velocity = Vector2.zero;
            _animator.SetTrigger("Death");
            SoundManager.Instance.PlaySound(_deathClip, transform.position);
            CameraController.Instance.ShakeCamera();
        }
    }

    public void FinishDeath()
    {
        _playerRB.simulated = false;
        SceneTransition.Instance.RestartLevelTransition();
    }
}