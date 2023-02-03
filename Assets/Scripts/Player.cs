using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private bool _isGrounded;
    [SerializeField]
    private float _groundCheckDistance;
    [SerializeField]
    private LayerMask _groundLayers;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private Kunai _kunaiPrefab;
    [SerializeField]
    private Transform _throwPoint;
    [SerializeField]
    private SlashArea _slashArea;

    private float _horizonalInput;
    private bool _isJumping;
    private bool _isAttacking;

    private Vector3 _savePoint;
    private int _coins;

    protected override void Start()
    {
        base.Start();

        SavePoint();

        _coins = PlayerPrefs.GetInt("Coin", 0);

        UIManager.Instance.SetCoinText(_coins);
    }

    private void Update()
    {
        if (IsDead)
        {
            return;
        }

        CheckGrounded();

        // _horizonalInput = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(_horizonalInput) > 0.1f)
        {
            Rotate(_horizonalInput > 0.0f);
        }

        if (_isGrounded)
        {
            // if (_isJumping && _rigidbody.velocity.y < -1.0f)
            // {
            //     _isJumping = false;
            // }

            if (_isJumping || _isAttacking)
            {
                return;
            }

            if (Mathf.Abs(_horizonalInput) > 0.1f)
            {
                ChangeAnimation("run");
            }
            else
            {
                ChangeAnimation("idle");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Slash();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                Throw();
            }
        }
        else
        {
            if (_rigidbody.velocity.y < 0.0f)
            {
                Fall();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isAttacking)
        {
            _rigidbody.velocity = new Vector2(0.0f, _rigidbody.velocity.y);

            return;
        }

        _rigidbody.velocity = new Vector2(_horizonalInput * _moveSpeed * Time.deltaTime, _rigidbody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            _coins += 1;

            PlayerPrefs.SetInt("Coin", _coins);

            UIManager.Instance.SetCoinText(_coins);

            Destroy(other.gameObject);
        }

        if (other.tag == "DeathZone")
        {
            OnHit(MaxHealth);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();

        transform.position = _savePoint;

        ChangeAnimation("idle");
    }

    protected override void OnDespawn()
    {
        base.OnDespawn();

        OnInit();
    }

    public void SavePoint()
    {
        _savePoint = transform.position;
    }

    public void SetMove(float horizontal)
    {
        _horizonalInput = horizontal;
    }

    public void Rotate(bool isTurnRight)
    {
        transform.rotation = Quaternion.Euler(0.0f, isTurnRight ? 0.0f : 180.0f, 0.0f);
    }

    public void Jump()
    {
        if (!_isGrounded)
        {
            return;
        }

        _isJumping = true;

        _rigidbody.AddForce(Vector2.up * _jumpForce);

        ChangeAnimation("jump");
    }

    public void Fall()
    {
        _isJumping = false;

        ChangeAnimation("fall");
    }

    public void Slash()
    {
        _isAttacking = true;

        _slashArea.Activate();

        ChangeAnimation("slash");

        Invoke(nameof(ResetAttack), 0.3f);
    }

    public void Throw()
    {
        _isAttacking = true;

        Instantiate(_kunaiPrefab, _throwPoint.position, _throwPoint.rotation);

        ChangeAnimation("throw");

        Invoke(nameof(ResetAttack), 0.3f);
    }

    private void CheckGrounded()
    {
        Debug.DrawRay(transform.position, Vector2.down * _groundCheckDistance, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _groundLayers);

        _isGrounded = hit.collider != null;
    }

    private void ResetAttack()
    {
        _isAttacking = false;
    }
}
