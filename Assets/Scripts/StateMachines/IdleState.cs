using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private float _randomTime;
    private float _timer;

    public void OnEnter(Enemy enemy)
    {
        _randomTime = Random.Range(2.0f, 4.0f);
        _timer = 0.0f;
    }

    public void OnExecute(Enemy enemy)
    {
        if (_timer >= _randomTime)
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
