using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform[] _points;
    [SerializeField]
    private float _moveSpeed;

    private int _sourceIndex;
    private int _targetIndex;

    private void Start()
    {
        _sourceIndex = 0;
        _targetIndex = (_sourceIndex + 1) % _points.Length;
        transform.position = _points[_sourceIndex].position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _points[_targetIndex].position, _moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _points[_targetIndex].position) < 0.01f)
        {
            _sourceIndex = _targetIndex;
            _targetIndex = (_sourceIndex + 1) % _points.Length;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }
}
