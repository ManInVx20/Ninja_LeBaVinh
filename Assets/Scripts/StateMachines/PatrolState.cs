using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private float _randomTime;
    private float _timer;

    public void OnEnter(Enemy enemy)
    {
        _randomTime = Random.Range(3.0f, 6.0f);
        _timer = 0.0f;
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.Target != null)
        {
            enemy.Rotate(enemy.transform.position.x < enemy.Target.transform.position.x);

            if (enemy.IsTargetInRange())
            {
                enemy.ChangeState(new AttackState());
            }
        }
        else
        {
            if (_timer >= _randomTime)
            {
                enemy.ChangeState(new IdleState());
            }
        }

        _timer += Time.deltaTime;
    }

    public void OnFixedExecute(Enemy enemy)
    {
        if (_timer < _randomTime)
        {
            enemy.KeepMoving();
        }
    }

    public void OnExit(Enemy enemy)
    {
        enemy.StopMoving();
    }
}
