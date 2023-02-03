using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    protected Rigidbody2D _rigidbody;
    [SerializeField]
    protected Animator _animator;

    [Header("Properties")]
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private HealthBar _healthBarPrefab;
    [SerializeField]
    private CombatText _combatText;

    private float _currentHealth;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;

    private HealthBar _healthBar;

    public bool IsDead => _currentHealth <= 0.0f;

    private Coroutine _generateHealthCoroutine;

    private string _currentAnimName;

    protected virtual void Start()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        _rigidbody.simulated = true;

        _currentHealth = _maxHealth;

        _healthBar = Instantiate(_healthBarPrefab, GameObject.Find("Canvas").transform);
        _healthBar.OnInit(this);
    }

    protected virtual void OnDespawn()
    {
        Destroy(_healthBar.gameObject);
    }

    protected virtual void OnDeath()
    {
        _rigidbody.simulated = false;

        ChangeAnimation("die");

        Invoke(nameof(OnDespawn), 2.0f);
    }

    protected void ChangeAnimation(string animName)
    {
        if (animName == null | animName == _currentAnimName)
        {
            return;
        }

        _animator.ResetTrigger(animName);
        _currentAnimName = animName;
        _animator.SetTrigger(_currentAnimName);
    }

    public void OnHit(float damage)
    {
        if (IsDead)
        {
            return;
        }

        Instantiate(_combatText, GameObject.Find("Canvas").transform).OnInit(transform, damage);

        _currentHealth = Mathf.Max(_currentHealth - damage, 0.0f);

        if (IsDead)
        {
            OnDeath();
        }
        else
        {
            if (_generateHealthCoroutine != null)
            {
                StopCoroutine(_generateHealthCoroutine);
            }

            _generateHealthCoroutine = StartCoroutine(GenerateHealthCoroutine(5.0f));
        }
    }

    private IEnumerator GenerateHealthCoroutine(float delayTime = 0.0f)
    {
        if (delayTime > 0.0f)
        {
            yield return new WaitForSeconds(delayTime);
        }

        while (_currentHealth < _maxHealth)
        {
            _currentHealth += 10.0f;

            yield return new WaitForSeconds(1.0f);
        }
    }
}
