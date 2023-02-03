using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private SlashArea _slashArea;

    private IState _currentState;

    public Character Target { get; private set; }

    private void Update()
    {
        if (IsDead)
        {
            return;
        }
        
        _currentState?.OnExecute(this);
    }

    private void FixedUpdate()
    {
        if (IsDead)
        {
            return;
        }

        _currentState?.OnFixedExecute(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyWall")
        {
            Rotate(transform.position.x > other.transform.position.x);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();

        ChangeState(new IdleState());
    }

    protected override void OnDespawn()
    {
        base.OnDespawn();

        Destroy(gameObject);
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        ChangeState(null);
    }

    public void ChangeState(IState newState)
    {
        _currentState?.OnExit(this);
        _currentState = newState;
        _currentState?.OnEnter(this);
    }

    public void Rotate(bool isTurnRight)
    {
        transform.rotation = Quaternion.Euler(0.0f, isTurnRight ? 0.0f : 180.0f, 0.0f);
    }

    public void KeepMoving()
    {
        _rigidbody.velocity = new Vector2(transform.right.x * _moveSpeed * Time.deltaTime, _rigidbody.velocity.y);

        ChangeAnimation("run");
    }

    public void StopMoving()
    {
        _rigidbody.velocity = new Vector2(0.0f, _rigidbody.velocity.y);

        ChangeAnimation("idle");
    }

    public void Slash()
    {
        _slashArea.Activate();

        ChangeAnimation("slash");
    }

    public bool IsTargetInRange()
    {
        return Vector2.Distance(transform.position, Target.transform.position) <= _attackRange;
    }

    public void SetTarget(Character character)
    {
        if (IsDead)
        {
            return;
        }

        Target = character;

        if (Target != null)
        {
            if (IsTargetInRange())
            {
                ChangeState(new AttackState());
            }
            else
            {
                ChangeState(new PatrolState());
            }
        }
        else
        {
            ChangeState(new IdleState());
        }
    }
}
