using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private float _timer;

    public void OnEnter(Enemy enemy)
    {
        if (enemy.Target != null)
        {
            enemy.StopMoving();
            enemy.Slash();
        }

        _timer = 0.0f;
    }

    public void OnExecute(Enemy enemy)
    {
        if (_timer > 1.5f)
        {
            enemy.ChangeState(new PatrolState());
        }

        _timer += Time.deltaTime;
    }

    public void OnFixedExecute(Enemy enemy)
    {
        
    }

    public void OnExit(Enemy enemy)
    {
        
    }
}
