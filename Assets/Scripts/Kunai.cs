using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private GameObject _hitVFXPrefab;

    private void Start()
    {
        _rigidbody.velocity = transform.right * 5.0f;

        Invoke(nameof(Reset), 2.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            other.GetComponent<Character>().OnHit(25.0f);
            Instantiate(_hitVFXPrefab, transform.position, transform.rotation);
            Reset();
        }
    }

    private void Reset()
    {
        Destroy(gameObject);
    }
}
