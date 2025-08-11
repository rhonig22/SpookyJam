using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput = 0f;
    private readonly float _originalGravity = 2f, _floatGravityMultiplier = 2f, _maxFloatFall = -2f,
        _topSpeed = 10f, _timeToTopSpeed = .2f, _degradeInertiaMultiplier = 6f, _horizontalThreshold = .2f;
    private bool _inverted = false, _grounded = false, _isShrinking = false, _isFloating = false, _isInVoid = false;
    private Vector2 _currentVelocity = Vector2.zero;
    [SerializeField] private Animator _animator;
    [SerializeField] private RuntimeAnimatorController _cutSceneController;
    [SerializeField] private Rigidbody2D _playerRB;
    [SerializeField] private AudioClip _invertClip;
    [SerializeField] private AudioClip _deathClip;
    [SerializeField] private AudioClip _landingClip;
    [SerializeField] private AudioClip _cantFlipClip;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private ParticleSystem _particleSystem;
    public bool IsDead { get; private set; } = false;
    public bool IsEnding { get; private set; } = false;

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
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

        if (_grounded && Input.GetButtonDown("Flip"))
        {
            CanFlip();
        }

        _animator.SetFloat("xMovement", (_inverted && !_isInVoid ? -1 : 1) * _horizontalInput);
        SetMovementParticles();
    }

    private void FixedUpdate()
    {
        if (_isShrinking || IsDead)
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
        if (_isFloating && ((_inverted && !_isInVoid ? -1 : 1) * currentYVelocity) < _maxFloatFall)
        {
            _playerRB.velocity = new Vector2(_playerRB.velocity.x, (_inverted && !_isInVoid ? -1 : 1) * _maxFloatFall);
        }
    }

    public void EnterVoid()
    {
        _isInVoid = true;
        InvertGravity();
    }

    public void ExitVoid()
    {
        _isInVoid = false;
        InvertGravity();
    }

    public void InvertGravity()
    {
        _inverted = !_inverted;
        _playerRB.gravityScale = (_inverted ? -1 : 1) * _originalGravity;
    }

    public void FlipCharacter()
    {
        transform.Rotate(Vector3.forward, 180f);
    }

    public void InvertCharacter()
    {
        GridManager.Instance.FlipGravity();
        transform.position += (!_inverted ? -1 : 1) * Vector3.up;
        InvertGravity();
        FlipCharacter();
    }

    public void CanFlip()
    {
        var testPoint = transform.position + (!_inverted ? -1 : 1) * Vector3.up;
        if (GridManager.Instance.TilemapContainsPoint(testPoint))
        {
            StartShrink();
        }
        else
        {
            SoundManager.Instance.PlaySound(_cantFlipClip, transform.position, 1f);
            _animator.SetTrigger("CantShrink");
        }
    }

    public void StartShrink()
    {
        _animator.SetTrigger("Flip");
        _isShrinking = true;
        _playerRB.velocity = Vector2.zero;
        SoundManager.Instance.PlaySound(_invertClip, transform.position, .6f);
    }

    public void EndShrink()
    {
        _isShrinking = false;
    }

    public void StartFloat()
    {
        if (_isInVoid)
            _playerRB.gravityScale *= _floatGravityMultiplier;
        _isFloating = true;
        _animator.SetBool("Float", true);
        _trailRenderer.emitting = true;
    }

    public void EndFloat()
    {
        if (_isInVoid)
            _playerRB.gravityScale = (_inverted ? -1 : 1) * _originalGravity;
        _isFloating = false;
        _animator.SetBool("Float", false);
        _trailRenderer.emitting = false;
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
        var _startGrounded = _grounded;
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            _grounded |= Vector2.Angle(normal, transform.up) < 90;
        }

        if (!_startGrounded && _grounded)
            SoundManager.Instance.PlaySound(_landingClip, transform.position, 1f);
    }

    public void EndLevel()
    {
        IsEnding = true;
        CameraController.Instance.SetFocusCamera();
        _animator.runtimeAnimatorController = _cutSceneController;
        _animator.SetTrigger("Swirl");
        _playerRB.gravityScale = 0;
        _playerRB.velocity = Vector2.zero;
    }

    public void StartDeath()
    {
        if (!IsDead)
        {
            IsDead = true;
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

    private void SetMovementParticles()
    {
        if (Math.Abs(_playerRB.velocity.magnitude) < _horizontalThreshold)
        {
            if (_particleSystem.isEmitting)
                _particleSystem.Stop();

            return;
        }


        if (!_particleSystem.isEmitting)
            _particleSystem.Play();
    }
}