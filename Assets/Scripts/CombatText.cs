using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _damageText;

    private Transform _target;

    public void OnInit(Transform target, float damage)
    {
        _target = target;
        _damageText.text = damage.ToString();
        Invoke(nameof(OnDespawn), 0.5f);
    }

    private void Start()
    {
        transform.position = Camera.main.WorldToScreenPoint(_target.transform.position + Vector3.up * 1.2f);
    }

    private void Update()
    {
        transform.position += Vector3.up * 200.0f * Time.deltaTime;
    }

    private void OnDespawn()
    {
        Destroy(gameObject);
    }
}
